namespace CodeLogger
{
    partial class InfoAuditer
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtAudit = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtAudit);
            this.splitContainer1.Size = new System.Drawing.Size(503, 264);
            this.splitContainer1.SplitterDistance = 167;
            this.splitContainer1.TabIndex = 2;
            // 
            // txtAudit
            // 
            this.txtAudit.BackColor = System.Drawing.Color.GhostWhite;
            this.txtAudit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAudit.Location = new System.Drawing.Point(0, 0);
            this.txtAudit.Margin = new System.Windows.Forms.Padding(0);
            this.txtAudit.Name = "txtAudit";
            this.txtAudit.Size = new System.Drawing.Size(332, 264);
            this.txtAudit.TabIndex = 0;
            this.txtAudit.Text = "";
            // 
            // InfoAuditer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "InfoAuditer";
            this.Size = new System.Drawing.Size(503, 264);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox txtAudit;
    }
}
