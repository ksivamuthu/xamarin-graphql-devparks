using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using DevParks.Models;
using DevParks.ViewModels;

namespace DevParks.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class RidesPage : ContentPage
    {
        RidesViewModel viewModel;

        public RidesPage(RidesViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public RidesPage()
        {
            InitializeComponent();         
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.LoadData().ConfigureAwait(false);

            //if (viewModel.Items.Count == 0)
            //viewModel.LoadItemsCommand.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewModel.Unregister();
        }
    }
}