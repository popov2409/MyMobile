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
            IngredientListView.ItemsSource = DbProxy.Ingridients;
            AvtomatListView.ItemsSource = DbProxy.Avtomats;
        }


        private void AvtomatButton_OnClicked(object sender, EventArgs e)
        {
            AvtomatListView.IsVisible = true;
            IngredientListView.IsVisible = false;
        }

        private void IngredientButton_OnClicked(object sender, EventArgs e)
        {
            AvtomatListView.IsVisible = false;
            IngredientListView.IsVisible = true;
        }
    }
}
