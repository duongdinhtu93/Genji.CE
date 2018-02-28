using System.Windows.Forms;

namespace GenjiCore.Components.ShutterNotifier.Appearance.Subcontrols
{
    partial class NotifierPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pbIcon = new System.Windows.Forms.PictureBox();
            this.lblText = new System.Windows.Forms.Label();
            this.lblTextAdditional = new System.Windows.Forms.Label();
            this.btnHide = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // pbIcon
            // 
            this.pbIcon.Location = new System.Drawing.Point(13, 16);
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.Size = new System.Drawing.Size(16, 16);
            this.pbIcon.TabIndex = 0;
            this.pbIcon.TabStop = false;
            this.pbIcon.Click += new System.EventHandler(this.CommonNotifyerPanelClick);
            // 
            // lblText
            // 
            this.lblText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(38, 18);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(0, 13);
            this.lblText.TabIndex = 1;
            this.lblText.Click += new System.EventHandler(this.CommonNotifyerPanelClick);
            // 
            // lblTextAdditional
            // 
            this.lblTextAdditional.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTextAdditional.AutoSize = true;
            this.lblTextAdditional.Location = new System.Drawing.Point(38, 34);
            this.lblTextAdditional.Name = "lblTextAdditional";
            this.lblTextAdditional.Size = new System.Drawing.Size(0, 13);
            this.lblTextAdditional.TabIndex = 2;
            this.lblTextAdditional.Click += new System.EventHandler(this.CommonNotifyerPanelClick);
            // 
            // btnHide
            // 
            this.btnHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHide.Location = new System.Drawing.Point(518, 75);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(82, 25);
            this.btnHide.TabIndex = 4;
            this.btnHide.TabStop = false;
            this.btnHide.Text = "Ok";
            this.btnHide.UseVisualStyleBackColor = true;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // NotifyerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.btnHide);
            this.Controls.Add(this.lblTextAdditional);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.pbIcon);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "NotifierPanel";
            this.Size = new System.Drawing.Size(616, 116);
            this.Click += new System.EventHandler(this.CommonNotifyerPanelClick);
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox pbIcon;
        private Label lblText;
        private Label lblTextAdditional;
        private Button btnHide;
    }
}
