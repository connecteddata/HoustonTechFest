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

    public abstract class RelationshipCommand<TLeft, TRight> : ICommand
        where TLeft : DomainObject
        where TRight : DomainObject
    {
        public readonly DomainRelationship<TLeft, TRight> Relationship;

        public RelationshipCommand(DomainRelationship<TLeft, TRight> relationship)
        {
            Relationship = relationship;
        }
    }

    public class AddRelationship<TLeft, TRight> : RelationshipCommand<TLeft, TRight>
        where TLeft : DomainObject
        where TRight : DomainObject
    {
        public AddRelationship(DomainRelationship<TLeft, TRight> relationship)
            : base(relationship)
        {

        }
    }

    public class RemoveRelationship<TLeft, TRight> : RelationshipCommand<TLeft, TRight>
        where TLeft : DomainObject
        where TRight : DomainObject
    {
        public RemoveRelationship(DomainRelationship<TLeft, TRight> relationship)
            : base(relationship)
        {

        }
    }

    public abstract class RelationshipCommandHandlerBase<TLeft, TRight> : CommandHandlerBase<RelationshipCommand<TLeft, TRight>>
        where TLeft : DomainObject
        where TRight : DomainObject
    {
        protected abstract ICypherFluentQuery Query(DomainRelationship<TLeft, TRight> relationship);

        public RelationshipCommandHandlerBase(IGraphClient graphClient)
            : base(graphClient)
        {
        }

        public virtual void Handle(ICommand command)
        {
            Handle(command as RelationshipCommand<TLeft,TRight>);
        }

        private void Handle(RelationshipCommand<TLeft, TRight> command)
        {
            this.ExecuteWithoutResults(Query(command.Relationship));
        }


        public virtual bool CanHandle(ICommand command)
        {
            return CanHandle(command as RelationshipCommand<TLeft, TRight>);
        }

        private bool CanHandle(RelationshipCommand<TLeft, TRight> command)
        {
            return null != command;
        }
    }

    public class AddRelationshipCommmandHandler<TLeft, TRight> : RelationshipCommandHandlerBase<TLeft, TRight>
        where TLeft : DomainObject
        where TRight : DomainObject
    {
        public AddRelationshipCommmandHandler(IGraphClient client)
            : base(client)
        { }

        protected override ICypherFluentQuery Query(DomainRelationship<TLeft, TRight> relationship)
        {

            var leftString = string.Format("(left:{0})", relationship.LeftSideDomainObject.EntityString());
            var rightString = string.Format("(right:{0})", relationship.RightSideDomainObject.EntityString());
            var relationshipString = string.Format("left{0}right", DirectionRelationshipString(relationship));

            return _graphClient.Cypher
                .Match(leftString, rightString)
                .Where((DomainObject left) => left.Guid == relationship.LeftSideDomainObject.Guid)
                .AndWhere((DomainObject right) => right.Guid == relationship.RightSideDomainObject.Guid)
                .CreateUnique(relationshipString);

        }

        private string DirectionRelationshipString(DomainRelationship<TLeft, TRight> relationship)
        {
            var cypherConventionName = relationship.Name.ToCypherRelationshipConvention();
            switch (relationship.Direction)
            {
                case Domain.RelationshipDirection.LeftToRight:
                    return string.Format("-[:{0}]->", cypherConventionName);
                case Domain.RelationshipDirection.RightToLeft:
                    return string.Format("<-[:{0}]-", cypherConventionName);
                case Domain.RelationshipDirection.Undirected:
                    return string.Format("-[:{0}]-", cypherConventionName);
                default:
                    throw new InvalidOperationException();
            }
        }
    }

    public class RemoveRelationshipCommandHandler<TLeft,TRight> : RelationshipCommandHandlerBase<TLeft,TRight>
        where TLeft : DomainObject
        where TRight : DomainObject
    {
        public RemoveRelationshipCommandHandler(IGraphClient client)
            : base(client)
        { }

        protected override ICypherFluentQuery Query(DomainRelationship<TLeft, TRight> relationship)
        {

            var leftString = string.Format("(left:{0})", relationship.LeftSideDomainObject.EntityString());
            var rightString = string.Format("(right:{0})", relationship.RightSideDomainObject.EntityString());
            var relationshipString = string.Format("left{0}right", relationship.DirectionRelationshipString<TLeft,TRight>("relationship"));

            return _graphClient.Cypher
                .Match(leftString, relationshipString, rightString)
                .Where((DomainObject left) => left.Guid == relationship.LeftSideDomainObject.Guid)
                .AndWhere((DomainObject right) => right.Guid == relationship.RightSideDomainObject.Guid)
                .Delete("relationship");

        }
    }

    public class AssociateSkillWithPersonCommandHandler : ICommandHandler<ICommand>
    {
        private readonly IGraphClient _client;
        public AssociateSkillWithPersonCommandHandler(IGraphClient client)
        {
            _client = client;
        }


        public void Handle(ICommand command)
        {
            
            ProxyHandler().Handle(ProxyAddCommand(command as AssociateSkillWithPerson));
        }

        public bool CanHandle(ICommand command)
        {
            if (!(command is AssociateSkillWithPerson)) return false;
            
            return ProxyHandler().CanHandle(ProxyAddCommand(command as AssociateSkillWithPerson));
        }

        private AddRelationship<Person1,Concept> ProxyAddCommand(AssociateSkillWithPerson command)
        {
            var relationship = new DomainRelationship<Person1, Concept>(command.Person, command.Skill, Domain.RelationshipDirection.LeftToRight, "HasSkill");
            var addCommand = new AddRelationship<Person1, Concept>(relationship);
            return addCommand;
        }

        private AddRelationshipCommmandHandler<Person1,Concept> ProxyHandler()
        {
            return new AddRelationshipCommmandHandler<Person1, Concept>(_client);
        }
        
        
    }

    public class DisaassociateSkillWithPersonCommandHandler : ICommandHandler<ICommand>
    {
        private readonly IGraphClient _client;
        public DisaassociateSkillWithPersonCommandHandler(IGraphClient client)
        {
            _client = client;
        }


        public void Handle(ICommand command)
        {

            ProxyHandler().Handle(ProxyCommand(command as DisassociateSkillFromPerson));
        }

        public bool CanHandle(ICommand command)
        {
            if (!(command is DisassociateSkillFromPerson)) return false;

            return ProxyHandler().CanHandle(ProxyCommand(command as DisassociateSkillFromPerson));
        }

        private RemoveRelationship<Person1, Concept> ProxyCommand(DisassociateSkillFromPerson command)
        {
            var relationship = new DomainRelationship<Person1, Concept>(command.Person, command.Skill, Domain.RelationshipDirection.LeftToRight, "HasSkill");
            var removeCommand = new RemoveRelationship<Person1, Concept>(relationship);
            return removeCommand;
        }

        private RemoveRelationshipCommandHandler<Person1, Concept> ProxyHandler()
        {
            return new RemoveRelationshipCommandHandler<Person1, Concept>(_client);
        }


    }
}

