using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWF
{
    class BaseConstants
    {
        public static class ERROR_CODE
        {
            public const string DBCONNECTIONERROR = "DB-ER-01";            
        }

        public static class ERROR_MSG
        {
            public const string DBCONNECTIONERROR = "디비가 오픈되지 않았습니다.";
        }

    }
}
