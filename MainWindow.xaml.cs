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

        private void Login(object sender, RoutedEventArgs e)
        {
            DBConnection db = new DBConnection();
            string user = Username.Text;
            string pswd = Password.Password;
            UserData Userdata = db.Login(user, pswd);
            if(Userdata.Username != "none")
            {
                Home hm = new Home();
                hm.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Unauthorized user");
            }

            Application.Current.Properties["Userdata"] = Userdata;
        }


    }
}
