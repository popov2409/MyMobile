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
            List<string> Items = DbProxy.Ingridients.Select(c => c.Value).ToList();

            Button avtomatButton = new Button() { Text = "Автоматы" };
            Grid.SetColumn(avtomatButton,0);

            Button ingredientButton=new Button(){Text = "Ингредиенты"};
            Grid.SetColumn(ingredientButton,1);

            ListView ingridienView = new ListView() {ItemsSource = Items};
            Grid.SetRow(ingridienView,0);

            Grid buttonGrid = new Grid()
            {
                ColumnDefinitions = new ColumnDefinitionCollection() {new ColumnDefinition(), new ColumnDefinition()},
                Children = {avtomatButton, ingredientButton}
            };

            Grid.SetRow(buttonGrid,1);

            Grid mainGrid=new Grid(){RowDefinitions = new RowDefinitionCollection(){new RowDefinition(),new RowDefinition(){Height = GridLength.Auto}},Children = {ingridienView,buttonGrid }};

            this.Content = new StackLayout() {Children = {mainGrid}};
        }
    }
}
