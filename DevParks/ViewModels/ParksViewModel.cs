using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using DevParks.Models;
using DevParks.Views;
using GraphQL.Common.Request;
using DevParks.Services;

namespace DevParks.ViewModels
{
    public class ParksViewModel : BaseViewModel
    {
        private readonly ParkService _parkService;

        public ObservableCollection<Park> Parks { get; set; }

        public ParksViewModel()
        {
            _parkService = DependencyService.Resolve<ParkService>();

            Title = "Parks";
            Parks = new ObservableCollection<Park>();
        }

        public async Task LoadData()
        {
            var parks = await _parkService.GetAllParks();

            Parks.Clear();
            foreach (var park in parks) Parks.Add(park);
        }
    }
}
