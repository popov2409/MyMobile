using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace MyMobile
{
    public class DbProxy
    {
        public static List<Avtomat> Avtomats;
        public static List<Ingredient> Ingridients;
        public static void SaveData()
        {
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
    }

    public class Record
    {
        /// <summary>
        /// Какой автомат
        /// </summary>
        public Guid Avtomat { get; set; }
        /// <summary>
        /// Какой ингредиент
        /// </summary>
        public Guid Ingredient { get; set; }
        /// <summary>
        /// Значение
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Дата установки
        /// </summary>
        public string Date { get; set; }
    }

}
