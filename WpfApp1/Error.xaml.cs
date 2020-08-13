using System;
using System.Windows;
using System.Windows.Media;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Error.xaml
    /// </summary>
    public partial class Error : Window
    {
        bool Off = true;

        //-----------------------Фоновое прозрачное окно, которое всегда отображается (нужно для нормального функционирования наказаний)
        Window1 window = new Window1();
        public void Exit(object sender, EventArgs e)
        {
            Off = false;
            window.Close();
            this.Close();
        }

        public Error()
        {
            InitializeComponent();
            window.Background = Brushes.Black;
            window.Show();
            Random R = new Random();
            int a = R.Next(0, 139);
            System.Windows.Controls.Button[] Btn = new System.Windows.Controls.Button[] { button, button_Copy1, button_Copy2,
                button_Copy3, button_Copy4, button_Copy5, button_Copy6, button_Copy7, button_Copy8, button_Copy9, button_Copy10,
                button_Copy11, button_Copy12, button_Copy13, button_Copy14, button_Copy15, button_Copy16, button_Copy17,
                button_Copy18, button_Copy19, button_Copy20, button_Copy21, button_Copy22, button_Copy23, button_Copy24,
                button_Copy25, button_Copy26, button_Copy27, button_Copy28, button_Copy29, button_Copy30,button_Copy31,
                button_Copy32, button_Copy33, button_Copy34,button_Copy35,button_Copy36,button_Copy37,button_Copy38,
                button_Copy39,button_Copy40,button_Copy41,button_Copy42,button_Copy43,button_Copy44,button_Copy45,
                button_Copy46,button_Copy47,button_Copy48,button_Copy49, button_Copy50, button_Copy51, button_Copy52, button_Copy53,
                button_Copy54, button_Copy55, button_Copy56,
                button_Copy57, button_Copy58, button_Copy59, button_Copy60, button_Copy61, button_Copy62, button_Copy63,
                button_Copy64, button_Copy54, button_Copy66, button_Copy67, button_Copy68, button_Copy69, button_Copy70,
                button_Copy71, button_Copy72, button_Copy73, button_Copy74, button_Copy75, button_Copy76,button_Copy77,
                button_Copy78, button_Copy79, button_Copy80,button_Copy81,button_Copy82,button_Copy83,button_Copy84,
                button_Copy85,button_Copy86,button_Copy87,button_Copy88,button_Copy89,button_Copy90,button_Copy91,button_Copy92,
                button_Copy93,button_Copy94,button_Copy95,button_Copy96,button_Copy97,button_Copy98,button_Copy99,button_Copy100,
                button_Copy101, button_Copy102,
                button_Copy103, button_Copy104, button_Copy105, button_Copy106, button_Copy107, button_Copy108, button_Copy109, button_Copy110,
                button_Copy111, button_Copy112, button_Copy113, button_Copy114, button_Copy115, button_Copy116, button_Copy117,
                button_Copy118, button_Copy119, button_Copy120, button_Copy121, button_Copy122, button_Copy123, button_Copy124,
                button_Copy125, button_Copy126, button_Copy127, button_Copy128, button_Copy129, button_Copy130,button_Copy131,
                button_Copy132, button_Copy133, button_Copy134,button_Copy135,button_Copy136,button_Copy137,button_Copy138,};
            Btn[a].Click += Exit;
            Btn[a].KeyUp += Exit;
            Btn[a].MouseLeftButtonUp += Exit;
            //Btn[a].Content = "111111111";
        }
        private void App_Activated(object sender, EventArgs e)
        {

        }

        private void App_Deactivated(object sender, EventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = Off;
        }
    }
}
