using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using SQLite;

namespace MyMobile
{
    public class DbProxy
    {
        SQLiteConnection database;

        public DbProxy(string databasePath)
        {
            database = new SQLiteConnection(databasePath);
            database.CreateTable<Avtomat>();
            database.CreateTable<Ingredient>();
            database.CreateTable<Record>();
        }

        public IEnumerable<Avtomat> GetAvtomats()
        {
            return database.Table<Avtomat>().ToList();
        }

        public IEnumerable<Ingredient> GetIngridients()
        {
            return database.Table<Ingredient>().ToList();
        }

        public IEnumerable<Record> GetRecords()
        {
            return database.Table<Record>().ToList();
        }

        //static void TestData()
        //{
        //    Avtomats.Add(new Avtomat() {Value = "Хлебозавод"});
        //    Avtomats.Add(new Avtomat() {Value = "Мираторг строитель"});
        //    Avtomats.Add(new Avtomat() {Value = "Площадь Василевского"});
        //    Avtomats.Add(new Avtomat() {Value = "Дом профсоюзов"});
        //    Avtomats.Add(new Avtomat() {Value = "Бау Мосиев"});
        //    Avtomats.Add(new Avtomat() {Value = "БМК"});
        //    Avtomats.Add(new Avtomat() {Value = "Мираторг лев"});

        //    Ingridients.Add(new Ingredient(){Value = "Стаканы"});
        //    Ingridients.Add(new Ingredient() { Value = "Палочки" });
        //    Ingridients.Add(new Ingredient() { Value = "Сахар" });
        //    Ingridients.Add(new Ingredient() { Value = "Вода" });
        //    Ingridients.Add(new Ingredient() { Value = "Коф.зерн" });
        //    Ingridients.Add(new Ingredient() { Value = "Сливки" });
        //    Ingridients.Add(new Ingredient() { Value = "Чай" });
        //}
    }

    
    [Table("Avtomats")]
    public class Avtomat
    {
        [PrimaryKey,AutoIncrement,Column("_id")]
        public int Id { get; set; }

        public string Value { get; set; }
    }

    [Table("Ingredients")]
    public class Ingredient
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public string Value { get; set; }

    }

    [Table("Record")]
    public class Record
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        /// <summary>
        /// Какой автомат
        /// </summary>
        public int AvtomatId { get; set; }

        /// <summary>
        /// Какой ингредиент
        /// </summary>
        public int IngredientId { get; set; }

        /// <summary>
        /// Дата установки
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Колличество ингредиентов
        /// </summary>
        public int IngredientCount { get; set; }
    }

}
