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
        private static readonly HttpClient client = new HttpClient();
        private const string BaseUrl = "http://localhost:3000";

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

        private async void Button_AddUser_Click(object sender, RoutedEventArgs e)
        {
            string Nom = txtNom.Text;
            if (int.TryParse(txtTemps.Text, out int Temps))
            {
                var user = new { Nom, Temps };
                await AddOrUpdateUser(user);
            }
            else
            {
                MessageBox.Show("Veuillez entrer un score valide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<List<Joueur>> GetUsers()
        {
            string url = $"{BaseUrl}/users";
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
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

        private async Task AddOrUpdateUser(object user)
        {
            string url = $"{BaseUrl}/user";
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Utilisateur ajouté ou mis à jour avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Erreur : {response.StatusCode}\n{await response.Content.ReadAsStringAsync()}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
