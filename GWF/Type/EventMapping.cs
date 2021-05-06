using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GWF.Type
{
    /// <summary>
    /// J2534/ecu 공통 : 전송결과 
    /// </summary>
    [DataContract]
    public class HTTPRESULT
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
