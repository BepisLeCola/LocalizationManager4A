using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace LocalizationManagerTool
{
    public partial class MainWindow : Window
    {
        // Déclaration des propriétés
        public ObservableCollection<Translation> Translations { get; set; }
        public List<string> Columns { get; set; }

        // Constructeur combiné
        public MainWindow()
        {
            InitializeComponent();

            // Initialisation de la collection Translations
            Translations = new ObservableCollection<Translation>();
            dataGrid.ItemsSource = Translations;

            Columns = new List<string> { "ID", "French", "English", "Japanese" };
        }

        // Exemple de méthode pour importer un fichier CSV
        private void ImportCSV_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                string[] lines = File.ReadAllLines(openFileDialog.FileName);
                Translations.Clear();  // On vide les anciennes traductions
                foreach (var line in lines.Skip(1)) // Ignorer l'en-tête
                {
                    var columns = line.Split(',');

                    try
                    {
                        // Parsing de chaque colonne et ajout d'une nouvelle traduction
                        int id = int.Parse(columns[0]);
                        string lang1 = columns[1];
                        string lang2 = columns[2];
                        string lang3 = columns.Length > 3 ? columns[3] : ""; // Lecture de "Japanese" si elle existe

                        Translations.Add(new Translation { ID = id, French = lang1, English = lang2, Japanese = lang3 });
                    }
                    catch (Exception ex)
                    {
                        // Affichage d'un message d'erreur en cas de problème avec une ligne spécifique
                        MessageBox.Show($"Erreur d'import : {ex.Message}");
                    }
                }

                // Appeler UpdateColumnsFromData après avoir ajouté les traductions
                UpdateColumnsFromData(Translations);
            }
        }



        // Méthode pour exporter en JSON
        private void ExportJSON_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON Files (*.json)|*.json";
            if (saveFileDialog.ShowDialog() == true)
            {
                // Sérialisation avec System.Text.Json
                var json = JsonSerializer.Serialize(Translations, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(saveFileDialog.FileName, json);
            }
        }

        // Importer un fichier JSON
        private void ImportJSON_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON Files (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                string json;
                try
                {
                    // Lire le fichier en utilisant StreamReader avec Encoding.UTF8 sans BOM
                    using (var reader = new StreamReader(openFileDialog.FileName, new UTF8Encoding(false)))
                    {
                        json = reader.ReadToEnd();
                    }

                    // Désérialiser le JSON en une liste d'objets Translation
                    var translations = JsonSerializer.Deserialize<List<Translation>>(json);

                    // Vider les traductions existantes dans la collection
                    Translations.Clear();

                    // Ajouter les traductions désérialisées à la collection ObservableCollection
                    foreach (var translation in translations)
                    {
                        Translations.Add(translation);
                    }

                    // Appeler UpdateColumnsFromData après avoir ajouté les traductions
                    UpdateColumnsFromData(Translations);
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"Erreur lors de la désérialisation du JSON : {ex.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur générale : {ex.Message}");
                }
            }
        }


        // Méthode pour importer un fichier XML
        private void ImportXML_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files (*.xml)|*.xml";
            if (openFileDialog.ShowDialog() == true)
            {
                XDocument doc = XDocument.Load(openFileDialog.FileName);
                var translations = new List<Translation>();

                foreach (var element in doc.Descendants("translation"))
                {
                    translations.Add(new Translation
                    {
                        ID = int.Parse(element.Attribute("id").Value),
                        French = element.Element("french").Value,
                        English = element.Element("english").Value,
                        Japanese = element.Element("japanese")?.Value // Gérer le cas où "japanese" est absent
                    });
                }

                Translations.Clear();
                foreach (var translation in translations)
                {
                    Translations.Add(translation);
                }

                // Appeler UpdateColumnsFromData après avoir ajouté les traductions
                UpdateColumnsFromData(Translations);
            }
        }


        // Méthode pour mettre à jour les colonnes du DataGrid si nécessaire
        private void UpdateColumnsFromData(IEnumerable<Translation> translations)
        {
            dataGrid.Columns.Clear(); // Efface les colonnes existantes

            if (translations.Any())
            {
                var properties = translations.First().GetType().GetProperties();
                foreach (var prop in properties)
                {
                    dataGrid.Columns.Add(new DataGridTextColumn
                    {
                        Header = prop.Name,
                        Binding = new Binding(prop.Name)
                    });
                }
            }
        }

        // Ouvrir ou fermer le Popup pour le menu "File"
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (popupFile.IsOpen)
            {
                popupFile.IsOpen = false;
            }
            else
            {
                popupFile.IsOpen = true;
            }
        }

        // Ouvrir ou fermer le Popup pour le menu "Edit"
        private void Button_Edit(object sender, RoutedEventArgs e)
        {
            if (popupEdit.IsOpen)
            {
                popupEdit.IsOpen = false;
            }
            else
            {
                popupEdit.IsOpen = true;
            }
        }
    }

    // Classe Translation représentant chaque entrée de traduction
    public class Translation
    {
        public int ID { get; set; }
        public string French { get; set; }
        public string English { get; set; }
        public string Japanese { get; set; } // Nouvelle propriété
    }

}
