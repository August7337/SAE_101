using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
    /// Logique d'interaction pour classement.xaml
    /// </summary>
    public partial class Classement : Window
    {
        private static readonly HttpClient CLIENT = new HttpClient();
        private static readonly string BASE_URL = "http://http://server.test";
        public static Joueur joueurActuel;

        public Classement()
        {
            InitializeComponent();
        }

        public class Joueur
        {
            public required string Nom { get; set; }
            public required int Temps { get; set; }
        }

        private void btn_Click_Ok(object sender, RoutedEventArgs e)
        {
            string nom = txtNom.Text;
            if (!string.IsNullOrEmpty(nom))
            {
                joueurActuel = new Joueur { Nom = nom, Temps = 0 };
            }
            this.DialogResult = true;
        }

        private async void Button_GetUsers_Click(object sender, RoutedEventArgs e)
        {
            var users = await GetUsers();
            lstUsers.Items.Clear();

            foreach (var user in users)
            {
                lstUsers.Items.Add($"{user.Nom} - {user.Temps}");
            }
        }

        private async Task<List<Joueur>> GetUsers()
        {
            string url = $"{BASE_URL}/users";
            try
            {
                HttpResponseMessage response = await CLIENT.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<Joueur>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    MessageBox.Show($"Erreur : {response.StatusCode}\n{await response.Content.ReadAsStringAsync()}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return new List<Joueur>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération des utilisateurs : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Joueur>();
            }
        }
    }
}
