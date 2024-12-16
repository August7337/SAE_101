using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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
    /// Logique d'interaction pour Defense.xaml
    /// </summary>
    public partial class Defense : Window
    {
        static readonly int PRIX_ANTI_TORNADE = 5000;
        static readonly int PRIX_ANTIDOTE = 5000;
        static readonly int PRIX_SCEAU_EAU = 5000;
        static readonly int PRIX_PARATONNERRE = 5000;
        
        public double argent;
        public string achat;
        public Defense()
        {
            InitializeComponent();
            
        }

        public void AfficheArgent()
        {
            Console.WriteLine("argent " + argent);
            lab_argent.Content = "Vous avez " + argent + " €";
            lab_prix_tornade.Content = "Prix: " + PRIX_ANTI_TORNADE + " €";
            lab_prix_antidote.Content = "Prix " + PRIX_ANTIDOTE + " €";
            lab_prix_eau.Content = "Prix " + PRIX_SCEAU_EAU + " €";
            lab_prix_paratonnerre.Content = "Prix " + PRIX_PARATONNERRE + " €";
        }

        private bool verifArgent(int prixObjet)
        {
            if (argent - prixObjet >= 0)
            {
                MessageBox.Show("Achat réussi", "Produit acheté", MessageBoxButton.OK, MessageBoxImage.Information);
                argent = argent - prixObjet;
                return true;
            }
            else
            {
                MessageBox.Show("Vous n'avez pas assez d'argent pour acheter cet objet", "Fonds insuffisants", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }

        private void but_antiTornade_Click(object sender, RoutedEventArgs e)
        {
            if (verifArgent(PRIX_ANTI_TORNADE))
            {
                achat = "antiTornade";
                this.DialogResult = true;
            }
            
        }

        private void but_antidote_Click(object sender, RoutedEventArgs e)
        {
            if (verifArgent(PRIX_ANTIDOTE))
            {
                achat = "antidote";
                this.DialogResult = true;
            }
        }

        private void but_eau_Click(object sender, RoutedEventArgs e)
        {
            if (verifArgent(PRIX_SCEAU_EAU))
            {
                achat = "sceauEau";
                this.DialogResult = true;
            }
        }

        private void but_paratonnere_Click(object sender, RoutedEventArgs e)
        {
            if (verifArgent(PRIX_PARATONNERRE))
            {
                achat = "paratonnerre";
                this.DialogResult = true;
            }
        }
    }
}
