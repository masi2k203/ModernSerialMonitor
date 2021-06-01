using System;
using System.IO.Ports;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Reactive.Bindings;
using ModernSerialMonitor.Models;
using System.Collections.Generic;

namespace ModernSerialMonitor.ViewModels
{
    class MonitorPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

        /// <summary>
        /// シリアル通信用オブジェクト
        /// </summary>
        private MonoSerialObject SerialObject = MonoSerialObject.GetInstance;

        #endregion

        #region プロパティ

        /// <summary>
        /// COMポート名のリスト
        /// </summary>
        public ReactiveCollection<string> COMPortList { get; set; } = new();

        /// <summary>
        /// ボーレートのリスト
        /// </summary>
        public ReactiveCollection<int> BaudRateList { get; set; } = new();

        /// <summary>
        /// 終端文字のリスト
        /// </summary>
        public ReactiveCollection<SerialManager.TerminatedCharacter> TerminatedCharactersList { get; set; } = new();

        /// <summary>
        /// [接続]ボタン活性管理
        /// </summary>
        public ReactiveProperty<bool> IsConnectButtonActive { get; } = new(true);

        /// <summary>
        /// [切断]ボタン活性管理
        /// </summary>
        public ReactiveProperty<bool> IsDisconnectButtonActive { get; } = new(false);

        /// <summary>
        /// 選択されたポート名管理
        /// </summary>
        public ReactiveProperty<string> SelectedCOMPortName { get; set; } = new();

        /// <summary>
        /// 選択されたボーレート管理
        /// </summary>
        public ReactiveProperty<int> SelectedBaudRate { get; set; } = new();

        /// <summary>
        /// 選択された終端文字管理
        /// </summary>
        public ReactiveProperty<SerialManager.TerminatedCharacter> SelectedTerminatedCharacter { get; set; } 
            = new(SerialManager.TerminatedCharactersList[0]);

        /// <summary>
        /// 送信データ管理
        /// </summary>
        public ReactiveProperty<string> SendTextProperty { get; set; } = new();

        /// <summary>
        /// 受信データ管理
        /// </summary>
        public ReactiveProperty<string> ReceiveTextProperty { get; } = null;

        /// <summary>
        /// ステータスバー背景色管理
        /// </summary>
        public ReactiveProperty<SolidColorBrush> StatusBarBackgroundProperty { get; } = null;

        /// <summary>
        /// ステータスバーメッセージ管理
        /// </summary>
        public ReactiveProperty<string> StatusBarMessageProperty { get; } = null;
        #endregion

        #region コマンド

        /// <summary>
        /// [接続]ボタンコマンド
        /// </summary>
        public ReactiveCommand ConnectCommand { get; } = new();

        /// <summary>
        /// [送信]ボタンコマンド
        /// </summary>
        public ReactiveCommand SendCommand { get; set; } = new();

        /// <summary>
        /// [切断]ボタンコマンド
        /// </summary>
        public ReactiveCommand DisconnectCommand { get; } = new();

        /// <summary>
        /// [履歴をクリア]ボタンコマンド
        /// </summary>
        public ReactiveCommand DeleteResultCommand { get; } = new();

        /// <summary>
        /// [履歴をコピー]ボタンコマンド
        /// </summary>
        public ReactiveCommand CopyResultCommand { get; } = new();
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MonitorPageViewModel()
        {
            foreach (var item in SerialManager.PortList)
            {
                COMPortList.Add(item);
            }
            foreach (var item in SerialManager.BaudRateList)
            {
                BaudRateList.Add(item);
            }
            foreach (var item in SerialManager.TerminatedCharactersList)
            {
                TerminatedCharactersList.Add(item);
            }

            // コマンドの購読
            ConnectCommand.Subscribe(_ => ConnectSerialPort());
            SendCommand.Subscribe(_ => SendText());
            DisconnectCommand.Subscribe(_ => DisconnectSerialPort());
            DeleteResultCommand.Subscribe(_ => DeleteResult());
            CopyResultCommand.Subscribe(_ => CopyResult());

            // 通知可能プロパティの結びつけ
            ReceiveTextProperty = SerialObject.ReceivedDataProperty;
            StatusBarBackgroundProperty = SerialObject.StatusBarBackgroundProperty;
            StatusBarMessageProperty = SerialObject.StatusInfomationTextProperty;

            // 活性管理
            IsConnectButtonActive.Value = !SerialObject.IsConnected.Value;
            IsDisconnectButtonActive.Value = SerialObject.IsConnected.Value;
        }


        #region メソッド

        /// <summary>
        /// 接続処理
        /// </summary>
        private void ConnectSerialPort()
        {
            SerialObject.PortName = SelectedCOMPortName.Value;
            SerialObject.BaudRate = SelectedBaudRate.Value;
            SerialObject.ConnectSerialPort();
            IsConnectButtonActive.Value = !SerialObject.IsConnected.Value;
            IsDisconnectButtonActive.Value = SerialObject.IsConnected.Value;
        }

        /// <summary>
        /// 送信処理
        /// </summary>
        private void SendText()
        {
            string termStr = SelectedTerminatedCharacter.Value.Terminated;
            string sendText = SendTextProperty.Value + termStr;

            SerialObject.SendData = sendText;

            SerialObject.SendSerialData();
        }

        /// <summary>
        /// 切断処理
        /// </summary>
        private void DisconnectSerialPort()
        {
            SerialObject.DisconnectSerialPort();
            IsConnectButtonActive.Value = !SerialObject.IsConnected.Value;
            IsDisconnectButtonActive.Value = SerialObject.IsConnected.Value;
        }

        /// <summary>
        /// 受信履歴消去
        /// </summary>
        private void DeleteResult()
        {
            ReceiveTextProperty.Value = string.Empty;
        }

        /// <summary>
        /// 履歴をコピー
        /// </summary>
        private void CopyResult()
        {
            Clipboard.SetData(DataFormats.Text, (Object)ReceiveTextProperty.Value);
        }

        #endregion
    }
}
