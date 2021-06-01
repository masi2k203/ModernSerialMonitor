using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ModernSerialMonitor.Views
{
    // ページ管理
    public enum NavIcon
    {
        Monitor,
        AdvancedMonitor,
        AppInfo,
        Setting
    }

    /// <summary>
    /// MainShellView.xaml の相互作用ロジック
    /// </summary>
    public partial class MainShellView : Window
    {
        private static IReadOnlyDictionary<NavIcon, Type> _pages = new Dictionary<NavIcon, Type>()
        {
            {NavIcon.Monitor, typeof(ModernSerialMonitor.Views.Pages.MonitorPageView) },
            {NavIcon.AdvancedMonitor, typeof(ModernSerialMonitor.Views.Pages.AdvancedMonitorPageView) },
            {NavIcon.AppInfo, typeof(ModernSerialMonitor.Views.Pages.AppInfoPageView) },
            {NavIcon.Setting, typeof(ModernSerialMonitor.Views.Pages.SettingPageView) }
        };

        private static IReadOnlyDictionary<NavIcon, string> _headers = new Dictionary<NavIcon, string>()
        {
            {NavIcon.Monitor, "シリアルモニタ" },
            {NavIcon.AdvancedMonitor, "高機能シリアルモニタ" },
            {NavIcon.Setting, "設定" }
        };

        public MainShellView()
        {
            InitializeComponent();

            // シングルトン試験ビルド
            ModernWpf.MessageBox.Show("このビルドは「シングルトン試験ビルド」です。\n試験により一部の機能が欠落しています。");

        }

        private void NavView_SelectionChanged(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            try
            {
                // 選択されたアイテムを取得
                var selectedItem = (ModernWpf.Controls.NavigationViewItem)args.SelectedItem;
                // Tag取得
                var iconName = selectedItem.Tag?.ToString();

                if (Enum.TryParse(iconName, out NavIcon icon))
                {
                    // ナビゲーション
                    ContentFrame.Navigate(_pages[icon]);
                }
            }
            catch (Exception ex)
            {
                ModernWpf.MessageBox.Show("例外 : {0}", ex.Message);
            }
        }
    }
}
