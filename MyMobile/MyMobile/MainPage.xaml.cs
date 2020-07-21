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
            StartPages();
           
        }


        void StartPages()
        {
            AvtomatGrid.IsVisible = true;
            InputDataGrid.IsVisible = false;
        }

        void InitializePage()
        {
            IngredientListView.ItemsSource = DbProxy.Ingridients.OrderBy(c => c.Value);
            AvtomatListView.ItemsSource = DbProxy.Avtomats.OrderBy(c => c.Value);
        }


        //private void AvtomatButton_OnClicked(object sender, EventArgs e)
        //{
        //    AvtomatGrid.IsVisible = true;
        //    IngredienGrid.IsVisible = false;
        //    AvtomatButton.BackgroundColor = Color.DarkOrange;
        //    IngredientButton.BackgroundColor = Color.Black;
        //    AvtomatButton.FontSize = 18;
        //    IngredientButton.FontSize = 16;
        //}

        //private void IngredientButton_OnClicked(object sender, EventArgs e)
        //{
        //    AvtomatGrid.IsVisible = false;
        //    IngredienGrid.IsVisible = true;

        //    AvtomatButton.BackgroundColor = Color.Black;
        //    IngredientButton.BackgroundColor = Color.DarkOrange;

        //    AvtomatButton.FontSize = 16;
        //    IngredientButton.FontSize = 18;
        //}

        private void AvtomatSearchEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            AvtomatListView.ItemsSource = DbProxy.Avtomats.Where(c=>c.Value.ToLower().Contains(AvtomatSearchEntry.Text.ToLower())).OrderBy(c => c.Value);
        }

        private void InputDataButton_OnClicked(object sender, EventArgs e)
        {
            if (AvtomatListView.SelectedItem == null)
            {
                DisplayAlert("Уведомление", "Не выбран автомат!", "OK");
                return;
            }

            AvtomatGrid.IsVisible = false;
            InputDataGrid.IsVisible = true;
            Avtomat a=AvtomatListView.SelectedItem as Avtomat;
            AvtomatNameLabel.Text = a.Value;
        }

        private void BackButton_OnClicked(object sender, EventArgs e)
        {
            StartPages();
        }

        private void VisualElement_OnUnfocused(object sender, FocusEventArgs e)
        {
            if ((sender as Entry).Text.Trim().Length == 0) (sender as Entry).Text = "0";
        }

    }
}
