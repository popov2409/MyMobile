using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            DbProxy.LoadData();
            InitializePage();
        }

        void InitializePage()
        {
            AvtomatGrid.IsVisible = true;
            InputDataGrid.IsVisible = false;
            foreach (Ingredient ingridient in DbProxy.Ingridients)
            {
                ingridient.Count = 0;
            }
            IngredientListView.ItemsSource = DbProxy.Ingridients.OrderBy(c => c.Value);
            AvtomatListView.ItemsSource = DbProxy.Avtomats.OrderBy(c => c.Value);
            selectedAvtomat = null;
        }


        private void AvtomatSearchEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            AvtomatListView.ItemsSource = DbProxy.Avtomats.Where(c=>c.Value.ToLower().Contains(AvtomatSearchEntry.Text.ToLower())).OrderBy(c => c.Value);
        }


        private Avtomat selectedAvtomat;
        private void InputDataButton_OnClicked(object sender, EventArgs e)
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
        }

        private void BackButton_OnClicked(object sender, EventArgs e)
        {
            InitializePage();
        }

        private void VisualElement_OnUnfocused(object sender, FocusEventArgs e)
        {
            if ((sender as Entry).Text.Trim().Length == 0) (sender as Entry).Text = "0";
        }

        private Ingredient selIngedient;

        private void SaveButton_OnClicked(object sender, EventArgs e)
        {
            Record rec = new Record() {Avtomat = selectedAvtomat.Id, Date = DatePicker.Date.ToShortDateString()};
            Dictionary<Guid, int> dictionary = new Dictionary<Guid, int>();
            foreach (Ingredient ingridient in DbProxy.Ingridients)
            {
                dictionary.Add(ingridient.Id, ingridient.Count);
            }
            rec.Ingredients = dictionary;
            DbProxy.SaveData(rec);
        }
    }
}
