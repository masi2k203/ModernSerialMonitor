using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace ModernSerialMonitor.Models
{
    // TODO: シリアルポートを使用できるように実装する
    // TODO: MVVMである必要は無いので、とりあえず実装してみて
    class SerialPortRepository : ISerialPortRepository
    {
        #region プロパティ

        /// <summary>
        /// シリアルポート
        /// </summary>
        private SerialPort port = null;

        #endregion

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Dispose()
        {
            if (port == null)
            {
                return;
            }

            if (port.IsOpen == true)
            {
                port.Close();
                port.Dispose();
                port = null;
            }
        }

        /// <summary>
        /// ポート一覧を取得
        /// </summary>
        /// <returns>ポート一覧</returns>
        public string[] GetPortName()
        {
            return SerialPort.GetPortNames();
        }

        /// <summary>
        /// バイナリデータを送信する
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void SendBinaryData(byte[] buffer, int offset, int count)
        {
            if (PortAvailableCheck() == true)
            {
                port.Write(buffer, offset, count);
            }
        }

        /// <summary>
        /// テキストデータを送信する
        /// </summary>
        /// <param name="data"></param>
        public void SendTextData(string data)
        {
            if (PortAvailableCheck() == true)
            {
                port.Write(data);
            }
        }

        /// <summary>
        /// ポートが使用可能かを調べる
        /// </summary>
        /// <returns></returns>
        private bool PortAvailableCheck()
        {
            if (port == null)
            {
                return false;
            }
            if (port.IsOpen == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
