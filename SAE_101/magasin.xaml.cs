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
using System.Windows.Shapes;

namespace SAE_101
{
    /// <summary>
    /// Logique d'interaction pour magasin.xaml
    /// </summary>

    public partial class magasin : Window
    {
        static readonly double PRIX_PIERRE = 0.5;
        static readonly double PRIX_BOIS = 0;
        static readonly double PRIX_METAL = 0;
        static readonly double PRIX_CIMENT = 0;
        static readonly double PRIX_FUTUR = 0;

        int quantite = 0;
        public double argent;
        public double pierre;
        double prixTotal = 0;

        public magasin()
        {
            InitializeComponent();
            box_qte.Text = quantite.ToString();
            quantite = 0;
            

        }

        private void but_vendre_Click(object sender, RoutedEventArgs e)
        {
            string materiaux = ((ComboBoxItem)this.liste_materiaux.SelectedItem).Content.ToString(); 
            Console.WriteLine(materiaux);
            if (pierre  - quantite < 0)
            {
                MessageBox.Show(this, "Vous n'avez pas assez de " + materiaux, "Quantité insufisante", MessageBoxButton.OK, MessageBoxImage.Warning);
                
            }
            else
            {
                switch (materiaux)
                {
                    case "pierre":
                        prixTotal += quantite * PRIX_PIERRE;
                        break;
                    case "bois":
                        prixTotal += quantite * PRIX_BOIS;
                        break;
                    case "metal":
                        prixTotal += quantite * PRIX_METAL;
                        break;
                    case "ciment":
                        prixTotal  += quantite * PRIX_CIMENT;
                        break;
                    case "futur":
                        prixTotal  += quantite * PRIX_FUTUR;
                        break;
                }
                pierre -= quantite;
                Console.WriteLine(pierre);
                Console.WriteLine(quantite);


                MessageBox.Show(this,"Argent gagné: " + prixTotal  + "€","Vendu !",MessageBoxButton.OK, MessageBoxImage.Information);
                argent = prixTotal;
                this.DialogResult = true;
            }
            


        }

        private void but_plus_Click(object sender, RoutedEventArgs e)
        {
            quantite =  int.Parse(box_qte.Text) + 1;
            box_qte.Text = quantite.ToString();
        }

        private void but_moins_Click(object sender, RoutedEventArgs e)
        {
            if (quantite > 0)
            {
                quantite = int.Parse(box_qte.Text) - 1;
                box_qte.Text = quantite.ToString();

            }
        }

        private void liste_materiaux_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void box_qte_LostFocus(object sender, RoutedEventArgs e)
        {
            quantite = int.Parse(box_qte.Text);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
