using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    /// 
    
    public partial class Window2 : Window
    {
        //-----------------------Фоновое прозрачное окно, (нужно для нормального функционирования наказаний)
        Window1 window = new Window1();
       
        bool Off = true;

        public  Window2()
        {
         
            InitializeComponent();

            window.Background = Brushes.Black;
            window.Show();
            Random R = new Random();
            int r = R.Next(1, 9);
            string a = Convert.ToString(r);
            image.Source = new BitmapImage(new Uri("pack://application:,,,/" + a + ".jpg"));

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Off = false;
            window.Close();
            this.Close();
        }

        private void Form_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = Off;
        }
    }
}
