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
            IngredientListView.ItemsSource = DbProxy.Ingridients.OrderBy(c=>c.Value);
            AvtomatListView.ItemsSource = DbProxy.Avtomats.OrderBy(c => c.Value);
        }


        private void AvtomatButton_OnClicked(object sender, EventArgs e)
        {
            AvtomatGrid.IsVisible = true;
            IngredienGrid.IsVisible = false;
            AvtomatButton.BackgroundColor = Color.DarkOrange;
            IngredientButton.BackgroundColor = Color.Black;
            AvtomatButton.FontSize = 18;
            IngredientButton.FontSize = 16;
        }

        private void IngredientButton_OnClicked(object sender, EventArgs e)
        {
            AvtomatGrid.IsVisible = false;
            IngredienGrid.IsVisible = true;

            AvtomatButton.BackgroundColor = Color.Black;
            IngredientButton.BackgroundColor = Color.DarkOrange;

            AvtomatButton.FontSize = 16;
            IngredientButton.FontSize = 18;
        }
    }
}
