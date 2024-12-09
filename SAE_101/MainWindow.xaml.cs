using System.Globalization;
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

namespace SAE_101
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ulong coins = 0;
        ulong coinsParClick = 1;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            coins += coinsParClick;
            label.Content = coins.ToString("C", CultureInfo.CurrentCulture);
        }

        private void button_Click_Achat(object sender, RoutedEventArgs e)
        {
            if (coins >= 10)
            {
                coins -= 10;
                coinsParClick++;
                label.Content = coins.ToString("C", CultureInfo.CurrentCulture);
            }
        }

        private void button_Click_Achat_Max(object sender, RoutedEventArgs e)
        {
            if (coins >= 10)
            {
                coinsParClick += coins / 10;
                coins %= 10;

                label.Content = coins.ToString("C", CultureInfo.CurrentCulture);

            }
        }
    }
}