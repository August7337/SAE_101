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
    /// Logique d'interaction pour Marche.xaml
    /// </summary>

    public partial class Marche : Window
    {
        static readonly double PRIX_PIERRE = 0.5;
        static readonly double PRIX_BOIS = 1;
        static readonly double PRIX_METAL = 2;
        static readonly double PRIX_CIMENT = 5;
        static readonly double PRIX_FUTUR = 10;

        int quantite = 1;
        public int indice,ressource;
        public double argent,prixVente;
        public int[] ressources;
        double prixTotal = 0;
        bool verifSaisie;
        bool max = false; // A chaque ouverture de la fenêtre, on suppose que l'utilisateur ne veut pas vendre tout son stock d'une ressource 

        public Marche()
        {
            InitializeComponent();
            box_qte.Text = quantite.ToString();
        }

        private void but_vendre_Click(object sender, RoutedEventArgs e)
        {
            verifSaisie = int.TryParse(box_qte.Text, out quantite);
            if (liste_materiaux.SelectedItem == null) // condition vérifiant si l'utilisateur à bien choisi une resssource
            {
                MessageBox.Show(this,"Erreur, vous n'avez sélectionné aucune ressource","Erreur",MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!verifSaisie)
            {
                MessageBox.Show(this, "Erreur, la quantité doit être un nombre entier", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (quantite < 1)
            {
                MessageBox.Show(this,"Erreur, la quantité doit être supérieur ou égale à 1", "Erreur de plage", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                string materiaux = ((ComboBoxItem)this.liste_materiaux.SelectedItem).Content.ToString();
                Console.WriteLine(materiaux);
                switch (materiaux)  // switch permettant de connaître la ressource que l'utilisateur souhaite vendre
                {
                    case "Pierre (0.5€)":
                        indice = 0;
                        ressource = ressources[indice];
                        prixVente = PRIX_PIERRE;
                        break;
                    case "Bois (1€)":
                        indice = 1;
                        ressource = ressources[indice];
                        prixVente = PRIX_BOIS;
                        break;
                    case "Métal (2€)":
                        indice = 2;
                        ressource = ressources[indice];
                        prixVente = PRIX_METAL;
                        break;
                    case "Ciment (5€)":
                        indice = 3;
                        ressource = ressources[indice];
                        prixVente = PRIX_CIMENT;
                        break;
                    case "Futur (10€)":
                        indice = 4;
                        ressource = ressources[indice];
                        prixVente = PRIX_FUTUR;
                        break;
                }
                if (max == true)
                {
                    quantite = ressource; // on écrase le contenu de la variable quantité par le stock de la ressource choisi 
                    box_qte.Text = ressource.ToString();
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
;


                    MessageBox.Show(this, "Argent gagné: " + prixTotal + "€", "Vendu !", MessageBoxButton.OK, MessageBoxImage.Information);
                    argent = prixTotal;
                    Console.WriteLine("arg" + argent);
                    for (int i = 0; i < ressources.Length; i++)
                    {
                        Console.Write(ressources[i] + ",");
                    }

                    this.DialogResult = true; // la fenêtre se ferme et les changements sont appliqués dans la MainWindow
                }
            }
        }

        private void but_moins_Click(object sender, RoutedEventArgs e)
        {
            if (quantite > 1)
            {
                verifSaisie = int.TryParse(box_qte.Text,out quantite);
                if (verifSaisie)
                {
                    quantite--;
                }
                box_qte.Text = quantite.ToString();

            }
        }

        private void but_plus_Click(object sender, RoutedEventArgs e)
        {
            verifSaisie = int.TryParse(box_qte.Text, out quantite);
            if (verifSaisie)
            {
                quantite++;
            }
            box_qte.Text = quantite.ToString();
        }

        private void but_max_vente_Click(object sender, RoutedEventArgs e)
        {
            max = true;  // ce booléen permet d'écraser la valeur de la variable quantité par la quantité possédé de la ressource choisie
            but_vendre_Click(sender, e);
        }

        private void marche_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                but_vendre_Click(sender,e); // appuyer sur la touche espace équivaut à clier sur le bouton vendre
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


    }
}
