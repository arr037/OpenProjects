using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Newtonsoft.Json;
using OpenProjects.Crypt;
using OpenProjects.Model;

namespace OpenProjects.Vadidation
{
    public class ItemRule : ValidationRule
    {
        private readonly ObservableCollection<Item> Items;

        public ItemRule()
        {
            Items = File.Exists("Data/Items.json")
                ? JsonConvert.DeserializeObject<ObservableCollection<Item>>(
                    Crypter.Decrypt(File.ReadAllText("Data/Items.json"), true))
                : new ObservableCollection<Item>();
        }

        private string CheckOldText=null;
        public string IsCheck { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var val = value as string;
            var check = CheckData(val);

            if (check) return new ValidationResult(false, "Такой элемент уже существует");

            return new ValidationResult(true, null);
        }

        private bool CheckData(string text)
        {
            if (IsCheck!="no" && string.IsNullOrEmpty(CheckOldText) || CheckOldText == text)
            {
                CheckOldText = text;
                return false;
            }

            if (Items.Any(p => p.Title == text)) return true;

            return false;
        }
    }
}