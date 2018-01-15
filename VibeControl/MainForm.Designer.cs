namespace VibeControl
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SpeedControl = new System.Windows.Forms.TrackBar();
            this.ToyName = new System.Windows.Forms.Label();
            this.ScanBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SpeedControl)).BeginInit();
            this.SuspendLayout();
            // 
            // SpeedControl
            // 
            this.SpeedControl.LargeChange = 10;
            this.SpeedControl.Location = new System.Drawing.Point(25, 156);
            this.SpeedControl.Maximum = 100;
            this.SpeedControl.Name = "SpeedControl";
            this.SpeedControl.Size = new System.Drawing.Size(213, 45);
            this.SpeedControl.SmallChange = 5;
            this.SpeedControl.TabIndex = 0;
            this.SpeedControl.TickFrequency = 5;
            this.SpeedControl.Scroll += new System.EventHandler(this.SpeedControl_Scroll);
            // 
            // ToyName
            // 
            this.ToyName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ToyName.Location = new System.Drawing.Point(12, 9);
            this.ToyName.Name = "ToyName";
            this.ToyName.Size = new System.Drawing.Size(260, 144);
            this.ToyName.TabIndex = 1;
            this.ToyName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ScanBtn
            // 
            this.ScanBtn.Location = new System.Drawing.Point(93, 207);
            this.ScanBtn.Name = "ScanBtn";
            this.ScanBtn.Size = new System.Drawing.Size(75, 23);
            this.ScanBtn.TabIndex = 2;
            this.ScanBtn.Text = "Scan";
            this.ScanBtn.UseVisualStyleBackColor = true;
            this.ScanBtn.Click += new System.EventHandler(this.ScanBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.ScanBtn);
            this.Controls.Add(this.ToyName);
            this.Controls.Add(this.SpeedControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Vibe Control";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SpeedControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar SpeedControl;
        private System.Windows.Forms.Label ToyName;
        private System.Windows.Forms.Button ScanBtn;
    }
}

