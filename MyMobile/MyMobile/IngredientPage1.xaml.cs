using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IngredientPage1 : ContentPage
    {
        public List<string> Items { get; set; }

        public IngredientPage1()
        {
            InitializeComponent();

            Items = DbProxy.Ingridients.Select(c => c.Value).ToList();

            MyListView.ItemsSource = Items;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
