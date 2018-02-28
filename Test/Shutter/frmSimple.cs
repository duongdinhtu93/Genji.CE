using GenjiCore.Components.ShutterNotifier;
using GenjiCore.Components.ShutterNotifier.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test.Shutter
{
    public partial class frmSimple : Form
    {
        public frmSimple()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            var shutter = new ShutterNotifier()
            {
                ParentControl = this,
                Animation = GenjiCore.Components.ShutterNotifier.Appearance.Constants.Animations.DecelerationBottom,
                Icon = GenjiCore.Components.ShutterNotifier.Appearance.Constants.Icons.Warning,
                ConcealmentMethod = GenjiCore.Components.ShutterNotifier.Appearance.Constants.ClickableControls.Area,
                Theme = GenjiCore.Components.ShutterNotifier.Appearance.Constants.Themes.Blue
            };
            shutter.In(new NotifierMessage("Vui lòng nhập các field bắt buộc", "Chi tiết",
                  new List<WarningItem>() {
                    new WarningItem("Email không được bỏ trống", txtEmail),
                    new WarningItem("Username không được bỏ trống", txtUsername),
                    new WarningItem("Mật khẩu không được bỏ trống", txtPassword)
                  }));
        }

        private void button1_Click(object sender, EventArgs e)
        {
           var shutter = new ShutterNotifier()
            {
                ParentControl = txtTextArea,
                Animation = GenjiCore.Components.ShutterNotifier.Appearance.Constants.Animations.FadeIn,
                Icon = GenjiCore.Components.ShutterNotifier.Appearance.Constants.Icons.Error,
                ConcealmentMethod = GenjiCore.Components.ShutterNotifier.Appearance.Constants.ClickableControls.Area,
                Theme = GenjiCore.Components.ShutterNotifier.Appearance.Constants.Themes.White
            };
            shutter.In("Field này không được bỏ trống");
        }
    }
}
