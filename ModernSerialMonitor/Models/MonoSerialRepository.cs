using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Reactive.Bindings;

namespace ModernSerialMonitor.Models
{
    /// <summary>
    /// 1つのシリアルポートを管理するリポジトリ
    /// シングルトン
    /// </summary>
    class MonoSerialRepository : ISerialRepository
    {
        #region シングルトン

        /// <summary>
        /// 自分自身
        /// </summary>
        private static MonoSerialRepository _instance = new();

        /// <summary>
        /// インスタンスを取得する
        /// </summary>
        public static MonoSerialRepository GetInstance
        {
            get
            {
                return _instance;
            }
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



        #region ポート設定関係のプロパティ

        /// <summary>
        /// ポート名のプロパティ
        /// </summary>
        public string PortName { get; set; }

        /// <summary>
        /// ボーレートのプロパティ
        /// </summary>
        public int BaudRate { get; set; }

        #endregion


        #region 変更通知をするプロパティ

        /// <summary>
        /// 状態情報文字列プロパティ
        /// </summary>
        public ReactiveProperty<string> StatusInformationText { get; set; } = new();

        /// <summary>
        /// ステータスバー背景色管理
        /// </summary>
        public ReactiveProperty<SolidColorBrush> StatusBarBackgroundProperty { get; } = new();

        /// <summary>
        /// 受信データのプロパティ
        /// </summary>
        public ReactiveProperty<string> ReceivedData { get; set; } = new();

        /// <summary>
        /// 接続状態のプロパティ
        /// </summary>
        public ReactiveProperty<bool> IsConnected { get; } = new(false);

        #endregion


        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private MonoSerialRepository()
        {
            _port = null;
            
            // ステータスバー背景色設定
            StatusBarBackgroundProperty.Value = StatusBarColors["Normal"];
            StatusInformationText.Value = "準備完了";
        }

        #endregion


        /// <summary>
        /// シリアルポートに接続
        /// </summary>
        public void ConnectSerialPort()
        {
            try
            {
                if (_port is null)
                {
                    // シリアルポート初期化
                    _port = new(PortName, BaudRate);

                    // イベント登録
                    _port.DataReceived += SerialDataReceived;

                    // ポート解放
                    _port.Open();

                    // 接続状態
                    IsConnected.Value = true;
                    StatusBarBackgroundProperty.Value = StatusBarColors["Connected"];
                    StatusInformationText.Value = "ポート：" + _port.PortName + "に接続中";
                }
            }
            catch (Exception ex)
            {
                StatusInformationText.Value = ex.Message;
                IsConnected.Value = false;
            }
        }

        /// <summary>
        /// 切断処理
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
                    StatusInformationText.Value = "切断済み";

                }
            }
            catch (Exception ex)
            {
                StatusInformationText.Value = ex.Message;
                StatusBarBackgroundProperty.Value = StatusBarColors["Normal"];
                IsConnected.Value = false;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if ((_port is not null) && _port.IsOpen)
            {
                _port.Close();
                _port.Dispose();
                _port = null;
            }
        }


        /// <summary>
        /// 受信時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (_port.IsOpen == false)
            {
                return;
            }

            try
            {
                ReceivedData.Value += _port.ReadExisting();
            }
            catch (Exception ex)
            {
                StatusInformationText.Value += ex.Message;
            }
        }
    }
}
