namespace GenjiCore.Helper.Utils
{
    partial class frmSuggester
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
            this.listBoxRecommendation = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBoxRecommendation
            // 
            this.listBoxRecommendation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxRecommendation.FormattingEnabled = true;
            this.listBoxRecommendation.ItemHeight = 16;
            this.listBoxRecommendation.Items.AddRange(new object[] {
            "Iphone",
            "Iphone 4",
            "Iphone 5",
            "Iphone 6",
            "Iphone 7"});
            this.listBoxRecommendation.Location = new System.Drawing.Point(0, 0);
            this.listBoxRecommendation.Name = "listBoxRecommendation";
            this.listBoxRecommendation.Size = new System.Drawing.Size(266, 87);
            this.listBoxRecommendation.TabIndex = 0;
            // 
            // frmSuggester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 87);
            this.ControlBox = false;
            this.Controls.Add(this.listBoxRecommendation);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmSuggester";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmSuggester";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListBox listBoxRecommendation;
    }
}