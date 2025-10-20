using System.Collections.ObjectModel;
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

namespace CS2TotalCasePriceCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SteamCaseValueCalculator _calculator = new SteamCaseValueCalculator();

        public ObservableCollection<CaseItem> CaseItems { get; set; }

        public MainWindow()
        {
            InitializeComponent();

     
            CaseItems = new ObservableCollection<CaseItem>
            {
             new CaseItem { CaseName = "Austin 2025 Contenders Autograph Capsule", Quantity = 1 },
             new CaseItem { CaseName = "Austin 2025 Contenders Sticker Capsule", Quantity = 7 },
             new CaseItem { CaseName = "Austin 2025 Legends Autograph Capsule", Quantity = 8 },
             new CaseItem { CaseName = "Dreams & Nightmares Case", Quantity = 15 },
             new CaseItem { CaseName = "Fever Case", Quantity = 1 },
             new CaseItem { CaseName = "Fracture Case", Quantity = 64 },
             new CaseItem { CaseName = "Killowatt Case", Quantity = 18 },
             new CaseItem { CaseName = "Recoil Case", Quantity = 16 },
             new CaseItem { CaseName = "Revolution Case", Quantity = 8 },
             new CaseItem { CaseName = "Revolver Case", Quantity = 1 },
             new CaseItem { CaseName = "Paris 2023 Legends Sticker Capsule", Quantity = 2 }

            };

            CaseGrid.ItemsSource = CaseItems;
        }

        private async void Calculate_Click(object sender, RoutedEventArgs e)
        {
            ProgressBar.Visibility = Visibility.Visible;
            ResultText.Text = "Fetching prices...";

            var dict = CaseItems
                .Where(c => !string.IsNullOrWhiteSpace(c.CaseName) && c.Quantity > 0)
                .ToDictionary(c => c.CaseName, c => c.Quantity);

            decimal total = await _calculator.CalculateTotalAsync(dict);

            ProgressBar.Visibility = Visibility.Collapsed;
            ResultText.Text = $"Your total case value: €{total:F2}";
        }
    }

    public class CaseItem
    {
        public string CaseName { get; set; }
        public int Quantity { get; set; }
    }
}
