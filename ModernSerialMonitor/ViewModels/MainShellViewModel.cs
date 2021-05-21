using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Reactive.Bindings;

namespace ModernSerialMonitor.ViewModels
{
    class MainShellViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region プロパティ

        /// <summary>
        /// 最上面に固定するフラグの管理 (デフォルトはfalse)
        /// </summary>
        public ReactiveProperty<bool> IsTopmostProperty { get; set; } = new(false);
        #endregion


    }
}
