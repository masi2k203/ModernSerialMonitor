using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernSerialMonitor.Models
{
    interface ISerialPortRepository : IDisposable
    {
        public string[] GetPortName();
        public void SendTextData(string data);
        public void SendBinaryData(byte[] buffer, int offset, int count);
    }
}
