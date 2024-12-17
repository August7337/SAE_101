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
using System.Security.Cryptography.X509Certificates;

namespace SAE_101
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly double TORNADE_ARGENT = 0.05;
        static readonly double TORNADE_RESSOURCES = 0.10;
        static readonly int MALADIE_ARGENT = 50;
        static readonly int PAS_MOUVEMENT = 20;
        static readonly int UNE_MINUTE_EN_TICK = 500; //3750
        static readonly int TROIS_MINUTES_EN_TICK = 1000; // 11250

        double argent = 0;
        int niveauMairie = 1;
        double argentParClick = 1;
        int argentParSecond = 1;
        double prixMairie = 10;

        int[] ressources = [0, 0, 0, 0, 0];
        int[] niveauMaisons = [0, 0, 0, 0, 0];

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

        int prixMaisonPierre = 50;
        int prixMaisonBois = 50;
        int prixMaisonMetal = 50;
        int prixMaisonCiment = 50;
        int prixMaisonFuture = 50;

        DispatcherTimer minuteur;
        DispatcherTimer minuteurEvent;

        int compteur = 0, compteurTonnerre = 0, compteurMaladie = 0, compteurFeu = 0, compteurDeclencheFoudre = 0;
        int compteurDeclencheMaladie = 0;
        private static MediaPlayer musique;
        double volume = 50;
        bool premierPassage = true;


        int tpsDeclencheFoudre;
        string achatDefense;
        bool catastrophe = false;
        string objetRequis = ""; // permet de vérifier si l'objet acheté pour contre une catastrophe est le bon 
        public static Random rdn = new Random();
        int usineTouche, niveauReelUsine; // niveauReelUsine va stocker le niveau de l'usine choisi par le random 
        int maisonTouche;
        double prixTornade = 0, ressourcesEnMoins = 0;

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
            tpsDeclencheFoudre = DeclencheFoudre();
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
            minuteurEvent = new DispatcherTimer();

        }

        private void minuteurTick(object? sender, EventArgs e)
        {
            compteur++;

            if (droite)
            {
                Canvas.SetLeft(stackCarriere, Canvas.GetLeft(stackCarriere) - PAS_MOUVEMENT);
                Canvas.SetLeft(stackCimenterie, Canvas.GetLeft(stackCimenterie) - PAS_MOUVEMENT);
                Canvas.SetLeft(stackDecharge, Canvas.GetLeft(stackDecharge) - PAS_MOUVEMENT);
                Canvas.SetLeft(stackFuturiste, Canvas.GetLeft(stackFuturiste) - PAS_MOUVEMENT);
                Canvas.SetLeft(stackMairie, Canvas.GetLeft(stackMairie) - PAS_MOUVEMENT);
                Canvas.SetLeft(stackScierie, Canvas.GetLeft(stackScierie) - PAS_MOUVEMENT);
                Canvas.SetLeft(stackMaisonPierre, Canvas.GetLeft(stackMaisonPierre) - PAS_MOUVEMENT);
                Canvas.SetLeft(stackMaisonBois, Canvas.GetLeft(stackMaisonBois) - PAS_MOUVEMENT);
                Canvas.SetLeft(stackMaisonMetal, Canvas.GetLeft(stackMaisonMetal) - PAS_MOUVEMENT);
                Canvas.SetLeft(stackMaisonCiment, Canvas.GetLeft(stackMaisonCiment) - PAS_MOUVEMENT);
                Canvas.SetLeft(stackMaisonFuture, Canvas.GetLeft(stackMaisonFuture) - PAS_MOUVEMENT);
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
                Canvas.SetLeft(stackMaisonBois, Canvas.GetLeft(stackMaisonBois) + PAS_MOUVEMENT);
                Canvas.SetLeft(stackMaisonMetal, Canvas.GetLeft(stackMaisonMetal) + PAS_MOUVEMENT);
                Canvas.SetLeft(stackMaisonCiment, Canvas.GetLeft(stackMaisonCiment) + PAS_MOUVEMENT);
                Canvas.SetLeft(stackMaisonFuture, Canvas.GetLeft(stackMaisonFuture) + PAS_MOUVEMENT);
            }

            if (compteur >= 20)
            {
                if (niveauMairie >= 10)
                {
                    double calcule = argentParSecond * (1 + (double)niveauMaisons[0] / 10) * (1 + (double)niveauMaisons[1] / 10) * (1 + (double)niveauMaisons[2] / 10) * (1 + (double)niveauMaisons[3] / 10) * (1 + (double)niveauMaisons[4] / 10);
                    argent += calcule;
                    lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);

                    Point relativePosition = mairie.TransformToAncestor(this).Transform(new Point(0, 0));
                    AfficherTexte(relativePosition, "+" + Math.Round(calcule, 2).ToString("C", CultureInfo.CurrentCulture));
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
                compteur = 0;
            }

            if (catastrophe && objetRequis == "antiTornade")
            {
                compteurTonnerre++;
                Console.WriteLine(compteurTonnerre);

                if (compteurTonnerre >= 500)
                {
                    prixTornade = Math.Round(argent * TORNADE_ARGENT, 0);
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
                            ressourcesEnMoins = ressources[i] * TORNADE_RESSOURCES;
                            if (ressourcesEnMoins < 1)
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
                    compteurTonnerre = 0;
                }


            }
            else if (catastrophe && objetRequis == "antidote")
            {
                compteurMaladie++;
                Console.WriteLine(compteurMaladie);

                if (compteurMaladie >= 100)
                {
                    if (argent > MALADIE_ARGENT)
                    {
                        argent -= MALADIE_ARGENT;
                        AfficheArgent();
                    }
                    compteurMaladie = 0;
                }
            }
            else if (catastrophe && objetRequis == "sceauEau")
            {
                compteurFeu++;
                Console.WriteLine(compteurFeu);

                if (compteurFeu >= 350)
                {
                    if (niveauMaisons[maisonTouche] > 0)
                    {
                        niveauMaisons[maisonTouche]--;
                        AfficheNivMaisons(maisonTouche);
                    }
                    compteurFeu = 0;
                }
            }
            else if (!catastrophe)
            {
                compteurDeclencheFoudre++;
                if(compteurDeclencheFoudre == tpsDeclencheFoudre)
                {
                    Foudre();
                    tpsDeclencheFoudre = DeclencheFoudre();
                    compteurDeclencheFoudre = 0;
                }
                    
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

        private void AfficheNivMaisons(int nivMaisons)
        {
            switch (nivMaisons)
            {
                case 0:
                    labNiveauMaisonPierre.Content = "Niveau: " + niveauMaisons[0].ToString();
                    break;
                case 1:
                    labNiveauMaisonBois.Content = "Niveau: " + niveauMaisons[1].ToString();
                    break;
                case 2:
                    labNiveauMaisonMetal.Content = "Niveau: " + niveauMaisons[2].ToString();
                    break;
                case 3:
                    labNiveauMaisonCiment.Content = "Niveau: " + niveauMaisons[3].ToString();
                    break;
                case 4:
                    labNiveauMaisonFuture.Content = "Niveau: " + niveauMaisons[4].ToString();
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
                    ArretCatastrophe();
                    AfficheArgent();
                }
            }
            else
                MessageBox.Show("Le magasin est fermé, revenez plus tard", "Aucun évènement", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (e.Key == Key.T)
                Tornade();

            if (e.Key == Key.A)
                Maladie();

            if (e.Key == Key.E)
                Feu();

            if (e.Key == Key.F)
                Foudre();

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
                niveauMaisons[0]++;
                prixMaisonPierre = (int)(prixMaisonPierre * 1.25);
                lab_pierre.Content = ressources[0].ToString();
                buttonAchatMaisonPierre.Content = "Ammelioration " + prixMaisonPierre.ToString();
                labNiveauMaisonPierre.Content = "Niveau " + niveauMaisons[0].ToString();
            }
        }

        private void button_Click_Achat_Maison_Pierre_Max(object sender, RoutedEventArgs e)
        {
            if (ressources[0] >= prixMaisonPierre)
            {
                int achatsMax = (int)Math.Floor(Math.Log(1 - (ressources[0] * (1 - 1.25)) / prixMaisonPierre) / Math.Log(1.25));
                int totalCost = (int)(prixMaisonPierre * (1 - Math.Pow(1.25, achatsMax)) / (1 - 1.25));

                niveauMaisons[0] += achatsMax;
                ressources[0] -= totalCost;
                prixMaisonPierre = (int)(prixMaisonPierre * Math.Pow(1.25, achatsMax));
                lab_pierre.Content = ressources[0].ToString();
                buttonAchatMaisonPierre.Content = "Ammelioration " + prixMaisonPierre.ToString();
                labNiveauMaisonPierre.Content = "Niveau " + niveauMaisons[0].ToString();
            }
        }

        private void Tornade()
        {
            lab_catastrophe.Content = "Tornade en cours";
            Console.WriteLine("tornade");
            objetRequis = "antiTornade";
            catastrophe = true;

        }

        private void Maladie()
        {
            lab_catastrophe.Content = "Le village est malade";
            Console.WriteLine("Maladie");
            objetRequis = "antidote";
            catastrophe = true;
        }

        private void Foudre()
        {
            lab_catastrophe.Content = "La fourdre s'est abbattu";
            objetRequis = "paratonnerre";
            catastrophe = true;
            usineTouche = rdn.Next(0, 5);
            switch (usineTouche) // on désactive le bouton corespondant à l'indice (selon indice des ressources)
            {
                case 0:
                    carriere.IsEnabled = false;// on désactive la carrière
                    buttonAchatCarriere.IsEnabled = false;
                    buttonAchatCarriereMax.IsEnabled = false;
                    niveauReelUsine = niveauCarriere;
                    niveauCarriere = 0; // mettre le niveau à 0 désactive l'augmmentation automatique qui ne se déclenche qu'au niveau 10
                    break;
                case 1:
                    scierie.IsEnabled = false;
                    buttonAchatScierie.IsEnabled = false;
                    buttonAchatScierieMax.IsEnabled = false;    
                    niveauReelUsine = niveauScierie;
                    niveauScierie = 0;
                    break;
                case 2:
                    decharge.IsEnabled = false;
                    buttonAchatDecharge.IsEnabled = false;
                    buttonAchatDechargeMax.IsEnabled = false;
                    niveauReelUsine = niveauDecharge;
                    niveauDecharge = 0;
                    break;
                case 3:
                    cimenterie.IsEnabled= false;
                    buttonAchatCimenterie.IsEnabled = false;
                    buttonAchatCimenterieMax.IsEnabled=false;
                    niveauReelUsine = niveauCimenterie;
                    niveauCimenterie = 0;
                    break;
                case 4:
                    futuriste.IsEnabled = false;
                    buttonAchatFuturiste.IsEnabled=false;
                    buttonAchatFuturisteMax.IsEnabled=false;
                    niveauReelUsine = niveauFuturiste;
                    niveauFuturiste = 0;
                    break;
                default: break;
            }
        }

        private void Feu()
        {
            lab_catastrophe.Content = "incendie en cours";
            objetRequis = "sceauEau";
            catastrophe = true;
            maisonTouche = rdn.Next(0, 5);
        }
 
        private void ArretCatastrophe()
        {
            if (achatDefense == objetRequis)
            {
                MessageBox.Show("La catastrophe s'est arrêté ! ", "Achat réussi", MessageBoxButton.OK, MessageBoxImage.Information);
                catastrophe = false;
                Console.WriteLine("Passage");
                if (objetRequis == "paratonnerre")
                {
                    switch (usineTouche)
                    {
                        case 0:
                            carriere.IsEnabled = true; // on active la carrière
                            buttonAchatCarriere.IsEnabled = true;
                            buttonAchatCarriereMax.IsEnabled = true;
                            niveauCarriere = niveauReelUsine;

                            break;
                        case 1:
                            scierie.IsEnabled = true;
                            buttonAchatScierie.IsEnabled = true;
                            buttonAchatScierieMax.IsEnabled = true;
                            niveauScierie = niveauReelUsine;
                            break;
                        case 2:
                            decharge.IsEnabled = true;
                            buttonAchatDecharge.IsEnabled = true;
                            buttonAchatDechargeMax.IsEnabled = true;
                            niveauDecharge = niveauReelUsine;
                            break;
                        case 3:
                            cimenterie.IsEnabled = false;
                            buttonAchatCimenterie.IsEnabled = true;
                            buttonAchatCimenterieMax.IsEnabled = true;
                            niveauCimenterie = niveauReelUsine;
                            break;
                        case 4:
                            futuriste.IsEnabled = true;
                            buttonAchatFuturiste.IsEnabled = true;
                            buttonAchatFuturisteMax.IsEnabled = true;
                            niveauFuturiste = niveauReelUsine;
                            break;
                        default: break;
                    }
                }
            }

            else
            {
                MessageBox.Show("Cet objet ne permet pas d'arrêter la catastrophe en cours", "Achat réussi", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ArretTornade()
        {
            catastrophe = false;
            minuteurEvent.Stop();
            Console.WriteLine("Passage");
        }


        private void button_Click_Achat_Maison_Bois(object sender, RoutedEventArgs e)
        {
            if (ressources[1] >= prixMaisonBois)
            {
                ressources[1] -= prixMaisonBois;
                niveauMaisons[1]++;
                prixMaisonBois = (int)(prixMaisonBois * 1.25);
                lab_bois.Content = ressources[1].ToString();
                buttonAchatMaisonBois.Content = "Ammelioration " + prixMaisonBois.ToString();
                labNiveauMaisonBois.Content = "Niveau " + niveauMaisons[1].ToString();
            }
        }

        private void button_Click_Achat_Maison_Bois_Max(object sender, RoutedEventArgs e)
        {
            if (argent >= prixMaisonBois)
            {
                int achatsMax = (int)Math.Floor(Math.Log(1 - (ressources[1] * (1 - 1.25)) / prixMaisonBois) / Math.Log(1.25));
                int totalCost = (int)(prixMaisonBois * (1 - Math.Pow(1.25, achatsMax)) / (1 - 1.25));

                niveauMaisons[1] += achatsMax;
                ressources[1] -= totalCost;
                prixMaisonBois = (int)(prixMaisonBois * Math.Pow(1.25, achatsMax));
                lab_bois.Content = ressources[1].ToString();
                buttonAchatMaisonBois.Content = "Ammelioration " + prixMaisonBois.ToString();
                labNiveauMaisonBois.Content = "Niveau " + niveauMaisons[1].ToString();
            }
        }

        private void button_Click_Achat_Maison_Metal(object sender, RoutedEventArgs e)
        {
            if (ressources[2] >= prixMaisonMetal)
            {
                ressources[2] -= prixMaisonMetal;
                niveauMaisons[2]++;
                prixMaisonMetal = (int)(prixMaisonMetal * 1.25);
                lab_metal.Content = ressources[2].ToString();
                buttonAchatMaisonMetal.Content = "Ammelioration " + prixMaisonMetal.ToString();
                labNiveauMaisonMetal.Content = "Niveau " + niveauMaisons[2].ToString();
            }
        }

        private void button_Click_Achat_Maison_Metal_Max(object sender, RoutedEventArgs e)
        {
            if (ressources[2] >= prixMaisonMetal)
            {
                int achatsMax = (int)Math.Floor(Math.Log(1 - (ressources[2] * (1 - 1.25)) / prixMaisonMetal) / Math.Log(1.25));
                int totalCost = (int)(prixMaisonMetal * (1 - Math.Pow(1.25, achatsMax)) / (1 - 1.25));

                niveauMaisons[2] += achatsMax;
                ressources[2] -= totalCost;
                prixMaisonMetal = (int)(prixMaisonMetal * Math.Pow(1.25, achatsMax));
                lab_metal.Content = ressources[2].ToString();
                buttonAchatMaisonMetal.Content = "Ammelioration " + prixMaisonMetal.ToString();
                labNiveauMaisonMetal.Content = "Niveau " + niveauMaisons[2].ToString();
            }
        }

        private void button_Click_Achat_Maison_Ciment(object sender, RoutedEventArgs e)
        {
            if (ressources[3] >= prixMaisonCiment)
            {
                ressources[3] -= prixMaisonCiment;
                niveauMaisons[3]++;
                prixMaisonCiment = (int)(prixMaisonCiment * 1.25);
                lab_ciment.Content = ressources[3].ToString();
                buttonAchatMaisonCiment.Content = "Ammelioration " + prixMaisonCiment.ToString();
                labNiveauMaisonCiment.Content = "Niveau " + niveauMaisons[3].ToString();
            }
        }

        private void button_Click_Achat_Maison_Ciment_Max(object sender, RoutedEventArgs e)
        {
            if (ressources[3] >= prixMaisonCiment)
            {
                int achatsMax = (int)Math.Floor(Math.Log(1 - (ressources[3] * (1 - 1.25)) / prixMaisonCiment) / Math.Log(1.25));
                int totalCost = (int)(prixMaisonCiment * (1 - Math.Pow(1.25, achatsMax)) / (1 - 1.25));

                niveauMaisons[3] += achatsMax;
                ressources[3] -= totalCost;
                prixMaisonCiment = (int)(prixMaisonCiment * Math.Pow(1.25, achatsMax));
                lab_ciment.Content = ressources[3].ToString();
                buttonAchatMaisonCiment.Content = "Ammelioration " + prixMaisonCiment.ToString();
                labNiveauMaisonCiment.Content = "Niveau " + niveauMaisons[3].ToString();
            }
        }

        private void button_Click_Achat_Maison_Future(object sender, RoutedEventArgs e)
        {
            if (ressources[4] >= prixMaisonFuture)
            {
                ressources[4] -= prixMaisonFuture;
                niveauMaisons[4]++;
                prixMaisonFuture = (int)(prixMaisonFuture * 1.25);
                lab_futur.Content = ressources[4].ToString();
                buttonAchatMaisonFuture.Content = "Ammelioration " + prixMaisonFuture.ToString();
                labNiveauMaisonFuture.Content = "Niveau " + niveauMaisons[4].ToString();
            }
        }

        private void button_Click_Achat_Maison_Future_Max(object sender, RoutedEventArgs e)
        {
            if (ressources[4] >= prixMaisonFuture)
            {
                int achatsMax = (int)Math.Floor(Math.Log(1 - (ressources[4] * (1 - 1.25)) / prixMaisonFuture) / Math.Log(1.25));
                int totalCost = (int)(prixMaisonFuture * (1 - Math.Pow(1.25, achatsMax)) / (1 - 1.25));

                niveauMaisons[4] += achatsMax;
                ressources[4] -= totalCost;
                prixMaisonFuture = (int)(prixMaisonFuture * Math.Pow(1.25, achatsMax));
                lab_futur.Content = ressources[4].ToString();
                buttonAchatMaisonFuture.Content = "Ammelioration " + prixMaisonFuture.ToString();
                labNiveauMaisonFuture.Content = "Niveau " + niveauMaisons[4].ToString();
            }
        }

        private static int DeclencheFoudre()
        {
            int declencheurF = rdn.Next(UNE_MINUTE_EN_TICK, TROIS_MINUTES_EN_TICK + 1);
            return declencheurF;
        }

        
    }
}