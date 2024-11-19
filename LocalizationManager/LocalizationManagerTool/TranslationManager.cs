using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalizationManagerTool
{
    public class TranslationManager
    {
        private static TranslationManager _instance;

        // Singleton : accès unique à l'instance
        public static TranslationManager Instance => _instance ??= new TranslationManager();

        // Liste de traductions
        public List<Translation> Translations { get; set; } = new List<Translation>();

        // Constructeur privé pour empêcher l'instanciation directe
        private TranslationManager() { }
    }
}

