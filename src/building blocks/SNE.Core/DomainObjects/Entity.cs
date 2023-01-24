using SNE.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNE.Core.DomainObjects
{
    public abstract class Entity
    {
        public Entity()
        {
            Id= Guid.NewGuid();
        }

        public Guid Id { get; set; }

        private List<Event> _eventos;

        public IReadOnlyCollection<Event> Eventos => _eventos?.AsReadOnly();

        public void AdicionarEvento(Event evento)
        {
            _eventos = _eventos ?? new List<Event>();

            _eventos.Add(evento);
        }

        public void RemoverEvento(Event eventItem)
        {
            _eventos.Remove(eventItem);
        }

        public void LimparEventos()
        {
            _eventos.Clear();
        }


        // Comparações

        public override bool Equals(object? obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;

            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if(ReferenceEquals(a, null) && ReferenceEquals(b, null))
            {
                return true;
            }

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        //cada instancia de um objeto tem um código, mas pra evitar q seja repitido, eu vou multíplicar ele por um num. aleatório e concatenar com o hachCode do id
        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }
    }
}
