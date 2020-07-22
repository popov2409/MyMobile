using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace MyMobile
{
    public class DbProxy
    {
        public static List<Avtomat> Avtomats;
        public static List<Ingredient> Ingridients;
        public static void SaveData(Record record)
        {

            var fileName = $"{record.Date.Replace('/', '_')}_{Avtomats.First(c => c.Id == record.Avtomat).Value.Replace(' ', '_')}.xml";
            XmlSerializer serializer=new XmlSerializer(typeof(Record));
            Stream writer = new FileStream(fileName, FileMode.Create);
            serializer.Serialize(writer, record);





        }

        public static void LoadData()
        {
            Avtomats = new List<Avtomat>();
            Ingridients = new List<Ingredient>();
            TestData();
        }


        static void TestData()
        {
            Avtomats.Add(new Avtomat() {Value = "Хлебозавод"});
            Avtomats.Add(new Avtomat() {Value = "Мираторг строитель"});
            Avtomats.Add(new Avtomat() {Value = "Площадь Василевского"});
            Avtomats.Add(new Avtomat() {Value = "Дом профсоюзов"});
            Avtomats.Add(new Avtomat() {Value = "Бау Мосиев"});
            Avtomats.Add(new Avtomat() {Value = "БМК"});
            Avtomats.Add(new Avtomat() {Value = "Мираторг лев"});

            Ingridients.Add(new Ingredient(){Value = "Стаканы"});
            Ingridients.Add(new Ingredient() { Value = "Палочки" });
            Ingridients.Add(new Ingredient() { Value = "Сахар" });
            Ingridients.Add(new Ingredient() { Value = "Вода" });
            Ingridients.Add(new Ingredient() { Value = "Коф.зерн" });
            Ingridients.Add(new Ingredient() { Value = "Сливки" });
            Ingridients.Add(new Ingredient() { Value = "Чай" });
        }
    }

    public class Avtomat
    {
        public Avtomat()
        {
            Id=Guid.NewGuid();
        }


        public Guid Id { get; set; }

        public string Value { get; set; }
    }

    public class Ingredient
    {
        public Ingredient()
        {
            Id=Guid.NewGuid();

        }

        public Guid Id { get; set; }

        public string Value { get; set; }

        public int Count { get; set; }
    }

    public class Record
    {
        public Record()
        {
            Id=Guid.NewGuid();
        }
        private Guid Id { get; set; }
        /// <summary>
        /// Какой автомат
        /// </summary>
        public Guid Avtomat { get; set; }

        /// <summary>
        /// Какой ингредиент
        /// </summary>
        public Dictionary<Guid, int> Ingredients;

        /// <summary>
        /// Дата установки
        /// </summary>
        public string Date { get; set; }
    }

}
