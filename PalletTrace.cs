using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO.Ports;
using System.Management;
using System.Data;
using Emgu.CV.Rapid;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing.Text;


namespace PalletTrace
{
    public partial class PalletTrace : Form
    {
        #region Fields
        private Rfid rfid;
        private Pallet pallet;
        private CameraHandler cameraHandler;
        private PalletDetector palletDetector;
        private ExcludedAreaHandler excludeAreaHandler;
        private PalletDrawer palletDrawer;
        private Point startPoint;
        private Rectangle currentRect;
        private bool palletDetected;
        private bool isDrawing;
      
        #endregion

        #region Constructor
        public PalletTrace()
        {
            
            InitializeComponent();
            int tolerance = Convert.ToInt32(ConfigurationManager.AppSettings["tolerance"]);
            int targetHeight = Convert.ToInt32(ConfigurationManager.AppSettings["targetHeight"]);
            int targetWidth = Convert.ToInt32(ConfigurationManager.AppSettings["targetWidth"]);
            rfid = new Rfid();
            pallet = new Pallet();
            cameraHandler = new CameraHandler();
            palletDetector = new PalletDetector(targetWidth, targetHeight, tolerance);
            excludeAreaHandler = new ExcludedAreaHandler();
            palletDrawer = new PalletDrawer();
            palletDetected = false;
            isDrawing = false;
            LoadAvailableCameras();
            LoadRfidComPorts();
            cboFrameRate.SelectedIndex = 4;
            cboAvailableCameras.SelectedIndex = cboAvailableCameras.Items.Count - 1;
            if (cboRfidComPort.Items.Count > 0)
            {
                cboRfidComPort.SelectedIndex = 0;
                StartRfidReader();
            }
            StartCamera();

            cboFrameRate.SelectedIndexChanged += cboFrameRate_SelectedIndexChanged;
            cboAvailableCameras.SelectedIndexChanged += cboAvailableCameras_SelectedIndexChanged;
            cboRfidComPort.SelectedIndexChanged += cboRfidComPort_SelectedIndexChanged;
            cameraHandler.CameraDevicesChanged += Camera_CameraDevicesChangedHandler;
            rfid.RfidDevicesChanged += Rfid_RfidDevicesChangedHandler;
            rfid.RfidReaderDisabled += Rfid_RfidReaderDisabledHandler;
            rfid.RfidReaderEnabled += Rfid_RfidReaderEnabledHandler;
            pallet.UnknownRfidTag += Pallet_UnknownRfidTagHandler;
            rfid.TagRead += Rfid_TagReadHandler;
        }
        #endregion

        #region Methods
        private void LoadAvailableCameras()
        {
            List<string> availableCameras = cameraHandler.GetAvailableCameras();
            cboAvailableCameras.Items.Clear();
            cboAvailableCameras.Items.AddRange(availableCameras.ToArray());
        }

        private void LoadRfidComPorts()
        {
            List<string> comPorts = rfid.GetAvailableRfidReaders();
            cboRfidComPort.Items.Clear();
            cboRfidComPort.Items.AddRange(comPorts.ToArray());
        }

        private void StartRfidReader()
        {
            int baudRate, tagIdLength;
            string portName;

            baudRate = 2400;
            tagIdLength = 10;
            portName = cboRfidComPort.Text;
            rfid.OpenSerialPort(portName, baudRate, tagIdLength);
        }

        private void StartCamera()
        {
            cameraHandler.SelectedCamera = cboAvailableCameras.SelectedIndex;
            cameraHandler.FrameRate = Convert.ToInt32(cboFrameRate.Text);
            cameraHandler.OpenVideoStream();
            tmrFrameUpdate.Interval = 1000 / cameraHandler.FrameRate;
            tmrFrameUpdate.Stop();
            tmrFrameUpdate.Start();
        }



        private void HideSettingPanel()
        {
            pnlSettings.Visible = false;
            pcbSettings.BackColor = PalletTrace.DefaultBackColor;
        }

        private void ShowSettingPanel()
        {
            pnlSettings.Visible = true;
            pcbSettings.BackColor = Color.White;
        }
        #endregion

