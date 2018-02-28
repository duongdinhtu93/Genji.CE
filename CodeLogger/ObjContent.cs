using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLogger
{
    public class ObjContent
    {
        public string ID { get; set; }
        [DisplayName("Phân hệ chức năng")]
        public string Module { get; set; }
        [DisplayName("Nội dung")]
        public string Content { get; set; }
    }
}
