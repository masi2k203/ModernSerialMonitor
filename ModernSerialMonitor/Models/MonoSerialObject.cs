using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using Reactive.Bindings;
using System.ComponentModel;
using System.Windows.Media;

namespace ModernSerialMonitor.Models
{
    /// <summary>
    /// 単一SerialPortを扱うシングルトンオブジェクト
    /// </summary>
    class MonoSerialObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region シングルトン

        /// <summary>
        /// 自分自身
        /// </summary>
        private static MonoSerialObject _instance = new();

        /// <summary>
        /// インスタンスを取得する
        /// </summary>
        public static MonoSerialObject GetInstance
        {
            get { return _instance; }
        }

        #endregion

        #region フィールド

        /// <summary>
        /// シリアルポート
        /// </summary>
        private SerialPort _port = null;

        /// <summary>
        /// ステータスバー背景色辞書
        /// </summary>
        private Dictionary<string, SolidColorBrush> StatusBarColors = new()
        {
            { "Normal", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC")) },
            { "Connected", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CA5100")) }
        };

        #endregion

        #region シリアルポートに関係するプロパティ

        /// <summary>
        /// ポート名
        /// </summary>
        public string PortName { get; set; }

        /// <summary>
        /// ボーレート
        /// </summary>
        public int BaudRate { get; set; }

        /// <summary>
        /// 送信するデータ
        /// </summary>
        public string SendData { get; set; }

        #endregion

        #region 変更通知を行うプロパティ

        /// <summary>
        /// シリアルポート状態を示すプロパティ
        /// </summary>
        public ReactiveProperty<string> StatusInfomationTextProperty { get; } = new();

        /// <summary>
        /// ステータスバー背景色を保持するプロパティ
        /// </summary>
        public ReactiveProperty<SolidColorBrush> StatusBarBackgroundProperty { get; } = new();

        /// <summary>
        /// 受信した文字列を保持するプロパティ
        /// </summary>
        public ReactiveProperty<string> ReceivedDataProperty { get; } = new();

        /// <summary>
        /// 接続状態を保持するプロパティ
        /// </summary>
        public ReactiveProperty<bool> IsConnected { get; } = new(false);

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private MonoSerialObject()
        {
            // ポートをnullで初期化
            _port = null;

            // ステータスバー設定
            StatusInfomationTextProperty.Value = "準備完了";
            StatusBarBackgroundProperty.Value = StatusBarColors["Normal"];
        }

        #endregion

        #region 接続・切断処理

        /// <summary>
        /// シリアルポートに接続する処理
        /// </summary>
        public void ConnectSerialPort()
        {
            try
            {
                // ポートがnullであれば
                if (_port is null)
                {
                    // シリアルポート初期化
                    _port = new(PortName, BaudRate);

                    // イベント登録
                    _port.DataReceived += DataReceived;

                    // ポート解放
                    _port.Open();
                    IsConnected.Value = true;

                    // ステータスバー変更
                    StatusInfomationTextProperty.Value = "ポート：" + _port.PortName + "に接続中";
                    StatusBarBackgroundProperty.Value = StatusBarColors["Connected"];

                }
            }
            catch (Exception ex)
            {
                StatusInfomationTextProperty.Value = ex.Message;
                IsConnected.Value = false;
            }
        }

        /// <summary>
        /// シリアルポートから切断する処理
        /// </summary>
        public void DisconnectSerialPort()
        {
            try
            {
                if ((_port is not null) && _port.IsOpen)
                {
                    _port.Close();
                    _port.Dispose();
                    _port = null;

                    IsConnected.Value = false;
                    StatusBarBackgroundProperty.Value = StatusBarColors["Normal"];
                    StatusInfomationTextProperty.Value = "切断済み";

                }
            }
            catch (Exception ex)
            {
                StatusInfomationTextProperty.Value = ex.Message;
                StatusBarBackgroundProperty.Value = StatusBarColors["Normal"];
                IsConnected.Value = false;
            }
        }

        #endregion

        #region 送信・受信処理

        /// <summary>
        /// データを送信する処理
        /// </summary>
        public void SendSerialData()
        {
            try
            {
                if ((_port is not null) && _port.IsOpen)
                {
                    _port.Write(SendData);
                }
            }
            catch (Exception ex)
            {
                StatusInfomationTextProperty.Value = ex.Message;
            }
        }

        /// <summary>
        /// 受信時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (_port.IsOpen == false)
            {
                return;
            }

            try
            {
                ReceivedDataProperty.Value += _port.ReadExisting();
            }
            catch (Exception ex)
            {
                StatusInfomationTextProperty.Value += ex.Message;
            }
        }

        #endregion
    }
}
