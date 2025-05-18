using DirectShowLib;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace PalletTrace
{
    internal class CameraHandler
    {
        #region Fields
        private int selectedCamera;
        private int framerate;
        private int frameHeight;
        private int frameWidth;
        private bool connected;
        private VideoCapture videoStream;
        private ManagementEventWatcher insertWatcher;
        private ManagementEventWatcher removeWatcher;
        #endregion

        #region Constructor
        public CameraHandler()
        {
            framerate = 30;
            selectedCamera = 0;
            StartCameraDeviceWatchers();
        }
        #endregion

        #region Properties
        public int FrameRate 
        {
            get { return framerate; }
            set { framerate = value; }
        }

        public int SelectedCamera
        {
            get { return selectedCamera; }
            set { selectedCamera = value; }
        }

        public int FrameHeight
        {
            get { return frameHeight; }
        }

        public int FrameWidth
        {
            get { return frameWidth; }
        }
        public bool Connected
        {
            get { return connected; }
            set { connected = value; }
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when connected cameras has changed
        /// </summary>
        public event EventHandler CameraDevicesChanged;
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns list of available cameras using DirectShowlib.
        /// </summary>
        /// <returns></returns>
        public List<string> GetAvailableCameras()
        {
            List<string> cameraList = new List<string>();
            DsDevice[] systemCameras = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            foreach (DsDevice camera in systemCameras)
            {
                cameraList.Add(camera.Name);
            }
            return cameraList;
        }

        /// <summary>
        /// Creates a capture using Emgu.CV2.
        /// Restarts capture if it already exists.
        /// </summary>
        public void OpenVideoStream()
        {
            try
            {
                videoStream?.Dispose();
                videoStream = new VideoCapture(selectedCamera, VideoCapture.API.DShow);
                if (videoStream.IsOpened)
                {
                    Connected = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Feil oppsto: " + ex.Message);
            }
        }

        /// <summary>
        /// Displays frame in picturebox.Outlines and draws 2x3 grid with serialnumbers on pallet if a pallet is detected.
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <param name="targetWidth"></param>
        /// <param name="targetHeight"></param>
        /// <param name="tolerance"></param>
        /// <param name="serialNumbers"></param>
        /// <param name="orientation"></param>
        public void DisplayFrame(PictureBox pictureBox, PalletDetector palletDetector, ExcludedAreaHandler excludedAreaHandler, PalletDrawer palletDrawer, Pallet pallet)
        {
            Mat image;
            RotatedRect? detectedPallet;
            try
            {
                if (!connected)
                {
                    return;
                }
                if (videoStream != null && videoStream.IsOpened)
                {
                    image = new Mat();

                    videoStream.Retrieve(image);
                    detectedPallet = palletDetector.DetectPallet(image, excludedAreaHandler);
                    if (detectedPallet.HasValue)
                    {
                        palletDrawer.DrawRotatedGridWithSerialNumbers(image, detectedPallet.Value, 2, 3, pallet.Products, pallet.Orientation);
                    }
                    this.frameHeight = image.Height;
                    this.frameWidth = image.Width;

                    pictureBox.Image = image.ToBitmap();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
          
        }

        /// <summary>
        /// Displays frame in picturebox.Outlines and draws 2x3 grid on pallet if a pallet is detected.
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <param name="targetWidth"></param>
        /// <param name="targetHeight"></param>
        /// <param name="tolerance"></param>
        public void DisplayFrame(PictureBox pictureBox, PalletDetector palletDetector, ExcludedAreaHandler excludeAreaHandler, PalletDrawer palletDrawer)
        {
         
            Mat image;
            RotatedRect? pallet;
            try
            {
                if (!connected)
                {
                    return;
                }
                if (videoStream != null && videoStream.IsOpened)
                {
                    image = new Mat();
                    videoStream.Retrieve(image);
                    pallet = palletDetector.DetectPallet(image, excludeAreaHandler);
                    if (pallet.HasValue)
                    {
                        palletDrawer.DrawRotatedGrid(image, pallet.Value, 2, 3);
                    }
                    this.frameHeight = image.Height;
                    this.frameWidth = image.Width;

                    pictureBox.Image = image.ToBitmap();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "OpenCV: could not retrieve channel 0")  {}
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        #endregion

        #region Private Methods
        // Starts monitoring change of connected cameradevices and triggers Eventhandler on change.
        private void StartCameraDeviceWatchers()
        {
            try
            {
                WqlEventQuery insertQuery = new WqlEventQuery(
                    "SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity' AND (TargetInstance.Description LIKE '%Camera%' OR TargetInstance.Description LIKE '%Video%')"
                );
                insertWatcher = new ManagementEventWatcher(insertQuery);
                insertWatcher.EventArrived += (sender, args) => OnCameraDevicesChanged();
                insertWatcher.Start();

                WqlEventQuery removeQuery = new WqlEventQuery(
                    "SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity' AND (TargetInstance.Description LIKE '%Camera%' OR TargetInstance.Description LIKE '%Video%')"
                );
                removeWatcher = new ManagementEventWatcher(removeQuery);
                removeWatcher.EventArrived += (sender, args) => OnCameraDevicesChanged();
                removeWatcher.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Klarte ikke å starte kamera-overvåker: " + ex.Message);
            }
        }

        //Fires Event
        private void OnCameraDevicesChanged()
        {
            try
            {
                CameraDevicesChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}