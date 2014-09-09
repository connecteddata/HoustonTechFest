using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.Domain
{
    
    public class AssociateSkillWithPerson : ICommand
    {
        public readonly Person1 Person;
        public readonly Concept Skill;

        public AssociateSkillWithPerson(Person1 person, Concept skill)
        {
            Person = person;
            Skill = skill;

        }
    }

    public class DisassociateSkillFromPerson : ICommand
    {
        public readonly Person1 Person;
        public readonly Concept Skill;

        public DisassociateSkillFromPerson(Person1 person, Concept skill)
        {
            Person = person;
            Skill = skill;
        }
    }


    public class CreateDomainObject<TDomainObject> : DomainObjectCommand<TDomainObject>
        where TDomainObject : DomainObject
    {
        public CreateDomainObject(TDomainObject @object)
            : base(@object)
        {
        }
    }

    public class CreateDomainObjects<TEnumerableDomainObject> : DomainObjectsCommand<TEnumerableDomainObject>
        where TEnumerableDomainObject : IEnumerable<DomainObject>
    {
        public CreateDomainObjects(TEnumerableDomainObject @objects)
            : base(@objects) { }
    }

    public class UpdateDomainObject<TDomainObject> : DomainObjectCommand<TDomainObject>
        where TDomainObject : DomainObject
    {
        public UpdateDomainObject(TDomainObject @object)
            : base(@object)
        {
        }
    }

    public class DeleteDomainObject<TDomainObject> : DomainObjectCommand<TDomainObject>
        where TDomainObject : DomainObject
    {

        public DeleteDomainObject(TDomainObject @object)
            : base(@object)
        {
        }
    }

}
