using ConnectedData.Domain;
using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.GraphDB
{
    public abstract class CommandHandlerBase<TCommand>
        where TCommand : ICommand
    {
        protected readonly IGraphClient _graphClient;

        public CommandHandlerBase(IGraphClient graphClient)
        {
            _graphClient = graphClient;
        }

        protected void ExecuteWithoutResults(ICypherFluentQuery cypherQuery)
        {
            Console.WriteLine(cypherQuery.Query.DebugQueryText);
            cypherQuery.ExecuteWithoutResults();
        }

    }

    public abstract class DomainObjectCommandHandler<TDomainObjectCommand,TDomainObject>
        : CommandHandlerBase<TDomainObjectCommand>
        where TDomainObjectCommand : DomainObjectCommand<TDomainObject>
        where TDomainObject : DomainObject
    {

        protected abstract ICypherFluentQuery Query(TDomainObject domainObject);

        public DomainObjectCommandHandler(IGraphClient graphClient) : base (graphClient)
        {
        }

        public virtual void Handle(ICommand command)
        {
            Handle(command as TDomainObjectCommand);
        }

        private void Handle(TDomainObjectCommand command)
        {
            this.ExecuteWithoutResults(Query(command.DomainObject));
        }


        public virtual bool CanHandle(ICommand command)
        {
            return CanHandle(command as TDomainObjectCommand);
        }

        private bool CanHandle(TDomainObjectCommand command)
        {
            return null != command;
        }
    }

    public abstract class DomainObjectsCommandHandler<TDomainObjectsCommand, TDomainObjects>
        : CommandHandlerBase<TDomainObjectsCommand>
        where TDomainObjectsCommand : DomainObjectsCommand<TDomainObjects>
        where TDomainObjects : IEnumerable<DomainObject>
    {

        protected abstract ICypherFluentQuery Query(TDomainObjects domainObjects);

        public DomainObjectsCommandHandler(IGraphClient graphClient)
            : base(graphClient)
        {
        }

        public virtual void Handle(ICommand command)
        {
            Handle(command as TDomainObjectsCommand);
        }

        private void Handle(TDomainObjectsCommand command)
        {
            this.ExecuteWithoutResults(Query(command.DomainObjects));
        }

        public virtual bool CanHandle(ICommand command)
        {
            return CanHandle(command as TDomainObjectsCommand);
        }

        private bool CanHandle(TDomainObjectsCommand command)
        {
            return null != command;
        }
    }

}
