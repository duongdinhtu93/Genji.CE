using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLogger
{
    public class ObjConfig
    {
        public ObjConfig()
        {

        }
        public Guid ID { get; set; }
        [DisplayName("Số thứ tự")]
        public int OrderIndex { get; set; }
        [DisplayName("Phân hệ chức năng")]
        public string Module { get; set; }
        [DisplayName("Class path")]
        public string ClassPath { get; set; }
        [DisplayName("DLL path")]
        public string DllPath { get; set; }
        [DisplayName("Quyền")]
        public string Permission { get; set; }
        [DisplayName("Cấu hình ứng dụng")]
        public string AppConfig { get; set; }
        [DisplayName("Tham số menu")]
        public string MenuArgs { get; set; }
        [DisplayName("Báo cáo")]
        public string Report { get; set; }
        [DisplayName("Cấu hình tệp tin")]
        public string File { get; set; }
    }
}
