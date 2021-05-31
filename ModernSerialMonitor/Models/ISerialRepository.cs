using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernSerialMonitor.Models
{
    interface ISerialRepository : IDisposable
    {
        public void ConnectSerialPort();
        public void DisconnectSerialPort();
        public void SerialDataReceived(object sender, SerialDataReceivedEventArgs e);
    }
}
