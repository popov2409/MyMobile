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

        public Avtomat GetAvtomat(Guid id)
        {
            return database.Get<Avtomat>(id);
        }

        public Ingredient GetIngredient(Guid id)
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

        public void ClearData()
        {
            foreach (Avtomat avtomat in GetAvtomats())
            {
                database.Delete<Avtomat>(avtomat.Id);
            }

            foreach (Record record in GetRecords())
            {
                database.Delete<Record>(record.Id);
            }

            foreach (Ingredient ingridient in GetIngridients())
            {
                database.Delete<Ingredient>(ingridient.Id);
            }
        }

        /// <summary>
        /// Сохранение данных
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public void SaveItem(Object item)
        {
            database.Insert(item);
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
        public Guid Id { get; set; }

        public string Value { get; set; }
    }

    [Table("Ingredients")]
    public class Ingredient
    {
        public Guid Id { get; set; }

        public string Value { get; set; }

        public int Count { get; set; }
    }

    [Table("Record")]
    public class Record
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Какой автомат
        /// </summary>
        public Guid AvtomatId { get; set; }

        /// <summary>
        /// Какой ингредиент
        /// </summary>
        public Guid IngredientId { get; set; }

        /// <summary>
        /// Дата установки
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Колличество ингредиентов
        /// </summary>
        public int IngredientCount { get; set; }

        ///// <summary>
        ///// Отправлено в отчет
        ///// </summary>
        public bool IsSend { get; set; }
    }

}
