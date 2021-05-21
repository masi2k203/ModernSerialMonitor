using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace ModernSerialMonitor.Views.Behaviors
{
    class WindowCloseBehavior : Behavior<MenuItem>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Click += ButtonClick;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.Click -= ButtonClick;
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            // senderをMenuitemとしてキャストする
            var menuitem = sender as MenuItem;
            // キャスト出来なければ処理中断
            if (menuitem is null)
            {
                return;
            }

            // 親のウィンドウを取得する
            var window = Window.GetWindow(menuitem);

            // ウィンドウを閉じる
            window.Close();
        }
    }
}
