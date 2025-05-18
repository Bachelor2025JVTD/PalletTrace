using DirectShowLib;
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PalletTrace
{
    internal class Rfid
    {
        #region Fields
        private ManagementEventWatcher insertWatcher;
        private ManagementEventWatcher removeWatcher;
        private SerialPort serialPort;
        private StringBuilder inputBuffer;
        private DateTime lastReadTime;
        private TimeSpan debounceTime;
        private int tagLength;
        private int baudRate;
        private string latestTagId;
        #endregion

        #region Constructor
        public Rfid()
        {
            LatestTagId = string.Empty;
            inputBuffer = new StringBuilder();
            lastReadTime = DateTime.MinValue;
            debounceTime = TimeSpan.FromSeconds(2);
            StartRfidDeviceWatchers();
        }
        #endregion

        #region Properties
        public int BaudRate
        {
            get { return baudRate; }
            set { baudRate = value; }
        }

        public int TagLength
        {
            get { return tagLength; }
            set
            {
                tagLength = value;
            }
        }

        public string LatestTagId
        {
            get { return latestTagId; }
            private set
            {
                if (latestTagId != value)
                {
                    latestTagId = value;
                }
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when RFID-tags are scanned
        /// </summary>
        public event EventHandler<string> TagRead;
        /// <summary>
        /// Occurs when connected Serial-devices has changed.
        /// </summary>
        public event EventHandler RfidDevicesChanged;
        /// <summary>
        /// Occurs when there is no Serial-devices is connected.
        /// </summary>
        public event EventHandler RfidReaderDisabled;
        /// <summary>
        /// Occurs if more than 0 Serial-devices is connected
        /// </summary>
        public event EventHandler RfidReaderEnabled;
        #endregion

        #region PublicMethods
        /// <summary>
        /// Opens and start receiving from given serial port
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="baudRate"></param>
        /// <param name="tagIdLength"></param>
        public void OpenSerialPort(string portName, int baudRate, int tagIdLength)
        {
            BaudRate = baudRate;
            TagLength = tagIdLength;
            serialPort?.Dispose();
            serialPort = new SerialPort(portName, this.baudRate, Parity.None, 8, StopBits.One);
            serialPort.DataReceived += RfidReader;

            try
            {
                serialPort.Open();
                serialPort.DtrEnable = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Feil ved åpning av seriell port: " + ex.Message);
            }
        }

        /// <summary>
        /// Search for and returns List of available serial ports.
        /// </summary>
        /// <returns></returns>
        public List<string> GetAvailableRfidReaders()
        {
            List<string> rfidList;
            string[] availablePorts;

            rfidList = new List<string>();

            try
            {
                availablePorts = SerialPort.GetPortNames();
                if (availablePorts.Length > 0)
                {
                    rfidList.AddRange(availablePorts);
                    RfidReaderEnabled?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    RfidReaderDisabled?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Feil ved lasting av COM-porter: " + ex.Message);
            }
            return rfidList;
        }
        #endregion

        #region PrivateMethods
        //Reads RFID-tags starting with \n, ending with \r and triggers EventHandler
        private void RfidReader(object sender, SerialDataReceivedEventArgs e)
        {
            DateTime dateTimeNow;
            string data, fullTag;
            Task.Run(() =>
            {
                try
                {
                    data = serialPort.ReadExisting();
                    inputBuffer.Append(data);
                    if (inputBuffer.ToString().Contains("\n") && inputBuffer.ToString().Contains("\r"))
                    {
                        fullTag = inputBuffer.ToString().Trim();
                        inputBuffer.Clear();

                        dateTimeNow = DateTime.Now;
                        if (fullTag == latestTagId && (dateTimeNow - lastReadTime) < debounceTime)
                        {
                            return;
                        }
                        latestTagId = fullTag;
                        lastReadTime = dateTimeNow;
                        TagRead?.Invoke(this, latestTagId);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            });
        }

        // Starts watching for changes in connected serial devices and triggers EventHandler on change.
        private void StartRfidDeviceWatchers()
        {
            try
            {
                WqlEventQuery insertQuery = new WqlEventQuery(
                "SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity' AND TargetInstance.Description LIKE '%Serial%'");
                insertWatcher = new ManagementEventWatcher(insertQuery);
                insertWatcher.EventArrived += (sender, args) => OnRfidDevicesChanged();
                insertWatcher.Start();

                WqlEventQuery removeQuery = new WqlEventQuery(
                "SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity' AND TargetInstance.Description LIKE '%Serial%'");
                removeWatcher = new ManagementEventWatcher(removeQuery);
                removeWatcher.EventArrived += (sender, args) => OnRfidDevicesChanged();
                removeWatcher.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Fires RFIDDeciceChanged Event
        private void OnRfidDevicesChanged()
        {
            try
            {
                RfidDevicesChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}