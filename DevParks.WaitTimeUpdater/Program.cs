using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Http;
using Newtonsoft.Json.Linq;

namespace DevParks.WaitTimeUpdater
{
    class Program
    {
        private static GraphQLHttpClient _graphQLClient = new GraphQLHttpClient(o =>
            {
            o.EndPoint = new Uri("http://0a1c6d2c.ngrok.io/v1/graphql");
            o.JsonSerializer = new GraphQL.Client.Serializer.Newtonsoft.NewtonsoftJsonSerializer();
        });  
    

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

                Thread.Sleep(1000);
            }
        }

        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public static async Task<JArray> GetAllParks()
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

            var response = await _graphQLClient.SendQueryAsync<JObject>(allParksRequest);
            return response.Data["rides"] as JArray;
        }

        public static async Task UpdateWaitTime(string rideId, string waitTime)
        {
            try
            {
                var updateRequest = new GraphQLRequest
                {
                    Query = @"mutation updateWaitTime($rideId: String, $waitTime: String) {
                            update_rides(where: {id:{_eq: $rideId}}, _set: {waitTime:$waitTime}) {
                                returning {
                                    name
                                    waitTime
                                }
                            }
                        }",

                    Variables = new
                    {
                        rideId = rideId,
                        waitTime = waitTime
                    }
                };


                await _graphQLClient.SendMutationAsync<dynamic>(updateRequest);
            }catch(Exception ex)
            {
                Thread.Sleep(2000);
            }
        }
    }
}