        #region TimerTickEventHandlers
        // Updates image in picturbox
        private async void FrameUpdate_Tick(object sender, EventArgs e)
        {
            try
            {
                if (palletDetected)
                {
                    await Task.Run(() => cameraHandler.DisplayFrame(pcbLivePicture, palletDetector, 
                                                                    excludeAreaHandler, palletDrawer, pallet));
                }
                else
                {
                    await Task.Run(() => cameraHandler.DisplayFrame(pcbLivePicture, palletDetector,
                                                                    excludeAreaHandler, palletDrawer));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void tmrResetPalletDetected_Tick(object sender, EventArgs e)
        {
            palletDetected = false;
            tmrResetPalletDetected.Stop();
        }

        private void tmrInformationVisible_Tick(object sender, EventArgs e)
        {
            lblInformation.Visible = false;
        }
        #endregion

        #region InternalEventHandlers
        private void Rfid_TagReadHandler(object sender, string tagId)
        {
            pallet.GetPalletAndOrientationWithProducts(tagId);
            palletDetected = true;
            Invoke(new Action(() =>
            {
                tmrResetPalletDetected.Stop();
                tmrResetPalletDetected.Start();
            }));
        }

        private void Rfid_RfidReaderDisabledHandler(object sender, EventArgs e)
        {
            lblInformation.Text = "Ingen tilgjengelige enheter funnet (COM).";
            lblInformation.ForeColor = Color.DarkRed;
            lblInformation.Visible = true;
        }

        private void Rfid_RfidReaderEnabledHandler(object sender, EventArgs e)
        {
            lblInformation.Visible = false;
        }

        private void Rfid_RfidDevicesChangedHandler(object sender, EventArgs e)
        {
            try
            {
                Invoke(new Action(() =>
                {
                    LoadRfidComPorts();
                    if (cboRfidComPort.Items.Count > 0)
                    {
                        cboRfidComPort.SelectedIndex = 0;
                        StartRfidReader();
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Pallet_UnknownRfidTagHandler(object sender, EventArgs e)
        {
            try
            {
                Invoke(new Action(() =>
                {
                    lblInformation.Text = "Kjenner ikke igjen pall";
                    lblInformation.Visible = true;
                    lblInformation.ForeColor = Color.DarkRed;
                    tmrInformationVisible.Stop();
                    tmrInformationVisible.Start();
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Camera_CameraDevicesChangedHandler(object sender, EventArgs e)
        {
            try
            {
                Invoke(new Action(() =>
                {
                    tmrFrameUpdate.Stop();
                    cameraHandler.Connected = false;
                    LoadAvailableCameras();
                    Thread.Sleep(800);
                    cboAvailableCameras.SelectedIndex = cboAvailableCameras.Items.Count - 1;
                    StartCamera();
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        #endregion

        #region GUIEventHandlers
        private void pcbSettings_Click(object sender, EventArgs e)
        {
            if (pnlSettings.Visible)
            {
                HideSettingPanel();
            }
            else
            {
                ShowSettingPanel();
            }
        }

        private void pcbSettings_MouseEnter(object sender, EventArgs e)
        {
            pcbSettings.Image = Properties.Resources.setting_icon_rotate;
        }

        private void pcbSettings_MouseLeave(object sender, EventArgs e)
        {
            pcbSettings.Image = Properties.Resources.setting_icon;
        }

        private void cboFrameRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            StartCamera();
        }

        private void cboAvailableCameras_SelectedIndexChanged(object sender, EventArgs e)
        {
            StartCamera();
        }

        private void cboRfidComPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            StartRfidReader();
        }

        private void pcbLivePicture_MouseDown(object sender, MouseEventArgs e)
        {
            HideSettingPanel();
            isDrawing = true;
            startPoint = e.Location;
            if (isDrawing)
            {
                currentRect = new Rectangle(Math.Min(startPoint.X, e.X), Math.Min(startPoint.Y, e.Y), Math.Abs(startPoint.X - e.X), Math.Abs(startPoint.Y - e.Y));
                pcbLivePicture.Invalidate();
            }
        }

        private void pcbLivePicture_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                currentRect = new Rectangle(Math.Min(startPoint.X, e.X), Math.Min(startPoint.Y, e.Y), Math.Abs(startPoint.X - e.X), Math.Abs(startPoint.Y - e.Y));
                pcbLivePicture.Invalidate();
            }
        }

        private void pcbLivePicture_MouseUp(object sender, MouseEventArgs e)
        {
            Rectangle scaledRect;
            int imageWidth, imageHeight;
            float scaleX, scaleY;
            isDrawing = false;

            if (pcbLivePicture.Image != null)
            {
                imageWidth = cameraHandler.FrameWidth;
                imageHeight = cameraHandler.FrameHeight;
                scaleX = (float)imageWidth / pcbLivePicture.Width;
                scaleY = (float)imageHeight / pcbLivePicture.Height;

                scaledRect = new Rectangle((int)(currentRect.X * scaleX), (int)(currentRect.Y * scaleY), (int)(currentRect.Width * scaleX), (int)(currentRect.Height * scaleY));
                excludeAreaHandler.ExcludedRegion = scaledRect;
            }
        }

        private void pcbLivePicture_Paint(object sender, PaintEventArgs e)
        {
            if (isDrawing && currentRect != Rectangle.Empty)
            {
                Pen pen = new Pen(Color.Red, 2);
                e.Graphics.DrawRectangle(pen, currentRect);
            }
        }

        private void chkShowExcludedRegion_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowExcludedRegion.Checked)
            {
                excludeAreaHandler.ShowExcludedRegion = true;
            }
            else
            {
                excludeAreaHandler.ShowExcludedRegion = false;
            }
        }

        private void chkShowPalletDimentions_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPalletDimentions.Checked)
            {
                palletDrawer.ShowPalletDimensions = true;
            }
            else
            {
                palletDrawer.ShowPalletDimensions = false;
            }
        }

        private void PalletTrace_MouseUp(object sender, MouseEventArgs e)
        {
            if (pnlSettings.Visible)
            {
                HideSettingPanel();
            }
        }
        #endregion
    }
}