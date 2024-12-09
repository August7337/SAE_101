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
        double argent = 0;
        double argentParClick = 1;
        double prixMairie = 10;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click_Mairie(object sender, RoutedEventArgs e)
        {
            argent += argentParClick;
            lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
        }

        private void button_Click_Achat_Mairie(object sender, RoutedEventArgs e)
        {
            if (argent >= prixMairie)
            {
                argent -= prixMairie;
                argentParClick++;
                prixMairie = prixMairie * 1.1;
                lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
                buttonAchatMairie.Content = "Ammelioration " + prixMairie.ToString("C", CultureInfo.CurrentCulture);
            }
        }

        private void button_Click_Achat_Mairie_Max(object sender, RoutedEventArgs e)
        {
            if (argent >= prixMairie)
            {
                double achatsMax = Math.Floor(argent / prixMairie);

                argentParClick += achatsMax;
                argent -= achatsMax * prixMairie;
                prixMairie = prixMairie * Math.Pow(1.1, achatsMax);
                lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
                buttonAchatMairie.Content = "Ammelioration " + prixMairie.ToString("C", CultureInfo.CurrentCulture);
            }
        }

        private void button_Click_Carriere(object sender, RoutedEventArgs e)
        {

        }

        private void button_Click_Achat_Carriere(object sender, RoutedEventArgs e)
        {

        }

        private void button_Click_Achat_Carriere_Max(object sender, RoutedEventArgs e)
        {

        }
    }
}