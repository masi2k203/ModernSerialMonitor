﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Reactive.Bindings;
using ModernSerialMonitor.Models;

namespace ModernSerialMonitor.ViewModels
{
    class MonitorPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region プロパティ

        /// <summary>
        /// COMポート名のリスト
        /// </summary>
        public ReactiveCollection<string> COMPortList { get; set; } = new();

        /// <summary>
        /// COMポートに接続されたデバイス名のリスト
        /// </summary>
        public ReactiveCollection<string> COMDeviceNameList { get; set; } = new();
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MonitorPageViewModel()
        {

        }
    }
}
