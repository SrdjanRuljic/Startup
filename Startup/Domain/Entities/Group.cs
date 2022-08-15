using System.Collections.Generic;

namespace Domain.Entities
{
    public class Group
    {
        public ICollection<Connection> Connections { get; set; } = new List<Connection>();

        public string Name { get; set; }

        public Group()
        {
        }

        public Group(string name)
        {
            Name = name;
        }
    }
}