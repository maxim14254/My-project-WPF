using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

        }

        //----------------------код чтобы окно  ,было позади всех окон----------------------------------------------------
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr window, int index, int value);
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr window, int index);
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        public static void HideFromAltTab(IntPtr Handle)
        {
            SetWindowLong(Handle,
                          GWL_EXSTYLE,
                          GetWindowLong(Handle, GWL_EXSTYLE) | WS_EX_TOOLWINDOW);
        }
        private IntPtr Handle
        {
            get
            {
                return new WindowInteropHelper(this).Handle;
            }
        }
        //Необходимо создать событие типа Loaded
        private void Loader(object sender, RoutedEventArgs e)
        {
            HideFromAltTab(this.Handle);
        }
        //------------------------------------------------------------------------------------------------
    }
}
