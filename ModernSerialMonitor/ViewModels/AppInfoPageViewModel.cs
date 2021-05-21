using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Reactive.Bindings;
using System.Diagnostics;
using System.Reflection;
using System.IO;

namespace ModernSerialMonitor.ViewModels
{
    class AppInfoPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region プロパティ

        /// <summary>
        /// バージョン情報管理
        /// </summary>
        public ReactiveProperty<string> VersionInfoProperty { get; } = new();

        /// <summary>
        /// ライセンス表示管理
        /// </summary>
        public ReactiveProperty<string> LicenseTextProperty { get; } = new();
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AppInfoPageViewModel()
        {
            // 製品バージョン取得
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            VersionInfoProperty.Value = fileVersionInfo.ProductVersion;

            // ライセンス表示が含まれたテキストファイルを読み込む
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ModernSerialMonitor.Assets.UsedLicenses.txt";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream is not null)
                {
                    using (var sr = new StreamReader(stream))
                    {
                        LicenseTextProperty.Value = sr.ReadToEnd();
                    }
                }
            }
        }
    }
}
