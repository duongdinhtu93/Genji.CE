using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLogger
{
    public class ObjInfo
    {
        public ObjInfo()
        {
            Host = ApplicationManager.GetListHost();
        }
        [DisplayName("Bảng")]
        public string Tables { get; set; }
        [DisplayName("Bảng tạm")]
        public string TempTables { get; set; }
        [DisplayName("Cột")]
        public string Columns { get; set; }
        [DisplayName("Triggers")]
        public string Triggers { get; set; }
        [DisplayName("Sequence")]
        public string Sequences { get; set; }
        [DisplayName("Function")]
        public string Functions { get; set; }
        [DisplayName("View")]
        public string Views { get; set; }
        [DisplayName("Procedure")]
        public string Procedures { get; set; }
        [DisplayName("UI")]
        public string DUIs { get; set; }
        [DisplayName("H")]
        public List<ObjHost> Host { get; private set;}
    }
}
