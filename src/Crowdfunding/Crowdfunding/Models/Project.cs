using System;
using System.Collections.Generic;

namespace Crowdfunding.Models
{
    public partial class Project
    {
        public Project()
        {
            Comment = new HashSet<Comment>();
            Material = new HashSet<Material>();
            Package = new HashSet<Package>();
            Reward = new HashSet<Reward>();
            Transaction = new HashSet<Transaction>();
            Update = new HashSet<Update>();
        }

        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public decimal Target { get; set; }
        public long MemberId { get; set; }
        public string ProjectDescription { get; set; }
        public byte ProjectCategoryId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ProjectLocation { get; set; }
        public byte Status { get; set; }

        public Member Member { get; set; }
        public ProjectCategory ProjectCategory { get; set; }
        public ProjectStatus StatusNavigation { get; set; }
        public ICollection<Comment> Comment { get; set; }
        public ICollection<Material> Material { get; set; }
        public ICollection<Package> Package { get; set; }
        public ICollection<Reward> Reward { get; set; }
        public ICollection<Transaction> Transaction { get; set; }
        public ICollection<Update> Update { get; set; }
    }
}
