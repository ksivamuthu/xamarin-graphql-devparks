using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using DevParks.Models;
using DevParks.Views;
using DevParks.ViewModels;

namespace DevParks.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ParksPage : ContentPage
    {
        ParksViewModel viewModel;

        public ParksPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ParksViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Park;
            if (item == null)
                return;

            await Navigation.PushAsync(new RidesPage(new RidesViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.LoadData().ConfigureAwait(false);

            //if (viewModel.Items.Count == 0)
                //viewModel.LoadItemsCommand.Execute(null);
        }
    }
}