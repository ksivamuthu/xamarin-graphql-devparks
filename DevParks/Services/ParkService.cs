using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevParks.Models;
using GraphQL.Client.Http;
using GraphQL.Common.Request;

namespace DevParks.Services
{
    public class ParkService
    {
        private readonly GraphQLHttpClient _graphQLClient;

        public ParkService()
        {
            _graphQLClient = new GraphQLHttpClient("https://devparks.azurewebsites.net/graphql");
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

            var response = await _graphQLClient.SendQueryAsync(allParksRequest);
            return response.GetDataFieldAs<List<Park>>("parks");
        }

        public async Task<ParkRides> GetPark(string parkId)
        {
            var getParkRequet = new GraphQLRequest
            {
                Query = @"
	                    query parks($id: ID) {
                          park(id: $id) {
                            id
                            name
                            rides {
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

            var response = await _graphQLClient.SendQueryAsync(getParkRequet);
            return response.GetDataFieldAs<ParkRides>("park");
        }

        [Obsolete]
        public Task<GraphQL.Client.IGraphQLSubscriptionResult> Subscribe()
        {
            var req = new GraphQLRequest
            {
                Query = @"
	                    subscription {
                          waitingTimeUpdated {
                            id
                            name
                            waitTime
                          }
                        }"
            };

            
            return _graphQLClient.SendSubscribeAsync(req);
        }
    }
}
