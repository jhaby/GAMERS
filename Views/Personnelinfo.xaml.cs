﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;
using System.ComponentModel;
using MaterialDesignThemes.Wpf;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GAMERS_TECH
{
    /// <summary>
    /// Interaction logic for Personnelinfo.xaml
    /// </summary>
    public partial class Personnelinfo : Page
    {
        private List<PersonnelData> UserList;
        PersonnelInfoViewModel persons;
        CreateUserData createUser;
        private static ConnService signalService;
        private TextBox txtBox;
        private static string msg;
        private Button sendBtn;

        public Personnelinfo(PersonnelInfoViewModel personnel, ConnService Sservice)

        {
            InitializeComponent();
            createUser = new CreateUserData();
            signalService = Sservice;

            UserList = new List<PersonnelData>();
            persons = personnel ;
            UserList = persons.GetData();
            Users.ItemsSource = UserList;
            createUser.PrevUid = "U"+(UserList.Count+1).ToString();
            this.DataContext = createUser;
            roleCombo.SelectionChanged += (s, e) =>
            {
                createUser.Role = roleCombo.Text;
            };

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Users.ItemsSource);
            view.Filter = UseFilter;

            signalService.SendingSuccess += (string response) =>
              {
                  MessageBox.Show(response);

              };

        }

        public static async Task SendSms(SMSDetails sms)
        {
            
            sms.Message = msg;
            await signalService.SendSMS(sms);
            
        }

        private bool UseFilter(object obj)
        {
            if (String.IsNullOrEmpty(SearchUser.Text))
                return true;
            else
                return ((obj as PersonnelData).Name.IndexOf(SearchUser.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void SearchUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(Users.ItemsSource).Refresh();
        }

        private async void Save_user_Click(object sender, RoutedEventArgs e)
        {
            progress.Visibility = Visibility.Visible;

            await Task.Run(() => SaveUser());
            progress.Visibility = Visibility.Collapsed;
        }
        public async Task SaveUser()
        {
            int result = await Helpers.InsertLoginInfo(createUser);
            Dispatcher.Invoke(() =>
            {
                if (result > 0)
                {
                    MessageBox.Show("User saved");
                    CreateUserData newuser = new CreateUserData();
                }
                else
                {
                    MessageBox.Show("Failed to connect to server");
                    progress.Visibility = Visibility.Collapsed;
                }
            });

        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            txtBox = sender as TextBox;
            msg = txtBox.Text;
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sendBtn = sender as Button;
            sendBtn.IsEnabled = false;

            Task.Run(async delegate
            {
                await Task.Delay(2000);
                Dispatcher.Invoke(() =>
                {
                    txtBox.Text = "";
                    sendBtn.IsEnabled = true;
                });
            });

        }
    }
   
}
