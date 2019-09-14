using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using DevParks.Backend.Model;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;

namespace DevParks.Backend.Services
{
    public class ParkService
    {
        private readonly CosmosClient _client;
        private Container _parksContainer;
        private Container _ridesContainer;
        private readonly ISubject<Ride> _waitTimeStream = new ReplaySubject<Ride>(1);

        public ParkService(IOptions<AppSettings> appSettings)
        {
            this._client = new CosmosClient(appSettings.Value.CosmosConnectionString);
            Database database = _client.CreateDatabaseIfNotExistsAsync("DevParks").Result;
            _parksContainer = database.CreateContainerIfNotExistsAsync("Parks", "/id", 400).Result;
            _ridesContainer = database.CreateContainerIfNotExistsAsync("Rides", "/parkId", 400).Result;
        }

        public async Task<List<Park>> GetAllParks()
        {
            var parksFeedIterator = _parksContainer.GetItemLinqQueryable<Park>().ToFeedIterator();
            var parks = new List<Park>();

            while (parksFeedIterator.HasMoreResults)
            {
                var response = await parksFeedIterator.ReadNextAsync();
                if (response != null && response.Resource != null && response.Resource.Count() > 0)
                {
                    parks.AddRange(response.Resource);
                }
            }

            return parks;
        }

        public async Task<Park> GetParkById(string parkId)
        {
            var parkFeedIterator = _parksContainer.GetItemLinqQueryable<Park>(true).Where(x => x.Id == parkId).ToFeedIterator();

            while (parkFeedIterator.HasMoreResults)
            {
                var response = await parkFeedIterator.ReadNextAsync();
                if (response != null && response.Resource != null && response.Resource.Count() > 0)
                {
                    return response.Resource.FirstOrDefault();
                }
            }

            return null;
        }

        public async Task<List<Ride>> GetRides()
        {
            var ridesFeedIterator = _ridesContainer.GetItemLinqQueryable<Ride>().ToFeedIterator();
            var rides = new List<Ride>();

            while (ridesFeedIterator.HasMoreResults)
            {
                var response = await ridesFeedIterator.ReadNextAsync();
                if (response != null && response.Resource != null && response.Resource.Count() > 0)
                {
                    rides.AddRange(response.Resource);
                }
            }

            return rides;
        }

        public async Task<Ride> GetRideById(string rideId)
        {
            var ridesFeedIterator = _ridesContainer.GetItemLinqQueryable<Ride>(true).Where(x => x.Id == rideId).ToFeedIterator();

            while (ridesFeedIterator.HasMoreResults)
            {
                var response = await ridesFeedIterator.ReadNextAsync();
                if (response != null && response.Resource != null && response.Resource.Count() > 0)
                {
                    return response.Resource.FirstOrDefault();
                }
            }

            return null;
        }

        public async Task<List<Ride>> GetRidesByParkId(string parkId)
        {
            var ridesFeedIterator = _ridesContainer.GetItemLinqQueryable<Ride>().Where(x => x.ParkId == parkId).ToFeedIterator();
            var rides = new List<Ride>();

            while (ridesFeedIterator.HasMoreResults)
            {
                var response = await ridesFeedIterator.ReadNextAsync();
                if (response != null && response.Resource != null && response.Resource.Count() > 0)
                {
                    rides.AddRange(response.Resource);
                }
            }

            return rides;
        }

        public async Task<Park> GetParkByRideId(string rideId)
        {
            var ride = await GetRideById(rideId);

            if (ride != null)
            {
                return await GetParkById(ride.ParkId);
            }

            return null;
        }

        public async Task<Ride> UpdateRideWaitTime(string rideId, string waitTime)
        {
            var ride = await GetRideById(rideId);
            if (ride != null)
            {
                ride.WaitTime = waitTime;

                var updatedRide = (await _ridesContainer.ReplaceItemAsync<Ride>(ride, ride.Id, new PartitionKey(ride.ParkId))).Resource;
                _waitTimeStream.OnNext(updatedRide);
                return updatedRide;
            }

            return null;
        }

        public IObservable<Ride> RideWaitTimes()
        {
            return _waitTimeStream.AsObservable();
        }
    }
}
