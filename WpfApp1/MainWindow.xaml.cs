using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;


namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool xui=true; // переменная для предотвращения многоповторности
       

        //-----------------------Настраиваем таймер для отсчета времени последнего обновления-----------------
        DispatcherTimer timer1 = new DispatcherTimer();
        //----------------------- Настаиваем таймер для выполнения строки прогресса----------------------------
        DispatcherTimer timer2 = new DispatcherTimer();
        public MainWindow()
        {
            //--------------------------------Добавление приложения в автозагрузку--------------------------------------
            try
            {
                Microsoft.Win32.RegistryKey myKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                //myKey.DeleteValue("App");
                if (myKey.GetValue("App") == null)
                {
                    myKey.SetValue("App",@""""+System.Windows.Forms.Application.ExecutablePath+@"""");
                    myKey.Close();
                }
            }
            catch
            {

            }
            //----------------------------------------------------------------------------------------------------------


            InitializeComponent();
       

            //------------------------Перемещаем форму в правый нижний угл-----------------------------------------------
            var primaryMonitorArea = SystemParameters.WorkArea;
            Left = primaryMonitorArea.Right - Width - 10;
            Top = primaryMonitorArea.Bottom - Height - 10;

            //------------------------- Настраиваем таймер для отсчета времени последнего обновления---------------------
            timer1.Tick += new EventHandler(Timer1_Tick);
            timer1.Interval = new TimeSpan(0, 0, 1);
            timer1.Start();

            //----------------------------Настаиваем таймер для выполнения строки прогресса------------------------------- 
            timer2.Tick += new EventHandler(Timer2_Tick);
            timer2.Interval = new TimeSpan(0, 0, 1);
            timer2.Start();

        }
        static DateTime time_update()
        {
            //-----------------------Определяем папку в которую закидываются базы и узнаем дату ее последнего изменения----------
           // string dirName = "C:\\ProgramData\\Kaspersky Lab\\KES\\Bases";
            string dirName = "C:\\ProgramData\\Kaspersky Lab\\AVP20.0\\Bases"; //путь папки у которой считывают дату изменения, можно указать любую
            DirectoryInfo dirInfo = new DirectoryInfo(dirName);
            dirInfo.LastWriteTime.ToShortDateString();
            //-----------------------Распеделяем полученные данные по группам---------------------------------------------
            int year = Convert.ToInt32(dirInfo.LastWriteTime.Year);
            int month = Convert.ToInt32(dirInfo.LastWriteTime.Month);
            int day = Convert.ToInt32(dirInfo.LastWriteTime.Day);
            int hout = Convert.ToInt32(dirInfo.LastWriteTime.Hour);
            int minutes = Convert.ToInt32(dirInfo.LastWriteTime.Minute);
            int seconds = Convert.ToInt32(dirInfo.LastWriteTime.Second);
            DateTime date_update = new DateTime(year, month, day, hout, minutes, seconds);

            //-------------------- Возвращаем полученные данные----------------------------------------------------
            return date_update;
        }
        //------------------- Отсчет 14 дней и вывод остатка на экран------------------------------------------------------------  
        private void Timer1_Tick(object sender, EventArgs e)
        {
            DateTime date_update = new DateTime();
            date_update = time_update();
            TimeSpan timeSpan = date_update.AddDays(14) - DateTime.Now;
            string h = Convert.ToString(timeSpan.Hours);
            string m = Convert.ToString(timeSpan.Minutes);
            string s = Convert.ToString(timeSpan.Seconds);


            if (DateTime.Now.Minute > 2) { xui = true; }
                

            if (timeSpan.Hours < 10) 
            { h = "0" + Convert.ToString(timeSpan.Hours); }
            if (timeSpan.Minutes < 10)
            { m = "0" + Convert.ToString(timeSpan.Minutes); }
            if (timeSpan.Seconds < 10)
            { s = "0" + Convert.ToString(timeSpan.Seconds); }

            if (timeSpan.Days <= 0 && timeSpan.Ticks < 0)
            {
                label3.Content = "      Опоздали!!!";
                if (DateTime.Now.Minute == 0 && xui == true)
                {
                    xui = false;
                    nakazanie();
                }
            }
            else if (timeSpan.Days >= 0 && timeSpan.Ticks > 0)
            {
                label3.Content = timeSpan.Days + " дней " + h + ":" + m + ":" + s;
                if (timeSpan.Days <= 7 && timeSpan.Days > 3 && DateTime.Now.Hour % 2 == 0 && DateTime.Now.Minute == 0 && xui == true)
                {
                    xui = false;
                    //window.Background = Brushes.Black;
                    Window2 Warring = new Window2();
                    Warring.ShowDialog(); 
                    //window.Background = Brushes.Transparent;
                    
                }
                else if (timeSpan.Days <= 3 && DateTime.Now.Minute == 0 && xui == true)
                {
                    xui = false;
                    nakazanie2();
                    
                }
            }
            try
            {
                if (!System.IO.File.Exists(System.Windows.Forms.Application.StartupPath + "\\script.vbs")) // созание скрипта который проверяет на запуск исполняемый файл
                {
                    string d = @"Set objWMIService = GetObject(""winmgmts:\\.\root\cimv2"") 
 Do
 Running = False
 Set colItems = objWMIService.ExecQuery(""Select * from Win32_Process"")
 For Each objItem in colItems
     If objItem.Name = ""Windows.exe"" Then
         Running = True
         Exit For
     End If
 Next
 If Not Running Then
     CreateObject(""WScript.Shell"").Run """ + System.Windows.Forms.Application.ExecutablePath + @""", 1, True 
 End If 
 Loop";
                    File.WriteAllText(System.Windows.Forms.Application.StartupPath + "\\script.vbs", d);
                }
            }
            catch { }
            //---------------------------- проверка запуска скрипта 1.vbs -------------------------------------------------------
            var a = Process.GetProcessesByName("wscript");
            if (a.Length == 0)
            {
                Process.Start(System.Windows.Forms.Application.StartupPath + "\\script.vbs");
            }

        }

        //---------------------------- Заполнение строки прогресса-------------------------------------------------------
        private void Timer2_Tick(object sender, EventArgs e)
        {
            DateTime date_update = new DateTime();
            date_update = time_update();
            TimeSpan timeSpan = date_update.AddDays(14) - DateTime.Now;
            int now = timeSpan.Days * 24 * 60 + timeSpan.Hours * 60 + timeSpan.Minutes;
            int max = 14 * 24 * 60;
            progressBar.Maximum = max;
            progressBar.Value = max - now;
        }
        //----------------------код чтобы окно не отображалось в Alt+Tab----------------------------------------------------
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
        private void Loader(object sender, RoutedEventArgs e)
        {
            HideFromAltTab(Handle);
        }
        //----------------------------------------------------------------------------------------------------------------------

        //----------------------Код чтобы окно ,было позади всех окон----------------------------------------------------
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(int hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        public const int HWND_BOTTOM = 0x1;
        public const uint SWP_NOSIZE = 0x1;
        public const uint SWP_NOMOVE = 0x2;
        public const uint SWP_SHOWWINDOW = 0x40;
        private void ShoveToBackground()
        {
            SetWindowPos((int)this.Handle, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOMOVE |
                SWP_NOSIZE | SWP_SHOWWINDOW);
        }
        private void Activ(object sender, EventArgs e)
        {
            ShoveToBackground();
        }
        //-----------------------------------------------------------------------------------------------------------------------


        public void nakazanie()
        {
            System.Drawing.Size resolution = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
            MoveMouse(resolution.Width, resolution.Height);
        }

        public void nakazanie2()
        {
           // window.Background = Brushes.Black;
            Error error = new Error();
            error.ShowDialog();
           // window.Background = Brushes.Transparent;
        }
        //------------------------------код для перемещения курсора мыши--------------------------------------------------------------
        private void MoveMouse(int screenWidth, int screenHeight)
        {
            POINT p = new POINT();
            Random r = new Random();
            int c = 0;
            while (true)
            {
                p.x = Convert.ToInt32(r.Next(screenWidth));
                p.y = Convert.ToInt32(r.Next(screenHeight));
                //  ClientToScreen(Handle, ref p);
                SetCursorPos(p.x, p.y);
                c++;
                Thread.Sleep(500);
                if (c == 1200)
                {
                    break;
                }
            }
        }

        [DllImport("user32.dll")]
        public static extern long SetCursorPos(int x, int y);
        public struct POINT
        {
            public int x;
            public int y;
        }
    }//---------------------------------------------------------------------------------------------------------

}
