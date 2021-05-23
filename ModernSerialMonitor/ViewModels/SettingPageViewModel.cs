using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;

namespace ModernSerialMonitor.ViewModels
{
    class SettingPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region プロパティ

        /// <summary>
        /// ライトテーマフラグ管理
        /// </summary>
        public ReactiveProperty<bool> IsLightThemeProperty { get; set; } = new(Properties.Settings.Default.IsLightTheme);

        /// <summary>
        /// ダークテーマフラグ管理
        /// </summary>
        public ReactiveProperty<bool> IsDarkThemeProperty { get; set; } = new(Properties.Settings.Default.IsDarkTheme);

        /// <summary>
        /// システム追従フラグフラグ管理
        /// </summary>
        public ReactiveProperty<bool> IsThemeAutoSetProperty { get; set; } = new(Properties.Settings.Default.IsThemeAutoSet);

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SettingPageViewModel()
        {
            // 値変更時の操作を購読
            IsLightThemeProperty.Subscribe(x => Properties.Settings.Default.IsLightTheme = x);
            IsDarkThemeProperty.Subscribe(x => Properties.Settings.Default.IsDarkTheme = x);
            IsThemeAutoSetProperty.Subscribe(x => Properties.Settings.Default.IsThemeAutoSet = x);

        }
    }
}
