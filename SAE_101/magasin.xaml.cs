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
        static readonly double PRIX_BOIS = 1;
        static readonly double PRIX_METAL = 2;
        static readonly double PRIX_CIMENT = 5;
        static readonly double PRIX_FUTUR = 10;

        int quantite = 1,indice;
        public double argent,ressource,prixVente;
        public double[] ressources;
        double prixTotal = 0;

        public magasin()
        {
            InitializeComponent();
            box_qte.Text = quantite.ToString();
            quantite = 0;


        }

        private void but_vendre_Click(object sender, RoutedEventArgs e)
        {
            if (liste_materiaux.SelectedItem == null)
            {
                MessageBox.Show(this,"Erreur, vous n'avez sélectionné aucune ressource","Erreur",MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (quantite < 1)
            {
                MessageBox.Show(this,"Erreur, la quantité doit être supérieur ou égale à 1", "Erreur de plage", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                string materiaux = ((ComboBoxItem)this.liste_materiaux.SelectedItem).Content.ToString();
                Console.WriteLine(materiaux);
                switch (materiaux)
                {
                    case "pierre":
                        indice = 0;
                        ressource = ressources[indice];
                        prixVente = PRIX_PIERRE;
                        break;
                    case "bois":
                        indice = 1;
                        ressource = ressources[indice];
                        prixVente = PRIX_BOIS;
                        break;
                    case "metal":
                        indice = 2;
                        ressource = ressources[indice];
                        prixVente = PRIX_METAL;
                        break;
                    case "ciment":
                        indice = 3;
                        ressource = ressources[indice];
                        prixVente = PRIX_CIMENT;
                        break;
                    case "futur":
                        indice = 4;
                        ressource = ressources[indice];
                        prixVente = PRIX_FUTUR;
                        break;
                }
                if (ressource - quantite < 0)
                {
                    MessageBox.Show(this, "Vous n'avez pas assez de " + materiaux, "Quantité insufisante", MessageBoxButton.OK, MessageBoxImage.Warning);

                }
                else
                {
                    prixTotal = quantite * prixVente;
                    ressource -= quantite;
                    ressources[indice] = ressource;
                    Console.WriteLine(ressource);
                    Console.WriteLine(quantite);


                    MessageBox.Show(this, "Argent gagné: " + prixTotal + "$", "Vendu !", MessageBoxButton.OK, MessageBoxImage.Information);
                    argent = prixTotal;
                    Console.WriteLine("arg" + argent);
                    for (int i = 0; i < ressources.Length; i++)
                    {
                        Console.Write(ressources[i] + ",");
                    }

                    this.DialogResult = true;
                }
            }
        }
            

        private void but_plus_Click(object sender, RoutedEventArgs e)
        {
            quantite = int.Parse(box_qte.Text) + 1;
            box_qte.Text = quantite.ToString();
        }

        private void but_moins_Click(object sender, RoutedEventArgs e)
        {
            if (quantite > 1)
            {
                quantite = int.Parse(box_qte.Text) - 1;
                box_qte.Text = quantite.ToString();

            }
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
