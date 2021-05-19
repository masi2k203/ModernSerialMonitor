using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Management;


namespace ModernSerialMonitor.Models
{
    public static class SerialManager
    {
        public class TerminatedCharacter
        {
            public string Description { get; set; }
            public string Terminated { get; set; }
        }

        /// <summary>
        /// 接続可能ポートリスト
        /// </summary>
        public static string[] PortList = SerialPort.GetPortNames();

        /// <summary>
        /// テンプレートボーレートリスト
        /// </summary>
        public static int[] BaudRateList = new int[]
        {
            110, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 57600, 115200
        };

        /// <summary>
        /// 終端文字テンプレートリスト
        /// </summary>
        public static List<TerminatedCharacter> TerminatedCharactersList = new()
        {
            new TerminatedCharacter { Description = "なし", Terminated = "" },
            new TerminatedCharacter { Description = "CRのみ", Terminated = "\r" },
            new TerminatedCharacter { Description = "LFのみ", Terminated = "\n" },
            new TerminatedCharacter { Description = "CRおよびLF", Terminated = "\r\n" }
        };
    }
}
