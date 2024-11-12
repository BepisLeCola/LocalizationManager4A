using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Transactions;
using System.Windows;

namespace LocalizationManagerTool
{

    public partial class MainWindow : Window
    {
        public ObservableCollection<Translation> Translations { get; set; }

       /* public MainWindow()
        {
            InitializeComponent();
            Translations = new ObservableCollection<Translation>();
            TranslationDataGrid.ItemsSource = Translations;
        }
       */

        // Importer CSV
        private void ImportCSV_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                string[] lines = File.ReadAllLines(openFileDialog.FileName);
                Translations.Clear();
                foreach (var line in lines.Skip(1)) // Ignorer l'en-tête
                {
                    var columns = line.Split(',');
                    Translations.Add(new Translation
                    {
                        ID = int.Parse(columns[0]),
                        Language1 = columns[1],
                        Language2 = columns[2]
                    });
                }
            }
        }

        // Exporter CSV
        private void ExportCSV_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            if (saveFileDialog.ShowDialog() == true)
            {
                var lines = new List<string> { "ID,Language1,Language2" };
                foreach (var translation in Translations)
                {
                    lines.Add($"{translation.ID},{translation.Language1},{translation.Language2}");
                }
                File.WriteAllLines(saveFileDialog.FileName, lines);
            }
        }

        // Exporter C#
        private void ExportCSharp_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "C# Files (*.cs)|*.cs";
            if (saveFileDialog.ShowDialog() == true)
            {
                var classCode = new StringBuilder();
                classCode.AppendLine("public class Translation {");
                classCode.AppendLine("    public int ID { get; set; }");
                classCode.AppendLine("    public string Language1 { get; set; }");
                classCode.AppendLine("    public string Language2 { get; set; }");
                classCode.AppendLine("    // Add more properties if needed");
                classCode.AppendLine("}");
                File.WriteAllText(saveFileDialog.FileName, classCode.ToString());
            }
        }

        // Exporter C++
        private void ExportCpp_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "C++ Files (*.h;*.cpp)|*.h;*.cpp";
            if (saveFileDialog.ShowDialog() == true)
            {
                var classCodeH = new StringBuilder();
                var classCodeCpp = new StringBuilder();

                classCodeH.AppendLine("class Translation {");
                classCodeH.AppendLine("public:");
                classCodeH.AppendLine("    int ID;");
                classCodeH.AppendLine("    std::string Language1;");
                classCodeH.AppendLine("    std::string Language2;");
                classCodeH.AppendLine("    // Add more properties if needed");
                classCodeH.AppendLine("};");

                classCodeCpp.AppendLine("#include \"Translation.h\"");
                classCodeCpp.AppendLine("// Implementations of methods if necessary");

                // Sauvegarder les fichiers .h et .cpp
                File.WriteAllText(saveFileDialog.FileName.Replace(".cpp", ".h"), classCodeH.ToString());
                File.WriteAllText(saveFileDialog.FileName, classCodeCpp.ToString());
            }
        }
    }

    // Classe Translation représentant chaque entrée de traduction
    public class Translation
    {
        public int ID { get; set; }
        public string Language1 { get; set; }
        public string Language2 { get; set; }
    }
}