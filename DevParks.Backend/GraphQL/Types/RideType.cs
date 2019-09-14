using System;
using DevParks.Backend.Model;
using GraphQL.Types;

namespace DevParks.Backend.GraphQL.Types
{
    public class RideType : ObjectGraphType<Ride>
    {
        public RideType()
        {
            Name = "Ride";


            Field(d => d.Id);
            Field(d => d.Name);
            Field(d => d.FastpassBooth);
            Field(d => d.ParkId);
            Field(d => d.WaitTime, nullable: true).Description("The wait time of the ride");
            Field(d => d.FastPassOnly);
            Field(d => d.HeightRestrictions);
            Field(d => d.Intense);
            Field(d => d.LoadingSpeed);
            Field(d => d.OpenEMHEvening);
            Field(d => d.OpenEMHMorning);
            Field(d => d.RiderSwap);
            Field(d => d.Seasonal);
            Field(d => d.ShortName);
            Field(d => d.SpecialNeeds);
            Field(d => d.Logo, true);
        }
    }
}
