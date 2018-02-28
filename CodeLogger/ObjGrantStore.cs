using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLogger
{
    public class ObjGrantStore
    {
        public string ID { get; set; }
        [DisplayName("Tên store procedure")]
        public string StoreName { get; set; }
        [DisplayName("Lệnh")]
        public string GrantCommand { get; set; }
    }
}
