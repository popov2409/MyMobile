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

        public int DeleteItem(object item)
        {
            switch (item)
            {
                case Avtomat avtomat:
                    return database.Delete<Avtomat>(avtomat.Id);
                case Ingredient ingredient:
                    return database.Delete<Ingredient>(ingredient.Id);
                case Record record:
                    return database.Delete<Record>(record.Id);
                default:
                    return 0;
            }
        } 

        public Avtomat GetAvtomat(int id)
        {
            return database.Get<Avtomat>(id);
        }

        public Ingredient GetIngredient(int id)
        {
            return database.Get<Ingredient>(id);
        }

        public IEnumerable<Ingredient> GetIngridients()
        {
            return database.Table<Ingredient>().ToList();
        }

        public IEnumerable<Record> GetRecords()
        {
            return database.Table<Record>().ToList();
        }

        /// <summary>
        /// Сохранение данных
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int SaveItem(Object item)
        {
            switch (item)
            {
                case Avtomat avtomat when avtomat.Id != 0:
                    database.Update(avtomat);
                    return avtomat.Id;
                case Avtomat avtomat:
                    return database.Insert(avtomat);
                case Ingredient ingredient when ingredient.Id != 0:
                    database.Update(ingredient);
                    return ingredient.Id;
                case Ingredient ingredient:
                    return database.Insert(ingredient);
                case Record record when record.Id != 0:
                    database.Update(record);
                    return record.Id;
                case Record record:
                    return database.Insert(record);
                default:
                    return 0;
            }
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

        public int Count { get; set; }
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
