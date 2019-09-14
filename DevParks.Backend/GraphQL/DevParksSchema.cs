using System;
using DevParks.Backend.GraphQL.Types;
using DevParks.Backend.Model;
using DevParks.Backend.Services;
using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Subscription;
using GraphQL.Types;

namespace DevParks.Backend.GraphQL
{
    public class DevParksSchema : Schema
    {
        public DevParksSchema(IDependencyResolver resolver)
          : base(resolver)
        {
            Query = resolver.Resolve<DevParksQuery>();
            Mutation = resolver.Resolve<DevParksMutation>();
            Subscription = resolver.Resolve<DevParksSubscription>();
        }
    }

    public class DevParksQuery : ObjectGraphType
    {
        private readonly ParkService _parkService;

        public DevParksQuery(ParkService parkService)
        {
            _parkService = parkService;
            var idArgs = new QueryArguments(new QueryArgument<IdGraphType> { Name = "id" });

            Field<ListGraphType<ParkType>>("parks", resolve: context => _parkService.GetAllParks());
            Field<ParkType>("park", arguments: idArgs,
                 resolve: context => _parkService.GetParkById(context.GetArgument<string>("id")));

            Field<ListGraphType<RideType>>("rides", resolve: context => _parkService.GetRides());
            Field<RideType>("ride", arguments: idArgs,
                 resolve: context => _parkService.GetRideById(context.GetArgument<string>("id")));
        }
    }

    public class DevParksMutation : ObjectGraphType
    {
        private readonly ParkService _parkService;

        public DevParksMutation(ParkService parkService)
        {
            _parkService = parkService;

            var args = new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "rideId" },
                new QueryArgument<StringGraphType> { Name = "waitTime" });

            Field<RideType>("updateWaitTime", arguments: args,
                 resolve: context => _parkService.UpdateRideWaitTime(
                     context.GetArgument<string>("rideId"),
                     context.GetArgument<string>("waitTime")));
        }
    }

    public class DevParksSubscription : ObjectGraphType
    {
        private readonly ParkService _parkService;

        public DevParksSubscription(ParkService parkService)
        {
            _parkService = parkService;
            AddField(new EventStreamFieldType
            {
                Name = "waitingTimeUpdated",
                Type = typeof(RideType),
                Resolver = new FuncFieldResolver<Ride>(ResolveRide),
                Subscriber = new EventStreamResolver<Ride>(SubscribeRide)
            });
        }

        private Ride ResolveRide(ResolveFieldContext context)
        {
            var ride = context.Source as Ride;

            return ride;
        }

        private IObservable<Ride> SubscribeRide(ResolveEventStreamContext context)
        {
            return _parkService.RideWaitTimes();
        }
    }
}