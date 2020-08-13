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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GAMERS_TECH
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           
                
        }

        private void Minimise(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseWindow(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private async void Login(object sender, RoutedEventArgs e)
        {
            string user = Username.Text;
            string pswd = Password.Password;
            progress.Visibility = Visibility.Visible;

            await Task.Run(() => LoginTask(user, pswd));

            progress.Visibility = Visibility.Collapsed;
            

        }
        private async Task LoginTask(string user, string pass)
        {
            try
            {
                UserData Userdata = await Helpers.LoadLoginInfo(user, pass);
                Dispatcher.Invoke(() =>
                {

                    if (Userdata != null)
                    {
                        Home hm = new Home(Userdata);
                        hm.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Unauthorised user");
                    }
                });
            }
            catch (Exception e)
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(e.Message);
                });
            }

        }


    }
}
