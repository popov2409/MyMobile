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
        }


        private void AvtomatSearchEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            AvtomatListView.ItemsSource = App.Database.GetAvtomats().Where(c=>c.Value.ToLower().Contains(AvtomatSearchEntry.Text.ToLower())).OrderBy(c => c.Value);
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

        async void ImportData()
        {
            string res= ReadData();

            if (!res.Any())
            {
                DisplayAlert("Нет файла с данными!", "Скопируйте файл LIST в память телефона и повторите операцию!","Ok");
                return;
            }

            string[] data = res.Split('#');
            string avtomats = data[0];
            string ingredients = data[1];
            App.Database.ClearData();
            foreach (string s in avtomats.Split(';'))
            {
                Avtomat a=new Avtomat()
                {
                    Id=Guid.Parse(s.Split(':')[0]),
                    Value = s.Split(':')[1]
                };
                App.Database.SaveItem(a);
            }

            foreach (string s in ingredients.Split(';'))
            {
                Ingredient i=new Ingredient()
                {
                    Id = Guid.Parse(s.Split(':')[0]),
                    Value = s.Split(':')[1],
                    Count = 0
                };
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
            string result="";
            try
            {
                using (var reader = new StreamReader("/storage/sdcard0/LIST.txt", true))
                {
                    result = reader.ReadLine();
                }
            }
            catch
            {
                // ignored
            }

            return result;
        }

        private void MenuButton_OnClicked(object sender, EventArgs e)
        {
            ImportData();
        }
    }
}
