using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Diagnostics;

namespace LocalizationManagerTool
{
    public class TranslationEntry
    {
        public int ID { get; set; }
        public string Language1 { get; set; }
        public string Language2 { get; set; }
        // Ajoutez d'autres propriétés pour les langues supplémentaires si nécessaire
    }

    public class CsvImporter
    {
        public string filePath = "LocalizationManagerTool/";

        public List<TranslationEntry> ImportFromCsv()
        {
            var entries = new List<TranslationEntry>();

            // Lire le fichier CSV
            using (var reader = new StreamReader(filePath))
            {
                string headerLine = reader.ReadLine();
                if (headerLine == null)
                {
                    Debug.Write("Y'a rieng");
                    return entries; // Retourner si le fichier est vide

                }

                // Lire les lignes suivantes
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue; // Ignorer les lignes vides

                    var values = line.Split(','); // Séparer par des virgules

                    // Créer un nouvel objet TranslationEntry et le remplir
                    var entry = new TranslationEntry
                    {
                        ID = int.Parse(values[0].Trim(), CultureInfo.InvariantCulture),
                        Language1 = values.Length > 1 ? values[1].Trim() : string.Empty,
                        Language2 = values.Length > 2 ? values[2].Trim() : string.Empty
                        // Ajoutez d'autres langues selon vos besoins
                    };

                    entries.Add(entry);
                }
            }

            return entries;
        }
    }
}