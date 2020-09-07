using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
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
        bool block = true; // переменная для предотвращения многоповторности

        DispatcherTimer timer1 = new DispatcherTimer();// таймер для отсчета времени последнего обновления
        public MainWindow()
        {
            #region Добавление приложения в автозагрузку

            Microsoft.Win32.RegistryKey myKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true); // записываем данный подраздел в переменную mykey
                                                                                                                                                            //myKey.DeleteValue("App");
            if (myKey.GetValue("App") == null)// проверяем на отсутствие имени приложения в данном разделе
            {
                myKey.SetValue("App", System.Windows.Forms.Application.ExecutablePath);// записываем имя приложения и указываем путь к нему
                myKey.Close();
            }

            #endregion

            InitializeComponent();

            #region Перемещаем форму в правый нижний угл

            var primaryMonitorArea = SystemParameters.WorkArea;// получаем размер экрана
            Left = primaryMonitorArea.Right - Width - 10;// задаем переменнной Left координату х 
            Top = primaryMonitorArea.Bottom - Height - 10;// задаем переменнной Left координату у
            #endregion

            #region Настраиваем таймер для отсчета времени последнего обновления
            timer1.Tick += new EventHandler(Timer1_Tick);
            timer1.Interval = new TimeSpan(0, 0, 1);
            timer1.Start();
            #endregion

        }
        static DateTime time_update()// метод получениея даты последнего изменения папки в формате DateTime
        {
            //-----------------------Определяем папку в которую закидываются базы и узнаем дату ее последнего изменения----------
            // string dirName = "C:\\ProgramData\\Kaspersky Lab\\KES\\Bases";
            string dirName = @"C:\ProgramData\Kaspersky Lab\AVP21.1\Bases"; //путь папки у которой считывают дату изменения, можно указать любую
            DirectoryInfo dirInfo = new DirectoryInfo(dirName);
            dirInfo.LastWriteTime.ToShortDateString(); // получаем значение даты последнего изменения папки
            DateTime date_update = dirInfo.LastWriteTime;
            return date_update;
        }
        //------------------- Отсчет 14 дней и вывод остатка на экран------------------------------------------------------------  
        private void Timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan timeSpan = DateTime.Now - DateTime.MinValue; // получаем сегодняшнию даты в формате TimeSpan
            DateTime date_update = new DateTime();
            try { date_update = time_update().AddDays(14) - timeSpan; }
            catch { }
            //date_update = time_update().AddDays(13) - timeSpan;// получаем дату 14 дней 00:00:00 в формате DateTime

            Timer2();

            if (DateTime.Now.Minute > 2) { block = true; }// переменная block позволяет запускать окна всего один раз 

            if (date_update.Day - 1 == 0 && date_update.Hour == 0 && date_update.Minute == 0 && date_update.Second == 0)  
            {
                label3.Content = "      Опоздали!!!";
                if (DateTime.Now.Minute == 0 && block == true)
                {
                    block = false;
                    nakazanie();
                }
            }
            else if (date_update.Day - 1 >= 0)
            {
                label3.Content = date_update.Day-1 + " дней " + date_update.ToString("HH:mm:ss");
                if (date_update.Day - 1 <= 7 && date_update.Day - 1 > 3 && DateTime.Now.Hour % 2 == 0 && DateTime.Now.Minute == 0 && block == true)
                {
                    block = false;
                    Window2 Warring = new Window2();
                    Warring.ShowDialog();

                }
                else if (date_update.Day - 1 <= 3 && DateTime.Now.Minute == 0 && block == true)
                {
                    block = false;
                    nakazanie2();

                }
            }
            #region скрипт который контролирует запуск программы
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
            #endregion

            var a = Process.GetProcessesByName("wscript");// проверка запуска скрипта
            if (a.Length == 0)
            {
                Process.Start(System.Windows.Forms.Application.StartupPath + "\\script.vbs");
            }

        }
        private void Timer2()
        {
            TimeSpan timeSpan = time_update().AddDays(14) - DateTime.Now;
            int now = timeSpan.Days * 24 * 60 + timeSpan.Hours * 60 + timeSpan.Minutes;
            int max = 14 * 24 * 60;
            progressBar.Maximum = max;
            progressBar.Value = max - now;
        }// Заполнение строки прогресса
        #region код чтобы окно не отображалось в Alt+Tab

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
        #endregion

        #region Код чтобы окно ,было позади всех окон

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
        #endregion

        public static void nakazanie()
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

        #region код для перемещения курсора мыши
        private static void MoveMouse(int screenWidth, int screenHeight)
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
        #endregion
    }

}
