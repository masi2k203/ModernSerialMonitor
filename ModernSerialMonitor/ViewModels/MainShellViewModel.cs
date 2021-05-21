using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Reactive.Bindings;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

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

        /// <summary>
        /// 製品プロパティ管理
        /// </summary>
        public ReactiveProperty<string> InformationlVersionProperty { get; } = new();
        #endregion

        #region コマンド

        /// <summary>
        /// [バージョン情報]ボタンコマンド
        /// </summary>
        public ReactiveCommand ShowVersionDialogCommand { get; } = new();

        /// <summary>
        /// [製作者のTwitter]ボタンコマンド
        /// </summary>
        public ReactiveCommand ShowProducerTwitterCommand { get; } = new();
        #endregion


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainShellViewModel()
        {
            // 製品バージョン取得
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            InformationlVersionProperty.Value = fileVersionInfo.ProductVersion;

            // コマンドの購読
            ShowVersionDialogCommand.Subscribe(_ => ShowVersionDialog());
            ShowProducerTwitterCommand.Subscribe(_ => ShowProducerTwitterPage());
        }


        #region メソッド
        /// <summary>
        /// バージョン情報Dialogを表示するメソッド
        /// </summary>
        private void ShowVersionDialog()
        {
            ModernWpf.MessageBox.Show($"バージョン：{InformationlVersionProperty.Value}", "バージョン情報");
        }

        /// <summary>
        /// 製作者のTwitterを開くメソッド
        /// </summary>
        private void ShowProducerTwitterPage()
        {
            string url = "https://twitter.com/klr8550_FB";
            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // Windowsのとき
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
            }
        }

        #endregion
    }
}
