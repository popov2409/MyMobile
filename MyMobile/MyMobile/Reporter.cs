using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

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

        public List<string> GetReport(DateTime startDate, DateTime endDate)
        {
            List<string> result=new List<string>();
            IEnumerable<Ingredient> res = App.Database.GetIngridients();
            foreach (Ingredient ingredient in res)
            {
                ingredient.Count= App.Database.GetRecords()
                    .Where(c =>c.IngredientId==ingredient.Id&& DateTime.Parse(c.Date) >= startDate && DateTime.Parse(c.Date) <= endDate).Sum(c=>c.IngredientId);
                result.Add(ingredient.Value+"#"+ingredient.Count);
            }
            return result;
        }

        public void SendReport(DateTime startDate, DateTime endDate)
        {
            List<string> report = GetReport(startDate, endDate);
            string text = "";
            foreach (string s in report)
            {
                text += s + "$";
            }

            SaveTextAsync(text);

            var message = new EmailMessage
            {
                Subject = "popov_ta@mail.ru",
                Body = "World",
            };

            
            var file = Path.Combine(FileSystem.CacheDirectory, REPORT_FILE_NAME);
            File.WriteAllText(file, "Hello World");

            message.Attachments.Add(new EmailAttachment(file));

            Email.ComposeAsync(message);

        }
    }
}
