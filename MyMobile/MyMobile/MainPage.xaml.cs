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
            if (AvtomatSearchEntry.Text.Equals("#userinfo") && App.Database.GetAvtomats().Any())
            {
                AvtomatSearchEntry.Text = "";
                DisplayAlert("Информация об операторе", App.Database.GetUserInfo().Name, "Ok");
            }

        }


        private void VisualElement_OnUnfocused(object sender, FocusEventArgs e)
        {
            if ((sender as Entry).Text.Trim().Length == 0) (sender as Entry).Text = "0";
        }

        private async void SaveButton_OnClicked(object sender, EventArgs e)
        {

            bool result = await DisplayAlert("Сохранение", "Сохранить данные?", "Да", "Нет");
            if(!result) return;

            var recor = App.Database.GetRecords();

            if (recor.Any()&& DateTime.Now < recor.Max(c => DateTime.Parse(c.Date)))
            {
                await DisplayAlert("Ошибка", "Неверная дата!", "Ok");
                return;
            }

            foreach (Ingredient ingredient in IngredientListView.ItemsSource)
            {
                if(ingredient.Count==0) continue;
                Record rec = new Record
                {
                    Id=Guid.NewGuid(),
                    Date = DateTime.Now.ToShortDateString(),
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
        private UserInfo UserTest;


        void CreateTestData()
        {
            UserTest =new UserInfo()
            {
                Id = Guid.NewGuid(),
                Name = "Иван",
                RoleName = 7,
                Password = ""
            };
            App.Database.SaveItem(UserTest);


            foreach (string testAvtomat in TestAvtomats)
            {
                App.Database.SaveItem(new Avtomat()
                {
                    Id = Guid.NewGuid(),
                    Value = testAvtomat
                });
            }

            foreach (string testAvtomat in TestIngredients)
            {
                App.Database.SaveItem(new Ingredient()
                {
                    Id = Guid.NewGuid(),
                    Value = testAvtomat
                });
            }
        }

        void ImportData()
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
            string[] userInfo = data[2].Split(':');
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
                App.Database.SaveItem(i);
            }


            UserInfo user = new UserInfo()
            {
                Id=Guid.Parse(userInfo[0]),
                Name = userInfo[1],
                Password = userInfo[2],
                RoleName = int.Parse(userInfo[3])
            };

            InitializePage();

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
            MenuGrid.IsVisible = !MenuGrid.IsVisible;
        }


        private void ImportDataButton_OnClicked(object sender, EventArgs e)
        {
            ImportData();
            MenuGrid.IsVisible = false;
        }

        private void ReportViewButton_OnClicked(object sender, EventArgs e)
        {
            MenuGrid.IsVisible = false;
            List<Record> records = App.Database.GetRecords().ToList();
            if (records.Count == 0)
            {
                DisplayAlert("Уведомление", "Нет данных для отправки!", "OK");
                return;
            }

            if (records.Count(c => !c.IsSend) == 0)
            {
                DisplayAlert("Уведомление", "Все данные в базе были отправлены оператору!", "OK");

            }
            else
            {
                StartDatePicker.Date = records.Where(c => !c.IsSend).Min(c => DateTime.Parse(c.Date));
                EndDatePicker.Date = records.Where(c => !c.IsSend).Max(c => DateTime.Parse(c.Date));
            }
            
            ReportGrid.IsVisible = true;
            
        }

        private void SendReportButton_OnClicked(object sender, EventArgs e)
        {
            Reporter.SendReport(StartDatePicker.Date,EndDatePicker.Date);
            ReportGrid.IsVisible = false;
        }

        private void CancelReportButton_OnClicked(object sender, EventArgs e)
        {
            ReportGrid.IsVisible = false;
        }
    }
}
