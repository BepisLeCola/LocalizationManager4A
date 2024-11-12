using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using System.Xml.Linq;
using System.Xml;

namespace LocalizationManagerTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> Columns = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            Columns.Add("id");
            Columns.Add("en");
            Columns.Add("fr");
            Columns.Add("es");
            Columns.Add("ja");

            foreach (string column in Columns)
            {
                //Pour ajouter une colonne à notre datagrid
                DataGridTextColumn textColumn = new DataGridTextColumn();
                textColumn.Header = column;
                textColumn.Binding = new Binding(column);
                dataGrid.Columns.Add(textColumn);
            }
        }

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
                        Language1 = element.Element("language1").Value,
                        Language2 = element.Element("language2").Value
                    });
                }

                Translations.Clear();
                foreach (var translation in translations)
                {
                    Translations.Add(translation);
                }
            }
        }



    }


}