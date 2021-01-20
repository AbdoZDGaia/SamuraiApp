using System;
using System.Collections.Generic;

namespace SamuraiApp.Data2
{
    public partial class Clans
    {
        public Clans()
        {
            Samurais = new HashSet<Samurais>();
        }

        public int Id { get; set; }
        public int ClanName { get; set; }

        public virtual ICollection<Samurais> Samurais { get; set; }
    }
}
