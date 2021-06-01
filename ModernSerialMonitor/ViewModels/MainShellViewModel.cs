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
using ModernWpf;

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

        /// <summary>
        /// アプリケーションテーマ情報管理
        /// </summary>
        public ReactiveProperty<ElementTheme> AppThemeProperty { get; } = new();

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

            // 設定値変更のイベントを購読
            Properties.Settings.Default.PropertyChanged += Default_PropertyChanged;

            // テーマを適用
            AppThemeProperty.Value = GetAppTheme();

            
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

        /// <summary>
        /// 現在のアプリテーマを取得する
        /// </summary>
        /// <returns></returns>
        private ElementTheme GetAppTheme()
        {
            if (Properties.Settings.Default.IsThemeAutoSet == true)
            {
                return ElementTheme.Default;
            }
            if (Properties.Settings.Default.IsLightTheme == true)
            {
                return ElementTheme.Light;
            }
            if (Properties.Settings.Default.IsDarkTheme == true)
            {
                return ElementTheme.Dark;
            }
            return ElementTheme.Default;
        }

        /// <summary>
        /// Property.Setting.Default以下が変更された際の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            AppThemeProperty.Value = GetAppTheme();
            Properties.Settings.Default.Save();
        }
        #endregion
    }
}
