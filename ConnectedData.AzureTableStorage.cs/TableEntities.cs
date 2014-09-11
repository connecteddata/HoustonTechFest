using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectedData.Utils;
using ConnectedData.Domain;

namespace ConnectedData.AzureTableStorage.cs
{
    public class PersonEntity : TableEntity
    {
        public string Id { get; set; }
        public Industry Industry { get; set; }
        public Location Location { get; set; }


        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Headline { get; set; }

        public ISet<Skill> Skills { get; set; }
        public ISet<Education> Educations { get; set; }

        public ISet<Position> Positions { get; set; }

        public bool IsDetailed
        {
            get
            {
                return
                    !String.IsNullOrEmpty(FirstName)
                    || !String.IsNullOrEmpty(LastName)
                    || !String.IsNullOrEmpty(Headline)
                    || !Skills.IsNullOrEmpty()
                    || !Educations.IsNullOrEmpty()
                    || !Positions.IsNullOrEmpty();
            }
        }
    }
}
