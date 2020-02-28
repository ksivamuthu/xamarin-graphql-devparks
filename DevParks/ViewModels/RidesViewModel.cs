using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DevParks.Models;
using DevParks.Services;
using GraphQL;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace DevParks.ViewModels
{
    public class RidesViewModel : BaseViewModel
    {
        public ObservableCollection<RideViewModel> Rides { get; set; } = new ObservableCollection<RideViewModel>();

        private ParkService _parkService;
        private Park _park;
        private IDisposable _result;


        public RidesViewModel(Park park)
        {
            _parkService = DependencyService.Resolve<ParkService>();
            _park = park;
            Title = park.Name;
        }

        public async Task LoadData()
        {
            _result = _parkService.WaitingTimeUpdatedStream(_park.Id).Subscribe(Result_OnReceive);

            Rides.Clear();
            var parkDetails = await _parkService.GetPark(_park.Id);
            foreach (var ride in parkDetails.Rides)
                Rides.Add(new RideViewModel()
                        { Id = ride.Id, Name = ride.Name, WaitTime = ride.WaitTime, Logo = ride.Logo });
        }

        private void Result_OnReceive(GraphQLResponse<JObject> obj)
        {
            var data = obj.Data["rides"];
            Console.WriteLine(data);

            foreach (var item in data)
            {
                var ride = Rides.FirstOrDefault(x => x.Id == item["id"].ToString());
                if (ride != null)
                {
                    ride.WaitTime = item["waitTime"].ToString();
                }
            }            
        }


        public void Unregister()
        {
            if (_result != null)
            {
                _result.Dispose();
            }
        }
    }
}
