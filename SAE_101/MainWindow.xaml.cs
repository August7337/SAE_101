using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Media;
using System.Security.Cryptography;
using System.Reflection.Metadata.Ecma335;

namespace SAE_101
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly double TORNADE_ARGENT = 0.05;
        static readonly double TORNADE_RESSOURCES = 0.10;
        static readonly int PAS_MOUVEMENT = 3;

        double argent = 0;
        int niveauMairie = 1;
        double argentParClick = 1;
        int argentParSecond = 1;
        double prixMairie = 10;

        int[] ressources = [0, 0, 0, 0, 0];

        int niveauCarriere = 1;
        double prixCarriere = 5;
        int pierreParClick = 1;
        int pierreParSeconde = 1;

        int niveauScierie = 1;
        double prixScierie = 15;
        int boisParClick = 1;
        int boisParSecond = 1;

        int niveauDecharge = 1;
        double prixDecharge = 20;
        int metalParClick = 1;
        int metalParSecond = 1;

        int niveauCimenterie = 1;
        double prixCimenterie = 30;
        int cimentParClick = 1;
        int cimentParSecond = 1;

        int niveauFuturiste = 1;
        double prixFuturiste = 50;
        int futurParClick = 1;
        int futurParSecond = 1;

        int niveauMaisonPierre = 0;
        int prixMaisonPierre = 50;

        DispatcherTimer minuteur;
        DispatcherTimer minuteurEvent;

        int conteur = 0;
        private static MediaPlayer musique;
        double volume = 50;
        bool premierPassage = true;

        string achatDefense;
        bool catastrophe = false;

        bool droite;
        bool gauche;

        public MainWindow()
        {
            InitializeComponent();
            Menu menu_accueil = new Menu();
            menu_accueil.ShowDialog(); // appel et afichage de la fenêtre menu d'acccueil
            if (menu_accueil.DialogResult == false)
                Application.Current.Shutdown();
            InitMinuteur();
        }

        public static void InitMusique()
        {
            musique = new MediaPlayer();
            musique.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "sons/musiqueFond.mp3"));
            musique.MediaEnded += RelanceMusique;
            musique.Play();
        }

        private static void RelanceMusique(object? sender, EventArgs e)
        {
            musique.Position = TimeSpan.Zero;
            musique.Play();
        }



        public static void VolumeMusique(double volume)
        {
            musique.Volume = volume / 100;
        }

        private void barre_volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (premierPassage == true)
            {
                MainWindow.InitMusique();
                premierPassage = false;
            }
            volume = barre_volume.Value;
            VolumeMusique(volume);
        }

        private void InitMinuteur()
        {
            minuteur = new DispatcherTimer();
            minuteur.Interval = TimeSpan.FromMilliseconds(16);
            minuteur.Tick += minuteurTick;
            minuteur.Start();
        }

        private void minuteurTick(object? sender, EventArgs e)
        {
            conteur++;

            if (droite)
            {
                Canvas.SetLeft(stackCarriere, Canvas.GetLeft(stackCarriere) - PAS_MOUVEMENT);
                Canvas.SetLeft(stackCimenterie, Canvas.GetLeft(stackCimenterie) - PAS_MOUVEMENT);
                Canvas.SetLeft(stackDecharge, Canvas.GetLeft(stackDecharge) - PAS_MOUVEMENT);
                Canvas.SetLeft(stackFuturiste, Canvas.GetLeft(stackFuturiste) - PAS_MOUVEMENT);
                Canvas.SetLeft(stackMairie, Canvas.GetLeft(stackMairie) - PAS_MOUVEMENT);
                Canvas.SetLeft(stackScierie, Canvas.GetLeft(stackScierie) - PAS_MOUVEMENT);
                Canvas.SetLeft(stackMaisonPierre, Canvas.GetLeft(stackMaisonPierre) - PAS_MOUVEMENT);
            }

            if (gauche)
            {
                Canvas.SetLeft(stackCarriere, Canvas.GetLeft(stackCarriere) + PAS_MOUVEMENT);
                Canvas.SetLeft(stackCimenterie, Canvas.GetLeft(stackCimenterie) + PAS_MOUVEMENT);
                Canvas.SetLeft(stackDecharge, Canvas.GetLeft(stackDecharge) + PAS_MOUVEMENT);
                Canvas.SetLeft(stackFuturiste, Canvas.GetLeft(stackFuturiste) + PAS_MOUVEMENT);
                Canvas.SetLeft(stackMairie, Canvas.GetLeft(stackMairie) + PAS_MOUVEMENT);
                Canvas.SetLeft(stackScierie, Canvas.GetLeft(stackScierie) + PAS_MOUVEMENT);
                Canvas.SetLeft(stackMaisonPierre, Canvas.GetLeft(stackMaisonPierre) + PAS_MOUVEMENT);
            }

            if (conteur >= 20)
            {
                if (niveauMairie >= 10)
                {
                    argent += argentParSecond * (1 + (double)niveauMaisonPierre/10);
                    lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);

                    Point relativePosition = mairie.TransformToAncestor(this).Transform(new Point(0, 0));
                    AfficherTexte(relativePosition, "+" + argentParSecond * (1 + (double)niveauMaisonPierre / 10) + " €");
                }

                if (niveauCarriere >= 10)
                {
                    ressources[0] += pierreParSeconde;
                    AfficheRessource(0);
                    Point relativePosition = carriere.TransformToAncestor(this).Transform(new Point(0, 0));
                    AfficherTexte(relativePosition, "+" + pierreParSeconde);
                }

                if (niveauScierie >= 10)
                {
                    ressources[1] += boisParSecond;
                    AfficheRessource(1);
                    Point relativePosition = scierie.TransformToAncestor(this).Transform(new Point(0, 0));
                    AfficherTexte(relativePosition, "+" + boisParSecond);
                }

                if (niveauDecharge >= 10)
                {
                    ressources[2] += metalParSecond;
                    AfficheRessource(2);
                    Point relativePosition = decharge.TransformToAncestor(this).Transform(new Point(0, 0));
                    AfficherTexte(relativePosition, "+" + metalParSecond);
                }

                if (niveauCimenterie >= 10)
                {
                    ressources[3] += cimentParSecond;
                    AfficheRessource(3);
                    Point relativePosition = cimenterie.TransformToAncestor(this).Transform(new Point(0, 0));
                    AfficherTexte(relativePosition, "+" + cimentParSecond);
                }

                if (niveauFuturiste >= 10)
                {
                    ressources[4] += futurParSecond;
                    AfficheRessource(4);
                    Point relativePosition = futuriste.TransformToAncestor(this).Transform(new Point(0, 0));
                    AfficherTexte(relativePosition, "+" + futurParSecond);
                }

                conteur = 0;
            }
        }

        private void button_Click_Mairie(object sender, RoutedEventArgs e)
        {
            argent += argentParClick;
            lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);

            Point position = Mouse.GetPosition(canvasAnimation);
            AfficherTexte(position, "+" + argentParClick + " €");
        }

        private void button_Click_Achat_Mairie(object sender, RoutedEventArgs e)
        {
            if (argent >= prixMairie)
            {
                niveauMairie++;
                argent -= prixMairie;
                argentParClick++;
                prixMairie = prixMairie * 1.25;
                argentParSecond = niveauMairie;
                lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
                buttonAchatMairie.Content = "Ammelioration " + prixMairie.ToString("C", CultureInfo.CurrentCulture);
                labNiveauMairie.Content = "Niveau " + niveauMairie.ToString();
            }
        }

        private void button_Click_Achat_Mairie_Max(object sender, RoutedEventArgs e)
        {
            if (argent >= prixMairie)
            {
                int achatsMax = (int)Math.Floor(Math.Log(1 - (argent * (1 - 1.25)) / prixMairie) / Math.Log(1.25));
                double totalCost = prixMairie * (1 - Math.Pow(1.25, achatsMax)) / (1 - 1.25);

                niveauMairie += achatsMax;
                argent -= totalCost;
                argentParClick += achatsMax;
                prixMairie = prixMairie * Math.Pow(1.25, achatsMax);
                argentParSecond = niveauMairie;
                lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
                buttonAchatMairie.Content = "Amélioration " + prixMairie.ToString("C", CultureInfo.CurrentCulture);
                labNiveauMairie.Content = "Niveau " + niveauMairie.ToString();
            }
        }

        private void button_Click_Carriere(object sender, RoutedEventArgs e)
        {
            ressources[0] += pierreParClick;
            AfficheRessource(0);
            Point position = Mouse.GetPosition(canvasAnimation);
            AfficherTexte(position, "+" + pierreParClick);
        }

        private void button_Click_Achat_Carriere(object sender, RoutedEventArgs e)
        {
            if (argent >= prixCarriere)
            {
                argent -= prixCarriere;
                pierreParClick++;
                niveauCarriere++;
                prixCarriere = prixCarriere * 1.25;
                lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
                buttonAchatCarriere.Content = "Ammelioration " + prixCarriere.ToString("C", CultureInfo.CurrentCulture);
                labNiveauCarriere.Content = "Niveau " + niveauCarriere.ToString();
                pierreParSeconde = niveauCarriere;
            }
        }

        private void button_Click_Achat_Carriere_Max(object sender, RoutedEventArgs e)
        {
            if (argent >= prixCarriere)
            {
                int achatsMax = (int)Math.Floor(Math.Log(1 - (argent * (1 - 1.25)) / prixCarriere) / Math.Log(1.25));
                double totalCost = prixCarriere * (1 - Math.Pow(1.25, achatsMax)) / (1 - 1.25);

                niveauCarriere += achatsMax;
                argent -= totalCost;
                pierreParClick += achatsMax;
                prixCarriere = prixCarriere * Math.Pow(1.25, achatsMax);
                pierreParSeconde = niveauCarriere;
                lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
                buttonAchatCarriere.Content = "Amélioration " + prixCarriere.ToString("C", CultureInfo.CurrentCulture);
                labNiveauCarriere.Content = "Niveau " + niveauCarriere.ToString();
            }
        }

        private void AfficherTexte(Point position, string texte)
        {
            TextBlock texteBlock = new TextBlock
            {
                Text = texte,
                Foreground = new SolidColorBrush(Colors.Black),
                FontSize = 21,
                FontWeight = FontWeights.Bold
            };

            // Ajouter un effet de contour
            texteBlock.Effect = new DropShadowEffect
            {
                Color = Colors.Yellow,
                BlurRadius = 5,
                ShadowDepth = 0,
                Opacity = 1
            };

            Canvas.SetLeft(texteBlock, position.X + 10);
            Canvas.SetTop(texteBlock, position.Y - 20);
            canvasAnimation.Children.Add(texteBlock);

            // Animation de disparition
            DoubleAnimation animationDisparition = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(1)));
            animationDisparition.Completed += (s, a) => canvasAnimation.Children.Remove(texteBlock);
            texteBlock.BeginAnimation(UIElement.OpacityProperty, animationDisparition);

            // Animation de montée
            TranslateTransform translateTransform = new TranslateTransform();
            texteBlock.RenderTransform = translateTransform;
            DoubleAnimation animationMontee = new DoubleAnimation(0, -30, new Duration(TimeSpan.FromSeconds(1)));
            translateTransform.BeginAnimation(TranslateTransform.YProperty, animationMontee);
        }

        private void AfficheArgent()
        {
            lab_argent.Content = argent + " €";
        }

        private void AfficheRessource(int ressource)
        {
            switch (ressource)
            {
                case 0:
                    lab_pierre.Content = ressources[0].ToString();
                    break;
                case 1:
                    lab_bois.Content = ressources[1].ToString();
                    break;
                case 2:
                    lab_metal.Content = ressources[2].ToString();
                    break;
                case 3:
                    lab_ciment.Content = ressources[3].ToString();
                    break;
                case 4:
                    lab_futur.Content = ressources[4].ToString();
                    break;
                default: break;
            }
        }

        private void button_Click_Vente_Pierre(object sender, RoutedEventArgs e)
        {
            double montantVente = ressources[0] * 0.1;
            argent += montantVente;
            ressources[0] = 0;
            lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
            AfficheRessource(0);
        }

        private void but_marche_Click(object sender, RoutedEventArgs e)
        {
            Marche menu_magasin = new Marche();
            menu_magasin.ressources = ressources;
            menu_magasin.argent = argent;
            menu_magasin.ShowDialog();
            if (menu_magasin.DialogResult == true)
            {
                ressources = menu_magasin.ressources;
                argent += menu_magasin.argent;
                AfficheArgent();
                AfficheRessource(menu_magasin.indice);
            }
        }

        private void but_defense_Click(object sender, RoutedEventArgs e)
        {
            if (catastrophe)
            {
                Defense menu_defense = new Defense();
                menu_defense.argent = argent;
                menu_defense.AfficheArgent();
                menu_defense.ShowDialog();

                if (menu_defense.DialogResult == true)
                {
                    achatDefense = menu_defense.achat;
                    argent = menu_defense.argent;
                    ArretTornade();
                    AfficheArgent();
                }
            }
            else
            {
                MessageBox.Show("Le magasin est fermé, revenez plus tard", "Aucun évènement", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void btn_Click_Classement(object sender, RoutedEventArgs e)
        {
            Classement menu_classement = new Classement();
            menu_classement.ShowDialog();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) // code de triche ctrl+g
        {
            //Code de triche
            if (e.Key == Key.G && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                argent += 100000;
                lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
            }

            if (e.Key == Key.Right)
                droite = true;

            if (e.Key == Key.Left)
                gauche = true;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
                droite = false;

            if (e.Key == Key.Left)
                gauche = false;
        }
            //essai IA

            if (e.Key == Key.T)
            {
                Tornade();
            }
        }

        private void button_Click_Decharge(object sender, RoutedEventArgs e)
        {
            ressources[2] += metalParClick;
            AfficheRessource(2);
            Point position = Mouse.GetPosition(canvasAnimation);
            AfficherTexte(position, "+" + metalParClick);
        }

        private void button_Click_Achat_Decharge(object sender, RoutedEventArgs e)
        {
            if (argent >= prixDecharge)
            {
                argent -= prixDecharge;
                metalParClick++;
                niveauDecharge++;
                prixDecharge = prixDecharge * 1.25;
                lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
                buttonAchatDecharge.Content = "Ammelioration " + prixDecharge.ToString("C", CultureInfo.CurrentCulture);
                labNiveauDecharge.Content = "Niveau " + niveauDecharge.ToString();
                metalParSecond = niveauDecharge;
            }
        }

        private void button_Click_Achat_Decharge_Max(object sender, RoutedEventArgs e)
        {
            if (argent >= prixDecharge)
            {
                int achatsMax = (int)Math.Floor(Math.Log(1 - (argent * (1 - 1.25)) / prixDecharge) / Math.Log(1.25));
                double totalCost = prixDecharge * (1 - Math.Pow(1.25, achatsMax)) / (1 - 1.25);

                niveauDecharge += achatsMax;
                argent -= totalCost;
                metalParClick += achatsMax;
                prixDecharge = prixDecharge * Math.Pow(1.25, achatsMax);
                metalParSecond = niveauDecharge;
                lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
                buttonAchatDecharge.Content = "Amélioration " + prixDecharge.ToString("C", CultureInfo.CurrentCulture);
                labNiveauDecharge.Content = "Niveau " + niveauDecharge.ToString();
            }
        }

        private void button_Click_Scierie(object sender, RoutedEventArgs e)
        {
            ressources[1] += metalParClick;
            AfficheRessource(1);
            Point position = Mouse.GetPosition(canvasAnimation);
            AfficherTexte(position, "+" + boisParClick);
        }

        private void button_Click_Achat_Scierie(object sender, RoutedEventArgs e)
        {
            if (argent >= prixScierie)
            {
                argent -= prixScierie;
                boisParClick++;
                niveauScierie++;
                prixScierie = prixScierie * 1.25;
                lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
                buttonAchatScierie.Content = "Ammelioration " + prixScierie.ToString("C", CultureInfo.CurrentCulture);
                labNiveauScierie.Content = "Niveau " + niveauScierie.ToString();
                boisParSecond = niveauScierie;
            }
        }

        private void button_Click_Achat_Scierie_Max(object sender, RoutedEventArgs e)
        {
            if (argent >= prixScierie)
            {
                int achatsMax = (int)Math.Floor(Math.Log(1 - (argent * (1 - 1.25)) / prixScierie) / Math.Log(1.25));
                double totalCost = prixScierie * (1 - Math.Pow(1.25, achatsMax)) / (1 - 1.25);

                niveauScierie += achatsMax;
                argent -= totalCost;
                boisParClick += achatsMax;
                prixScierie = prixScierie * Math.Pow(1.25, achatsMax);
                boisParSecond = niveauScierie;
                lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
                buttonAchatScierie.Content = "Amélioration " + prixScierie.ToString("C", CultureInfo.CurrentCulture);
                labNiveauScierie.Content = "Niveau " + niveauScierie.ToString();
            }
        }

        private void button_Click_Cimenterie(object sender, RoutedEventArgs e)
        {
            ressources[3] += cimentParClick;
            AfficheRessource(3);

            Point position = Mouse.GetPosition(canvasAnimation);
            AfficherTexte(position, "+" + cimentParClick);
        }

        private void button_Click_Achat_Cimenterie(object sender, RoutedEventArgs e)
        {
            if (argent >= prixCimenterie)
            {
                argent -= prixCimenterie;
                cimentParClick++;
                niveauCimenterie++;
                prixCimenterie = prixCimenterie * 1.25;
                lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
                buttonAchatCimenterie.Content = "Ammelioration " + prixCimenterie.ToString("C", CultureInfo.CurrentCulture);
                labNiveauCimenterie.Content = "Niveau " + niveauCimenterie.ToString();
                cimentParSecond = niveauCimenterie;
            }
        }

        private void button_Click_Achat_Cimenterie_Max(object sender, RoutedEventArgs e)
        {
            if (argent >= prixCimenterie)
            {
                int achatsMax = (int)Math.Floor(Math.Log(1 - (argent * (1 - 1.25)) / prixCimenterie) / Math.Log(1.25));
                double totalCost = prixCimenterie * (1 - Math.Pow(1.25, achatsMax)) / (1 - 1.25);

                niveauCimenterie += achatsMax;
                argent -= totalCost;
                cimentParClick += achatsMax;
                prixCimenterie = prixCimenterie * Math.Pow(1.25, achatsMax);
                cimentParSecond = niveauCimenterie;
                lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
                buttonAchatCimenterie.Content = "Amélioration " + prixCimenterie.ToString("C", CultureInfo.CurrentCulture);
                labNiveauCimenterie.Content = "Niveau " + niveauCimenterie.ToString();
            }
        }

        private void button_Click_Futuriste(object sender, RoutedEventArgs e)
        {
            ressources[4] += futurParClick;
            AfficheRessource(4);

            Point position = Mouse.GetPosition(canvasAnimation);
            AfficherTexte(position, "+" + futurParClick);
        }

        private void button_Click_Achat_Futuriste(object sender, RoutedEventArgs e)
        {
            if (argent >= prixFuturiste)
            {
                argent -= prixFuturiste;
                futurParClick++;
                niveauFuturiste++;
                prixFuturiste = prixFuturiste * 1.25;
                lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
                buttonAchatFuturiste.Content = "Ammelioration " + prixFuturiste.ToString("C", CultureInfo.CurrentCulture);
                labNiveauFuturiste.Content = "Niveau " + niveauFuturiste.ToString();
                futurParSecond = niveauFuturiste;
            }
        }

        private void button_Click_Achat_Futuriste_Max(object sender, RoutedEventArgs e)
        {
            if (argent >= prixFuturiste)
            {
                int achatsMax = (int)Math.Floor(Math.Log(1 - (argent * (1 - 1.25)) / prixFuturiste) / Math.Log(1.25));
                double totalCost = prixFuturiste * (1 - Math.Pow(1.25, achatsMax)) / (1 - 1.25);

                niveauFuturiste += achatsMax;
                argent -= totalCost;
                futurParClick += achatsMax;
                prixFuturiste = prixFuturiste * Math.Pow(1.25, achatsMax);
                futurParSecond = niveauFuturiste;
                lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
                buttonAchatFuturiste.Content = "Amélioration " + prixFuturiste.ToString("C", CultureInfo.CurrentCulture);
                labNiveauFuturiste.Content = "Niveau " + niveauFuturiste.ToString();
            }
        }

        private void button_Click_Achat_Maison_Pierre(object sender, RoutedEventArgs e)
        {
            if (ressources[0] >= prixMaisonPierre)
            {
                ressources[0] -= prixMaisonPierre;
                niveauMaisonPierre++;
                prixMaisonPierre = (int)(prixMaisonPierre * 1.25);
                lab_pierre.Content = ressources[0].ToString();
                buttonAchatMaisonPierre.Content = "Ammelioration " + prixMaisonPierre.ToString();
                labNiveauMaisonPierre.Content = "Niveau " + niveauMaisonPierre.ToString();
            }
        }

        private void button_Click_Achat_Maison_Pierre_Max(object sender, RoutedEventArgs e)
        {
            if (argent >= prixFuturiste)
            {
                int achatsMax = (int)Math.Floor(Math.Log(1 - (ressources[0] * (1 - 1.25)) / prixMaisonPierre) / Math.Log(1.25));
                int totalCost = (int)(prixMaisonPierre * (1 - Math.Pow(1.25, achatsMax)) / (1 - 1.25));

                niveauMaisonPierre += achatsMax;
                ressources[0] -= totalCost;
                prixMaisonPierre = (int)(prixMaisonPierre * Math.Pow(1.25, achatsMax));
                lab_pierre.Content = ressources[0].ToString();
                buttonAchatMaisonPierre.Content = "Ammelioration " + prixMaisonPierre.ToString();
                labNiveauMaisonPierre.Content = "Niveau " + niveauMaisonPierre.ToString();
            }
        }

        }

        private void Tornade()
        {
            catastrophe = true;
            minuteurEvent = new DispatcherTimer();
            minuteurEvent.Interval = TimeSpan.FromSeconds(10);
            minuteurEvent.Start();
            minuteurEvent.Tick += minuteurTornadeTick;

        }

        private void minuteurTornadeTick(object? sender, EventArgs e)
        {
            double prixTornade = Math.Round(argent * TORNADE_ARGENT, 0);
            if (prixTornade < 1)
            {
                prixTornade = 1;
            }
            Console.WriteLine(prixTornade);
            if (argent > 0)
            {
                argent -= prixTornade;
                for (int i = 0; i <= 4; i++)
                {
                    double ressourcesEnMoins = ressources[i] * TORNADE_RESSOURCES;
                    if(ressourcesEnMoins < 1)
                    {
                        ressourcesEnMoins = 1;
                    }
                    Console.WriteLine(ressourcesEnMoins);
                    if (ressources[i] > 0)
                    {
                        ressources[i] -= (int)ressourcesEnMoins;
                    }
                         
                }
                AfficheArgent();
                for (int j = 0; j <= 4; j++)
                {
                    AfficheRessource(j);
                }
            }


        }
        
        private void ArretTornade()
        {
            catastrophe = false;
            minuteurEvent.Stop();
            Console.WriteLine("Passage");
        }

    }
}