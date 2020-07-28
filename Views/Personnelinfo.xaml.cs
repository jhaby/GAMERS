using System;
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


namespace GAMERS_TECH
{
    /// <summary>
    /// Interaction logic for Personnelinfo.xaml
    /// </summary>
    public partial class Personnelinfo : Page
    {
        private List<PersonnelData> UserList;

        public Personnelinfo()

        {
            InitializeComponent();
            UserList = new List<PersonnelData>();
            PersonnelInfoViewModel persons = new PersonnelInfoViewModel();
            UserList = persons.GetData();
            Users.ItemsSource = UserList;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Users.ItemsSource);
            view.Filter = UseFilter;
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
    }
}
