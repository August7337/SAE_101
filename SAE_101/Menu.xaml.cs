using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SAE_101
{
    /// <summary>
    /// Logique d'interaction pour Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        double volume = 50;
        public Menu()
        {
            InitializeComponent();
        }



        private void but_jouer_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void but_quitter_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void barre_volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Console.WriteLine(volume);
            volume = barre_volume.Value;
            MainWindow.VolumeMusique(volume);

        }

        private void menu_accueil_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                but_jouer_Click(sender, e); // l'ppui sur la touche entrer équivaut à cliquer sur le bouton jouer 
            }
        }
    }
}