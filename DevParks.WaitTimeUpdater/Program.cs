using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Client.Http;
using GraphQL.Common.Request;
using Newtonsoft.Json.Linq;

namespace DevParks.WaitTimeUpdater
{
    class Program
    {
        private static readonly GraphQLHttpClient _graphQLClient = new GraphQLHttpClient("https://devparks.azurewebsites.net/graphql");
    

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var parks = GetAllParks().Result;
            var rideIds = parks.Select(x => x["id"].ToString());

            while(true)
            {
                var ride = rideIds.ToList()[RandomNumber(0, rideIds.Count() - 1)];

                Console.WriteLine(ride);

                UpdateWaitTime(ride, RandomNumber(1, 100).ToString() + "m").Wait();

                Thread.Sleep(500);
            }
        }

        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public static async Task<List<JObject>> GetAllParks()
        {
            var allParksRequest = new GraphQLRequest
            {
                Query = @"
	                    {
		                    rides {
                                id			                                                    
		                    }
	                    }"
            };

            var response = await _graphQLClient.SendQueryAsync(allParksRequest);
            return response.GetDataFieldAs<List<JObject>>("rides");
        }

        public static async Task UpdateWaitTime(string rideId, string waitTime)
        {
            var updateRequest = new GraphQLRequest
            {
                Query = @"mutation updateWaitTime($rideId: ID, $waitTime: String) {
                            updateWaitTime(rideId: $rideId, waitTime:$waitTime) {
                                id
                                waitTime
                            }
                        }",
                Variables = new
                {
                    rideId = rideId,
                    waitTime = waitTime
                }
            };

            var response = await _graphQLClient.SendMutationAsync(updateRequest);
        }
    }
}
