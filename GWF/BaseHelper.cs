using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWF
{
    public class BaseHelper
    {
        private bool result;
        private string returnCode;
        private string returnMsg;
        private string errorCode;
        private string errorMsg;

        public bool Result
        {
            get { return result; }
            set { result = value; }
        }
        public string ReturnCode
        {
            get { return returnCode; }
            set { returnCode = value; }
        }
        public string ReturnMsg
        {
            get { return returnMsg; }
            set { returnMsg = value; }
        }
        public string ErrorCode
        {
            get { return errorCode; }
            set { errorCode = value; }
        }
        public string ErrorMsg
        {
            get { return errorMsg; }
            set { errorMsg = value; }
        }


        public BaseHelper()
        {
            Reset();
        }

        internal void Reset()
        {
            Result = true;
            ReturnCode = "00";
            ReturnMsg = string.Empty;
            ErrorCode = "00";
            ErrorMsg = string.Empty;
        }
    }
}
