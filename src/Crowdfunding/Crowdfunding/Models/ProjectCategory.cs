using System;
using System.Collections.Generic;

namespace Crowdfunding.Models
{
    public partial class ProjectCategory
    {
        public ProjectCategory()
        {
            Project = new HashSet<Project>();
        }

        public byte CategoryId { get; set; }
        public string CategoryDescription { get; set; }

        public ICollection<Project> Project { get; set; }
    }
}
