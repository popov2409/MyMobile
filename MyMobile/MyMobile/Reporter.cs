using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyMobile
{
    public class Reporter
    {
        private string REPORT_FILE_NAME = "report.txt";

        public async Task SaveTextAsync(string text)
        {
            string filepath = GetFilePath();
            using (StreamWriter writer = File.CreateText(filepath))
            {
                await writer.WriteAsync(text);
            }
        }
        // вспомогательный метод для построения пути к файлу
        string GetFilePath()
        {
            return Path.Combine(GetDocsPath(), REPORT_FILE_NAME);
        }
        // получаем путь к папке MyDocuments
        string GetDocsPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public IEnumerable<Ingredient> GetReport(DateTime startDate, DateTime endDate)
        {
            IEnumerable<Ingredient> result = App.Database.GetIngridients();
            foreach (Ingredient ingredient in result)
            {
                ingredient.Count= App.Database.GetRecords()
                    .Where(c =>c.IngredientId==ingredient.Id&& DateTime.Parse(c.Date) >= startDate && DateTime.Parse(c.Date) <= endDate).Sum(c=>c.IngredientId);
            }

            return result;
        }
    }
}
