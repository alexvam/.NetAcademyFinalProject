using System;
using System.Collections.Generic;

namespace Crowdfunding.Models
{
    public partial class ProjectStatus
    {
        public ProjectStatus()
        {
            Project = new HashSet<Project>();
        }

        public byte StatusId { get; set; }
        public string StatusDescription { get; set; }
        public string StatusCategory { get; set; }

        public ICollection<Project> Project { get; set; }
    }
}
