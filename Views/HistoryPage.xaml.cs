using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for HistoryPage.xaml
    /// </summary>
    public partial class HistoryPage : Page
    {
        ConnService sService;
        private HistoryViewModel historyContext;
        public static event Action<HistoryModel> historyItem;

        public HistoryPage(ConnService signalr)
        {
            InitializeComponent();
            historyContext = new HistoryViewModel();
            this.DataContext = historyContext;
            sService = signalr;

            sService.HandleEventReceived += (Sender obj) =>
             {
                 historyContext.RefreshList();
             };
            

        }

       

        private async void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView view = sender as ListView;
            string value = view.SelectedValue.ToString().Split(": ")[1];
            string CaseID = view.Tag.ToString();

            if (value == "Reinstate case")
            {
               

                int index = historyContext.Items.FindIndex(ag => ag.CaseId.Equals(CaseID, StringComparison.Ordinal));
                if (historyContext.Items[index].Status != "Reinstated")
                {
                    var alert = new CasesModel()
                    {
                        CaseId = CaseID,
                        DateTime = DateTime.Now,
                        Location = historyContext.Items[index].Location,
                        VHTCode = historyContext.Items[index].VHTCode,
                        Description = historyContext.Items[index].Description,
                        Village = historyContext.Items[index].Village,
                        Status = "pending"
                    };
                    await Helpers.ReinstateCase(alert);
                    await sService.ReinstateCase(alert);
                    historyContext.RefreshList();
                }
                else
                {
                    MessageBox.Show("Failed! Already reinstated");
                }

            }
            else if(value == "Details")
            {
                int index = historyContext.Items.FindIndex(ag => ag.CaseId.Equals(CaseID, StringComparison.Ordinal));

                HistoryModel hItem = new HistoryModel();
                hItem = historyContext.Items[index];

                historyItem?.Invoke(hItem);


            }

        }
    }

    class HistoryViewModel : INotifyPropertyChanged
    {
        private List<HistoryModel> items;

        public List<HistoryModel> Items { get => items; set {  items = value; OnPropertyChanged("Items"); } }
        
        public HistoryViewModel()
        {
           Items = Helpers.LoadHistoryAsync("history").Result;
            
        }

        public void RefreshList()
        {
            Items = Helpers.LoadHistoryAsync("history").Result;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string member)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
        }

    }
    class HistoryList
    {
        private string code;
        private string description;
        private string agent;
        private string vHT;
        private string status;
        private string date;

        public string Date { get => date; set => date = value; }
        public string Code { get => code; set => code = value; }
        public string Description { get => description; set => description = value; }
        public string Agent { get => agent; set => agent = value; }
        public string VHT { get => vHT; set => vHT = value; }
        public string Status { get => status; set => status = value; }
        public List<string> Options { get; set; }
}
}
