using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DevParks.Models;
using GraphQL;
using GraphQL.Client.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DevParks.Services
{
    public class ParkService
    {
        private readonly GraphQLHttpClient _graphQLClient;

        public ParkService()
        {
            _graphQLClient = new GraphQLHttpClient(o =>
            {
                o.EndPoint = new Uri("http://0a1c6d2c.ngrok.io/v1/graphql");                
                o.JsonSerializer = new GraphQL.Client.Serializer.Newtonsoft.NewtonsoftJsonSerializer();
            });           
        }

        public async Task<List<Park>> GetAllParks()
        {
            var allParksRequest = new GraphQLRequest
            {
                Query = @"
	                    {
		                    parks {
                                id
			                    name
                                logo
		                    }
	                    }"
            };

            var response = await _graphQLClient.SendQueryAsync<ParksResponse>(allParksRequest);
            return response.Data.Parks;
        }

        public async Task<ParkRides> GetPark(string parkId)
        {
            var getParkRequet = new GraphQLRequest
            {
                Query = @"
	                    query parks($id: Int) {
                          parks(where: { id: { _eq:  $id } } ) {
                            id

                            name
                            parks_rides {
                              id
                              name
                              logo
                              waitTime
                            }
                          }
                        }",

                Variables = new
                {
                    id = parkId
                }
            };

            var response = await _graphQLClient.SendQueryAsync<ParksRidesResponse>(getParkRequet);
            return response.Data.Parks.FirstOrDefault();
        }

        public IObservable<GraphQLResponse<JObject>> WaitingTimeUpdatedStream(string parkId)
        {
            var req = new GraphQLRequest
            {
                Query = @"
	                    subscription getWaitTime($id: Int) {
                            rides(where:{parkId:{_eq:$id}}) {
                                id
                                waitTime
                            }
                        }",
                Variables = new
                {
                    id = parkId
                }
            };

            return _graphQLClient.CreateSubscriptionStream<JObject>(req);
        }
    }
}
