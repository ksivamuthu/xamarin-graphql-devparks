using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DevParks.Models;
using DevParks.Services;
using Xamarin.Forms;

namespace DevParks.ViewModels
{
    public class RidesViewModel : BaseViewModel
    {
        public ObservableCollection<RideViewModel> Rides { get; set; } = new ObservableCollection<RideViewModel>();

        private ParkService _parkService;
        private Park _park;

        private GraphQL.Client.IGraphQLSubscriptionResult _result;

        public RidesViewModel(Park park)
        {
            _parkService = DependencyService.Resolve<ParkService>();
            _park = park;
            Title = park.Name;
        }

        public async Task LoadData()
        {
            _result = await _parkService.Subscribe();
            _result.OnReceive += Result_OnReceive;

            Rides.Clear();
            var parkDetails = await _parkService.GetPark(_park.Id);
            foreach (var ride in parkDetails.Rides)
                Rides.Add(new RideViewModel()
                        { Id = ride.Id, Name = ride.Name, WaitTime = ride.WaitTime, Logo = ride.Logo });
        }

        private void Result_OnReceive(GraphQL.Common.Response.GraphQLResponse obj)
        {
            var data = obj.GetDataFieldAs<Ride>("waitingTimeUpdated");  
            Console.WriteLine(data);

            var ride = Rides.FirstOrDefault(x => x.Id == data.Id);
            if(ride != null)
            {
                ride.WaitTime = data.WaitTime;
            }
        }


        public void Unregister()
        {
            if (_result != null)
            {
                _result.OnReceive -= Result_OnReceive;
            }
        }
    }
}
