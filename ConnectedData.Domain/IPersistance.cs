using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.Domain
{
    public interface ICommandHandler<in TCommand>
        where TCommand : ICommand
    {
        void Handle(TCommand command);
        bool CanHandle(TCommand command);
    }

    public interface ICommand { }

    public abstract class DomainObjectCommand<TDomainObject> : ICommand
        where TDomainObject : DomainObject 
    { 
        public readonly TDomainObject DomainObject;

        public DomainObjectCommand(TDomainObject domainObject)
        {
            DomainObject = domainObject;
        }
    }

    public abstract class DomainObjectsCommand<TDomainObjects> : ICommand
        where TDomainObjects : IEnumerable<DomainObject>
    {
        public readonly TDomainObjects DomainObjects;


        public DomainObjectsCommand(TDomainObjects domainObjects)
        {
            DomainObjects = domainObjects;
        }

    }


    public interface IPersistance
    {
        void Create<TEnumerable>(TEnumerable entities) where TEnumerable : IEnumerable<DomainObject>;

        void Update<T>(T t) where T : DomainObject;
        //void Update(IEnumerable<T> t);


        void Delete<T>(T t) where T : DomainObject;
        //void Delete(IEnumerable<T> t);

        void Handle(ICommand command);
    }

    public abstract class PersistanceBase : IPersistance
    {
        protected readonly IEnumerable<ICommandHandler<ICommand>> _commandHandlers;

        public PersistanceBase(IEnumerable<ICommandHandler<ICommand>> commandHandlers)
        {
            _commandHandlers = commandHandlers;
        }

        public void Create<T>(T entities) where T : IEnumerable<DomainObject>
        {
            Handle(new CreateDomainObjects<T>(entities));
        }

        public void Update<T>(T t) where T : DomainObject
        {
            Handle(new UpdateDomainObject<T>(t));
        }

        public void Delete<T>(T t) where T : DomainObject
        {
            Handle(new DeleteDomainObject<T>(t));
        }

        public void Handle(ICommand command)
        {
            var handler = _commandHandlers.FirstOrDefault(ch => ch.CanHandle(command));
            if (null == handler) throw new InvalidOperationException(String.Format("Cannot handle the command {0} as there are no handlers registered that can handle it.", command.GetType().Namespace + "." + command.GetType().Name));
            handler.Handle(command);
        }
    }
}
