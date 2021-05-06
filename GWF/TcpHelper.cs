using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace GWF
{
    public class TcpHelper
    {
        public static IChannel GetChannel(int tcpPort, bool isSecure)
        {
            BinaryServerFormatterSinkProvider serverProv = new BinaryServerFormatterSinkProvider();
            serverProv.TypeFilterLevel = TypeFilterLevel.Full;
            IDictionary propBag = new Hashtable();
            propBag["port"] = tcpPort;
            propBag["typeFilterLevel"] = TypeFilterLevel.Full;
            propBag["name"] = Guid.NewGuid().ToString();
            if (isSecure)
            {
                propBag["secure"] = isSecure;
                propBag["impersonate"] = false;
            }
            return new TcpChannel(propBag, null, serverProv);
        }
    }
}
