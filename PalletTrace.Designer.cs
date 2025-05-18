namespace PalletTrace
{
    partial class PalletTrace
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PalletTrace));
            pcbLivePicture = new PictureBox();
            tmrFrameUpdate = new System.Windows.Forms.Timer(components);
            cboAvailableCameras = new ComboBox();
            pcbSettings = new PictureBox();
            cboFrameRate = new ComboBox();
            pnlSettings = new Panel();
            chkShowPalletDimentions = new CheckBox();
            lblShowPalletDimention = new Label();
            chkShowExcludedRegion = new CheckBox();
            lblShowExcludedRegion = new Label();
            lblRFIDSelect = new Label();
            lblFramerateSelect = new Label();
            lblSelectCamera = new Label();
            cboRfidComPort = new ComboBox();
            tmrResetPalletDetected = new System.Windows.Forms.Timer(components);
            lblInformation = new Label();
            tmrInformationVisible = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)pcbLivePicture).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pcbSettings).BeginInit();
            pnlSettings.SuspendLayout();
            SuspendLayout();
            // 
            // pcbLivePicture
            // 
            pcbLivePicture.Image = (Image)resources.GetObject("pcbLivePicture.Image");
            pcbLivePicture.InitialImage = null;
            pcbLivePicture.Location = new Point(12, 61);
            pcbLivePicture.Name = "pcbLivePicture";
            pcbLivePicture.Size = new Size(1611, 910);
            pcbLivePicture.SizeMode = PictureBoxSizeMode.StretchImage;
            pcbLivePicture.TabIndex = 0;
            pcbLivePicture.TabStop = false;
            pcbLivePicture.Paint += pcbLivePicture_Paint;
            pcbLivePicture.MouseDown += pcbLivePicture_MouseDown;
            pcbLivePicture.MouseMove += pcbLivePicture_MouseMove;
            pcbLivePicture.MouseUp += pcbLivePicture_MouseUp;
            // 
            // tmrFrameUpdate
            // 
            tmrFrameUpdate.Tick += FrameUpdate_Tick;
            // 
            // cboAvailableCameras
            // 
            cboAvailableCameras.Cursor = Cursors.Hand;
            cboAvailableCameras.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAvailableCameras.FormattingEnabled = true;
            cboAvailableCameras.Items.AddRange(new object[] { "<kameraliste>" });
            cboAvailableCameras.Location = new Point(44, 89);
            cboAvailableCameras.Name = "cboAvailableCameras";
            cboAvailableCameras.Size = new Size(162, 28);
            cboAvailableCameras.TabIndex = 1;
            // 
            // pcbSettings
            // 
            pcbSettings.Cursor = Cursors.Hand;
            pcbSettings.Image = (Image)resources.GetObject("pcbSettings.Image");
            pcbSettings.Location = new Point(1567, 6);
            pcbSettings.Name = "pcbSettings";
            pcbSettings.Size = new Size(50, 50);
            pcbSettings.SizeMode = PictureBoxSizeMode.StretchImage;
            pcbSettings.TabIndex = 2;
            pcbSettings.TabStop = false;
            pcbSettings.Click += pcbSettings_Click;
            pcbSettings.MouseEnter += pcbSettings_MouseEnter;
            pcbSettings.MouseLeave += pcbSettings_MouseLeave;
            // 
            // cboFrameRate
            // 
            cboFrameRate.Cursor = Cursors.Hand;
            cboFrameRate.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFrameRate.FormattingEnabled = true;
            cboFrameRate.Items.AddRange(new object[] { "2", "5", "10", "24", "30", "60" });
            cboFrameRate.Location = new Point(44, 176);
            cboFrameRate.Name = "cboFrameRate";
            cboFrameRate.Size = new Size(161, 28);
            cboFrameRate.TabIndex = 3;
            // 
            // pnlSettings
            // 
            pnlSettings.BackColor = Color.White;
            pnlSettings.BorderStyle = BorderStyle.FixedSingle;
            pnlSettings.Controls.Add(chkShowPalletDimentions);
            pnlSettings.Controls.Add(lblShowPalletDimention);
            pnlSettings.Controls.Add(chkShowExcludedRegion);
            pnlSettings.Controls.Add(lblShowExcludedRegion);
            pnlSettings.Controls.Add(lblRFIDSelect);
            pnlSettings.Controls.Add(lblFramerateSelect);
            pnlSettings.Controls.Add(lblSelectCamera);
            pnlSettings.Controls.Add(cboRfidComPort);
            pnlSettings.Controls.Add(cboFrameRate);
            pnlSettings.Controls.Add(cboAvailableCameras);
            pnlSettings.Location = new Point(1380, 1);
            pnlSettings.Name = "pnlSettings";
            pnlSettings.Size = new Size(243, 403);
            pnlSettings.TabIndex = 4;
            pnlSettings.Visible = false;
            // 
            // chkShowPalletDimentions
            // 
            chkShowPalletDimentions.AutoSize = true;
            chkShowPalletDimentions.Cursor = Cursors.Hand;
            chkShowPalletDimentions.Location = new Point(188, 354);
            chkShowPalletDimentions.Name = "chkShowPalletDimentions";
            chkShowPalletDimentions.Size = new Size(18, 17);
            chkShowPalletDimentions.TabIndex = 15;
            chkShowPalletDimentions.UseVisualStyleBackColor = true;
            chkShowPalletDimentions.CheckedChanged += chkShowPalletDimentions_CheckedChanged;
            // 
            // lblShowPalletDimention
            // 
            lblShowPalletDimention.AutoSize = true;
            lblShowPalletDimention.Location = new Point(27, 351);
            lblShowPalletDimention.Name = "lblShowPalletDimention";
            lblShowPalletDimention.Size = new Size(147, 20);
            lblShowPalletDimention.TabIndex = 14;
            lblShowPalletDimention.Text = "Vis palledimensjoner";
            // 
            // chkShowExcludedRegion
            // 
            chkShowExcludedRegion.AutoSize = true;
            chkShowExcludedRegion.Cursor = Cursors.Hand;
            chkShowExcludedRegion.Location = new Point(188, 334);
            chkShowExcludedRegion.Name = "chkShowExcludedRegion";
            chkShowExcludedRegion.Size = new Size(18, 17);
            chkShowExcludedRegion.TabIndex = 13;
            chkShowExcludedRegion.UseVisualStyleBackColor = true;
            chkShowExcludedRegion.CheckedChanged += chkShowExcludedRegion_CheckedChanged;
            // 
            // lblShowExcludedRegion
            // 
            lblShowExcludedRegion.AutoSize = true;
            lblShowExcludedRegion.Location = new Point(27, 331);
            lblShowExcludedRegion.Name = "lblShowExcludedRegion";
            lblShowExcludedRegion.Size = new Size(155, 20);
            lblShowExcludedRegion.TabIndex = 12;
            lblShowExcludedRegion.Text = "Vis ekskludert område";
            // 
            // lblRFIDSelect
            // 
            lblRFIDSelect.AutoSize = true;
            lblRFIDSelect.Location = new Point(44, 238);
            lblRFIDSelect.Name = "lblRFIDSelect";
            lblRFIDSelect.Size = new Size(77, 20);
            lblRFIDSelect.TabIndex = 10;
            lblRFIDSelect.Text = "RFID-leser";
            // 
            // lblFramerateSelect
            // 
            lblFramerateSelect.AutoSize = true;
            lblFramerateSelect.Location = new Point(44, 153);
            lblFramerateSelect.Name = "lblFramerateSelect";
            lblFramerateSelect.Size = new Size(157, 20);
            lblFramerateSelect.TabIndex = 9;
            lblFramerateSelect.Text = "Oppdateringsfrekvens:";
            // 
            // lblSelectCamera
            // 
            lblSelectCamera.AutoSize = true;
            lblSelectCamera.Location = new Point(44, 66);
            lblSelectCamera.Name = "lblSelectCamera";
            lblSelectCamera.Size = new Size(63, 20);
            lblSelectCamera.TabIndex = 8;
            lblSelectCamera.Text = "Kamera:";
            // 
            // cboRfidComPort
            // 
            cboRfidComPort.Cursor = Cursors.Hand;
            cboRfidComPort.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRfidComPort.FormattingEnabled = true;
            cboRfidComPort.Location = new Point(44, 261);
            cboRfidComPort.Name = "cboRfidComPort";
            cboRfidComPort.Size = new Size(161, 28);
            cboRfidComPort.TabIndex = 4;
            // 
            // tmrResetPalletDetected
            // 
            tmrResetPalletDetected.Interval = 3000;
            tmrResetPalletDetected.Tick += tmrResetPalletDetected_Tick;
            // 
            // lblInformation
            // 
            lblInformation.AutoSize = true;
            lblInformation.Font = new Font("Segoe UI", 18F);
            lblInformation.Location = new Point(12, 8);
            lblInformation.Name = "lblInformation";
            lblInformation.Size = new Size(346, 41);
            lblInformation.TabIndex = 6;
            lblInformation.Text = "Informasjon kommer her";
            lblInformation.Visible = false;
            // 
            // tmrInformationVisible
            // 
            tmrInformationVisible.Interval = 3000;
            tmrInformationVisible.Tick += tmrInformationVisible_Tick;
            // 
            // PalletTrace
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1635, 983);
            Controls.Add(lblInformation);
            Controls.Add(pcbSettings);
            Controls.Add(pnlSettings);
            Controls.Add(pcbLivePicture);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "PalletTrace";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Produktidentifikasjon";
            MouseUp += PalletTrace_MouseUp;
            ((System.ComponentModel.ISupportInitialize)pcbLivePicture).EndInit();
            ((System.ComponentModel.ISupportInitialize)pcbSettings).EndInit();
            pnlSettings.ResumeLayout(false);
            pnlSettings.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pcbLivePicture;
        private System.Windows.Forms.Timer tmrFrameUpdate;
        private ComboBox cboAvailableCameras;
        private PictureBox pcbSettings;
        private ComboBox cboFrameRate;
        private Panel pnlSettings;
        private ComboBox cboRfidComPort;
        private Label lblRFIDSelect;
        private Label lblFramerateSelect;
        private Label lblSelectCamera;
        private System.Windows.Forms.Timer tmrResetPalletDetected;
        private Label lblShowExcludedRegion;
        private CheckBox chkShowExcludedRegion;
        private CheckBox chkShowPalletDimentions;
        private Label lblShowPalletDimention;
        private Label lblInformation;
        private System.Windows.Forms.Timer tmrInformationVisible;
    }
}
