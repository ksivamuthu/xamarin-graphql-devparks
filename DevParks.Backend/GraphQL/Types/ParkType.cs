using System;
using System.Collections.Generic;
using DevParks.Backend.Model;
using DevParks.Backend.Services;
using GraphQL.Types;

namespace DevParks.Backend.GraphQL.Types
{
    public class ParkType : ObjectGraphType<Park>
    {
        private readonly ParkService _parkService;

        public ParkType(ParkService parkService)
        {
            _parkService = parkService;

            Name = "Park";

            Field(d => d.Id).Description("The id of the park.");
            Field(d => d.Name).Description("The name of the park.");
            Field(d => d.Location, true);
            Field(d => d.OpeningHours, true);
            Field(d => d.ClosingHours, true);
            Field(d => d.Logo, true);

            Field<ListGraphType<RideType>, List<Ride>>()
                .Name("rides").ResolveAsync(ctx => _parkService.GetRidesByParkId(ctx.Source.Id));      
        }
    }
}
