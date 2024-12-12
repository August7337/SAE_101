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
        double prixMairie = 10;

        double pierre = 0;
        int niveauCarriere = 1;
        double prixCarriere = 5;
        double pierreParClick = 1;
        int pierreParSeconde = 1;
        bool pierreAutoActive = false;

        DispatcherTimer minuteur;
        int conteur = 0;

        public MainWindow()
        {
            InitializeComponent();
            Menu menu_accueil = new Menu();
            menu_accueil.ShowDialog();
            if (menu_accueil.DialogResult == false)
                Application.Current.Shutdown();
            InitMinuteur();
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
            conteur++;

            if (conteur >= 20 && niveauCarriere >= 10)
            {
                pierre += pierreParSeconde;
                lab_pierre.Content = pierre.ToString();

                Point relativePosition = carriere.TransformToAncestor(this).Transform(new Point(0, 0));
                AfficherTexte(relativePosition, "+" + pierreParSeconde);

                conteur = 0;
            }
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
                niveauMairie++;
                argent -= prixMairie;
                argentParClick++;
                prixMairie = prixMairie * 1.25;
                lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
                buttonAchatMairie.Content = "Ammelioration " + prixMairie.ToString("C", CultureInfo.CurrentCulture);
                labNiveauMairie.Content = "Niveau " + niveauMairie.ToString();
            }
        }
    
        private void button_Click_Achat_Mairie_Max(object sender, RoutedEventArgs e)
        {
            if (argent >= prixMairie)
            {
                int achatsMax = (int)Math.Floor(argent / prixMairie);
                niveauMairie += achatsMax;
                Console.WriteLine("achat max mairie : " + achatsMax);
                Console.WriteLine("lvl mairie : " + niveauMairie);
                argentParClick += achatsMax;
                argent -= achatsMax * prixMairie;
                prixMairie = prixMairie * Math.Pow(1.25, achatsMax);
                lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
                buttonAchatMairie.Content = "Ammelioration " + prixMairie.ToString("C", CultureInfo.CurrentCulture);
                labNiveauMairie.Content = "Niveau " + niveauMairie.ToString();
            }
        }

        private void button_Click_Carriere(object sender, RoutedEventArgs e)
        {
            pierre += pierreParClick;
            lab_pierre.Content = pierre.ToString();

            Point position = Mouse.GetPosition(canvasAnimation);
            AfficherTexte(position, "+" + pierreParClick);
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
                pierreParSeconde = niveauCarriere / 10;
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
                pierreParSeconde = niveauCarriere / 10;
                lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
                buttonAchatCarriere.Content = "Amélioration " + prixCarriere.ToString("C", CultureInfo.CurrentCulture);
                labNiveauCarriere.Content = "Niveau " + niveauCarriere.ToString();

                if (niveauCarriere >= 10)
                {
                    minuteur.Start();
                }
            }
        }

        private void button_Click_Vente_Pierre(object sender, RoutedEventArgs e)
        {
            double montantVente = pierre * 0.1;
            argent += montantVente;
            pierre = 0;
            lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
            lab_pierre.Content = pierre.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            magasin menu_magasin = new magasin();
            menu_magasin.ShowDialog();
            if (menu_magasin.DialogResult == true)
            {

            }
        }

        private void btn_Click_Classement(object sender, RoutedEventArgs e)
        {
            Classement menu_classement = new Classement();
            menu_classement.ShowDialog();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.G && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                argent += 100000;
                lab_argent.Content = argent.ToString("C", CultureInfo.CurrentCulture);
            }
        }
    }
}