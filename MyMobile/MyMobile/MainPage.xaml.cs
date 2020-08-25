using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyMobile
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            InitializePage();
            if (!App.Database.GetAvtomats().Any())
            {
                CreateTestData();
            }
            DisplayAlert("Уведомление", ReadData(), "OK");
        }

        protected override bool OnBackButtonPressed()
        {
            if (selectedAvtomat != null)
            {
                InitializePage();
                return true;
            }
            return base.OnBackButtonPressed();
        }

        

        void InitializePage()
        {
            AvtomatGrid.IsVisible = true;
            InputDataGrid.IsVisible = false;
            foreach (Ingredient ingridient in App.Database.GetIngridients())
            {
                ingridient.Count = 0;
            }
            IngredientListView.ItemsSource = App.Database.GetIngridients().OrderBy(c => c.Value);
            AvtomatListView.ItemsSource = App.Database.GetAvtomats().OrderBy(c => c.Value);
            selectedAvtomat = null;
            HeaderLabel.Text = "Автоматы";
            //BackButton.Text = "<-";
        }


        private void AvtomatSearchEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            AvtomatListView.ItemsSource = App.Database.GetAvtomats().Where(c=>c.Value.ToLower().Contains(AvtomatSearchEntry.Text.ToLower())).OrderBy(c => c.Value);
        }

        private void BackButton_OnClicked(object sender, EventArgs e)
        {
            InitializePage();
        }

        private void VisualElement_OnUnfocused(object sender, FocusEventArgs e)
        {
            if ((sender as Entry).Text.Trim().Length == 0) (sender as Entry).Text = "0";
        }

        private async void SaveButton_OnClicked(object sender, EventArgs e)
        {

            bool result = await DisplayAlert("Сохранение", "Сохранить данные?", "Да", "Нет");
            if(!result) return;

            foreach (Ingredient ingredient in IngredientListView.ItemsSource)
            {
                Record rec = new Record
                {
                    Date = InputDatePicker.Date.ToShortDateString(),
                    AvtomatId = selectedAvtomat.Id,
                    IngredientId = ingredient.Id,
                    IngredientCount = ingredient.Count,
                };
                App.Database.SaveItem(rec);
            }
            InitializePage();
        }

        private string[] TestAvtomats = { "Хлебозавод", "Мираторг строитель", "Площадь Василевского", "Дом профсоюзов", "Бау Мосиев" , "Мираторг лев" };
        private string[] TestIngredients = {"Стаканы", "Палочки", "Сахар", "Вода", "Коф.зерн", "Сливки", "Чай"};

        void CreateTestData()
        {
            foreach (string testAvtomat in TestAvtomats)
            {
                App.Database.SaveItem(new Avtomat()
                {
                    Value = testAvtomat
                });
            }

            foreach (string testAvtomat in TestIngredients)
            {
                App.Database.SaveItem(new Ingredient()
                {
                    Value = testAvtomat
                });
            }


        }

        private void ReportButton_OnClicked(object sender, EventArgs e)
        {
            App.Report.SendReport(DateTime.MinValue, DateTime.MaxValue);
        }
        private Avtomat selectedAvtomat;

        private void AvtomatListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (AvtomatListView.SelectedItem == null)
            {
                DisplayAlert("Уведомление", "Не выбран автомат!", "OK");
                return;
            }

            AvtomatGrid.IsVisible = false;
            InputDataGrid.IsVisible = true;
            selectedAvtomat = AvtomatListView.SelectedItem as Avtomat;
            AvtomatNameLabel.Text = selectedAvtomat.Value;
            HeaderLabel.Text = "Ингредиенты";

        }


        public string ReadData()
        {
            //var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "data.txt");
            //DisplayAlert("Уведомление", backingFile, "OK");
            //if (backingFile == null || !File.Exists(backingFile))
            //{
            //    DisplayAlert("Уведомление", "Не найден файл с данными", "OK");
            //    return null;
            //}

            string line="No!";
            //using (var reader = new StreamReader(backingFile, true))
            try
            {
                using (var reader = new StreamReader("/storage/sdcard0/data.txt", true))
                {
                    line = reader.ReadLine();
                }
            }
            catch
            {
                // ignored
            }

            return line;
        }
    }
}
