using ApplicationCore;
using ApplicationCore.Components;
using ApplicationCore.Helper;
using ApplicationCore.Helper.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Test.Properties;

namespace Test
{
    public partial class test : Form
    {
        public test()
        {
            InitializeComponent();
            Load += Test_Load;
        }

        private void Test_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            textBox2.RegisterSuggester();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CoreControllerCenter.NotifyController.ShowTaskbarPopup("Thông báo", "Bạn có tin nhắn từ ABC", OpenGoogle);

        }
        private void OpenGoogle()
        {
            Process.Start("http://google.com");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CoreControllerCenter.FileController.Upload(new ApplicationCore.Components.FileTransfer.FTPAuthentication() { Url = "ftp://speedtest.tele2.net/upload/" }, null);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.ShowWaiting(this, new Action[] { () => { try { Thread.Sleep(5000); } catch { } } }, "Đang xử lý");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var condinate = ApplicationCore.Components.GPS.GeographyLocation.GetCoordinates(txtAddress.Text);
            if (string.IsNullOrEmpty(condinate.Address))
            {
                MessageBox.Show("Không tìm thấy địa vị trí này");
                return;
            }

            MessageBox.Show(condinate.Address);

        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {
            var suggestwords = ApplicationCore.Helper.Utils.Suggester.GetSuggestion(txtAddress.Text);
            textBox1.Text = string.Empty;
            suggestwords.ToList().ForEach(d =>
            {
                textBox1.Text += d + "\n";
            });
            return;


            if (_tmrDelaySearch != null)
                _tmrDelaySearch.Stop();

            if (_tmrDelaySearch == null)
            {
                _tmrDelaySearch = new System.Windows.Forms.Timer();
                _tmrDelaySearch.Tick += _tmrDelaySearch_Tick;
                _tmrDelaySearch.Interval = DelayedTextChangedTimeout;
            }

            _tmrDelaySearch.Start();

        }

        private System.Windows.Forms.Timer _tmrDelaySearch;
        private const int DelayedTextChangedTimeout = 500;

        void _tmrDelaySearch_Tick(object sender, EventArgs e)
        {
            string word = string.IsNullOrEmpty(txtAddress.Text.Trim()) ? null : txtAddress.Text.Trim();
            if (word != string.Empty)
            {
                var condinate = ApplicationCore.Components.GPS.GeographyLocation.GetCoordinates(word);
                if (!string.IsNullOrEmpty(condinate.Address))
                    textBox1.Text = condinate.Address;
            }


            if (_tmrDelaySearch != null)
                _tmrDelaySearch.Stop();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SoundHelper.PlaySound(5, 1000, 50);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string keyword = txtAddress.Text;
            ApplicationCore.Helper.Utils.Suggester.GetSuggestion(keyword);
        }
    }
}
