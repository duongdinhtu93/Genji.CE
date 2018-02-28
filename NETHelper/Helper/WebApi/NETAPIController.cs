using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace GenjiCore.Helper.WebApi
{
    public class NETAPIController: ApiController
    {
        public string UserID
        {
            get
            {
                return WebApiHeadersBase.UserID;
            }
        }

        public string LoginName
        {
            get
            {
                return WebApiHeadersBase.LoginName;
            }
        }

        public string LanguageCode
        {
            get
            {
                return WebApiHeadersBase.LanguageCode;
            }
        }
    }
}
