using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Xml.Serialization;

namespace MyMobile
{
    public class Reporter
    {

        public List<string> GetReport(DateTime startDate, DateTime endDate)
        {
            List<string> result=new List<string>();
            IEnumerable<Ingredient> res = App.Database.GetIngridients();
            foreach (Ingredient ingredient in res)
            {
                ingredient.Count= App.Database.GetRecords()
                    .Where(c =>c.IngredientId==ingredient.Id&& DateTime.Parse(c.Date) >= startDate && DateTime.Parse(c.Date) <= endDate).Sum(c=>c.IngredientCount);
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

            //SaveTextAsync(text);

            var message = new EmailMessage
            {
                Subject = "",
                Body = "",
                To = {""}
            };

            
            var file = Path.Combine(FileSystem.CacheDirectory, $"report_{DateTime.Now.ToShortDateString()}.txt");
            File.WriteAllText(file, text);

            message.Attachments.Add(new EmailAttachment(file));

            Email.ComposeAsync(message);

        }

        public void SendXmlReport(DateTime startDate, DateTime endDate)
        {
            List<Record> records = App.Database.GetRecords()
                .Where(c => DateTime.Parse(c.Date) >= startDate && DateTime.Parse(c.Date) <= endDate).ToList();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Record>));
            var file = Path.Combine(FileSystem.CacheDirectory, $"report_{DateTime.Now.ToShortDateString()}.txt");
            using (Stream stream=File.Create(file))
            {
                var message = new EmailMessage
                {
                    Subject = "",
                    Body = "",
                    To = { "" }
                };
                
                serializer.Serialize(stream,records);
                message.Attachments.Add(new EmailAttachment(file));
                Email.ComposeAsync(message);
            }
        }
    }
}
