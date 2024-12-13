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
        double argent = 0;
        int niveauMairie = 1;
        double argentParClick = 1;
        int argentParSecond = 1;
        double prixMairie = 10;

        double[] ressources = [0,50,10,25,7];
        int niveauCarriere = 1;
        double prixCarriere = 5;
        double pierreParClick = 1;
        int pierreParSeconde = 1;

        DispatcherTimer minuteur;
        int conteurCarriere = 0;
        int conteurMairie = 0;
        private static MediaPlayer musique;
        double volume =0.5;


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
            musique.Volume = volume/100;
        }

        private void InitMinuteur()
        {
            minuteur = new DispatcherTimer();
            minuteur.Interval = TimeSpan.FromMilliseconds(20);
            minuteur.Tick += minuteurTick;
            minuteur.Start();
        }

        private void minuteurTick(object? sender, EventArgs e)
        {
            conteurCarriere++;
            conteurMairie++;

            if (conteurCarriere >= 20 && niveauCarriere >= 10)
            {
                ressources[0] += pierreParSeconde;
                lab_pierre.Content = ressources[0].ToString();

                Point relativePosition = carriere.TransformToAncestor(this).Transform(new Point(0, 0));
                AfficherTexte(relativePosition, "+" + pierreParSeconde);

                conteurCarriere = 0;
            }

            if (conteurMairie >= 20 && niveauMairie >= 10)
            {
                argent += argentParSecond;
                lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);

                Point relativePosition = mairie.TransformToAncestor(this).Transform(new Point(0, 0));
                AfficherTexte(relativePosition, "+" + argentParSecond + " €");

                conteurMairie = 0;
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
            lab_pierre.Content = ressources[0].ToString();

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

        private void button_Click_Vente_Pierre(object sender, RoutedEventArgs e)
        {
            double montantVente = ressources[0] * 0.1;
            argent += montantVente;
            ressources[0] = 0;
            lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
            lab_pierre.Content = ressources[0].ToString();
        }

        private void but_marche_Click(object sender, RoutedEventArgs e)
        {
            magasin menu_magasin = new magasin();
            menu_magasin.ressources = ressources; 
            menu_magasin.argent = argent;
            menu_magasin.ShowDialog();
            if (menu_magasin.DialogResult == true)
            {
                ressources = menu_magasin.ressources;
                lab_pierre.Content = ressources[0];
                argent += menu_magasin.argent;
                lab_argent.Content = argent + "€";
            }
        }

        private void btn_Click_Classement(object sender, RoutedEventArgs e)
        {
            Classement menu_classement = new Classement();
            menu_classement.ShowDialog();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) // code de triche ctrl+g
        {
            if (e.Key == Key.G && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                argent += 100000;
                lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
            }
        }

        
    }
}