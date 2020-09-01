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

        private static List<string> GetReport(DateTime startDate, DateTime endDate)
        {
            List<string> result=new List<string>();
            IEnumerable<Ingredient> res = App.Database.GetIngridients();
            List<Record> records = App.Database.GetRecords()
                .Where(c => DateTime.Parse(c.Date) >= startDate && DateTime.Parse(c.Date) <= endDate).ToList();
            foreach (Record record in records)
            {
                result.Add(
                    $"{record.Id}:{record.Date}:{record.AvtomatId}:{record.IngredientId}:{record.IngredientCount}");
                record.IsSend = true;
                App.Database.UpdateItem(record);
            }
            return result;
        }

        public static void SendReport(DateTime startDate, DateTime endDate)
        {
            List<string> report = GetReport(startDate, endDate);
            

            //SaveTextAsync(text);

            var message = new EmailMessage
            {
                Subject = "",
                Body = "",
                To = {""}
            };

            string result = $"{App.Database.GetUserInfo().Id}#";
            var file = Path.Combine(FileSystem.CacheDirectory, $"report_{DateTime.Now.ToShortDateString().Replace('/','_')}.txt");
            foreach (string s in report)
            {
                result += s + ";";
            }

            File.WriteAllText(file, result);

            message.Attachments.Add(new EmailAttachment(file));

            Email.ComposeAsync(message);

        }
    }
}
