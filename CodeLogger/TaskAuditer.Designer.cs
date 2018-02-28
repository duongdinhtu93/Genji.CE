namespace CodeLogger
{
    partial class TaskAuditer
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageInfo = new System.Windows.Forms.TabPage();
            this.tabPageConfig = new System.Windows.Forms.TabPage();
            this.tabPageContent = new System.Windows.Forms.TabPage();
            this.tabPageGrant = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPageInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPageInfo);
            this.tabControl1.Controls.Add(this.tabPageConfig);
            this.tabControl1.Controls.Add(this.tabPageContent);
            this.tabControl1.Controls.Add(this.tabPageGrant);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(531, 306);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPageInfo
            // 
            this.tabPageInfo.Controls.Add(this.infoAuditer1);
            this.tabPageInfo.Location = new System.Drawing.Point(4, 25);
            this.tabPageInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageInfo.Name = "tabPageInfo";
            this.tabPageInfo.Size = new System.Drawing.Size(523, 277);
            this.tabPageInfo.TabIndex = 0;
            this.tabPageInfo.Text = "Info";
            this.tabPageInfo.UseVisualStyleBackColor = true;
            // 
            // tabPageConfig
            // 
            this.tabPageConfig.Location = new System.Drawing.Point(4, 25);
            this.tabPageConfig.Name = "tabPageConfig";
            this.tabPageConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConfig.Size = new System.Drawing.Size(523, 277);
            this.tabPageConfig.TabIndex = 1;
            this.tabPageConfig.Text = "Config";
            this.tabPageConfig.UseVisualStyleBackColor = true;
            // 
            // tabPageContent
            // 
            this.tabPageContent.Location = new System.Drawing.Point(4, 25);
            this.tabPageContent.Name = "tabPageContent";
            this.tabPageContent.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageContent.Size = new System.Drawing.Size(523, 277);
            this.tabPageContent.TabIndex = 2;
            this.tabPageContent.Text = "Content";
            this.tabPageContent.UseVisualStyleBackColor = true;
            // 
            // tabPageGrant
            // 
            this.tabPageGrant.Location = new System.Drawing.Point(4, 25);
            this.tabPageGrant.Name = "tabPageGrant";
            this.tabPageGrant.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGrant.Size = new System.Drawing.Size(523, 277);
            this.tabPageGrant.TabIndex = 3;
            this.tabPageGrant.Text = "Grant";
            this.tabPageGrant.UseVisualStyleBackColor = true;
            // 
            // infoAuditer1
            // 
            this.infoAuditer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoAuditer1.Location = new System.Drawing.Point(0, 0);
            this.infoAuditer1.Name = "infoAuditer1";
            this.infoAuditer1.Size = new System.Drawing.Size(523, 277);
            this.infoAuditer1.TabIndex = 0;
            // 
            // TaskAuditer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "TaskAuditer";
            this.Size = new System.Drawing.Size(531, 306);
            this.tabControl1.ResumeLayout(false);
            this.tabPageInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageInfo;
        private System.Windows.Forms.TabPage tabPageConfig;
        private System.Windows.Forms.TabPage tabPageContent;
        private System.Windows.Forms.TabPage tabPageGrant;
        private InfoAuditer infoAuditer1;
    }
}
