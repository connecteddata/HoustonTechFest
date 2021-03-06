﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap;
using StructureMap.Graph;
using ShortBus;
using Neo4jClient;
using System.Configuration;

namespace ConnectedData.Web.DependencyResolution
{
    public static class Bootstrapper
    {
        public static ShortBus.IMediator Mediator
        {
            get { return ConfiguredMediator(); }
        }

        private static ShortBus.IMediator ConfiguredMediator()
        {

            return new ShortBus.Mediator(Resolver());
        }

        private static ShortBus.IDependencyResolver Resolver()
        {
            var graphClient = new Neo4jClient.GraphClient(new Uri(ConfigurationManager.AppSettings["neo4jConnectionString"]));
            graphClient.Connect();
            var container = new StructureMap.Container(
                c =>
                {
                    c.Scan(s =>
                        {
                            s.AssemblyContainingType<IMediator>();
                            s.TheCallingAssembly();
                            s.Assembly("ConnectedData.LinkedIn");
                            //s.Assembly("ConnectedData.GraphDB");
                            s.Assembly("ConnectedData.AzureSQLServer");
                            s.WithDefaultConventions();
                            s.AddAllTypesOf((typeof(IAsyncRequestHandler<,>)));
                            s.AddAllTypesOf((typeof(IRequestHandler<,>)));
                            s.AddAllTypesOf((typeof(IAsyncNotificationHandler<>)));
                            s.AddAllTypesOf(typeof(INotificationHandler<>));

                        });
                    c.ForSingletonOf<IGraphClient>().Use(graphClient);
                }
            );
            return new ShortBus.StructureMap.StructureMapDependencyResolver(container);
        }

    }

    public class UserInterestQueryHandler 
        : ShortBus.IRequestHandler<Domain.UserInterestsQuery, IEnumerable<Domain.Interest>>
    {

        public IEnumerable<Domain.Interest> Handle(Domain.UserInterestsQuery request)
        {
            return new List<Domain.Interest>() {
                new Domain.Interest() { Name = "A dummy handler"}
            }.AsEnumerable();
        }
    }

}