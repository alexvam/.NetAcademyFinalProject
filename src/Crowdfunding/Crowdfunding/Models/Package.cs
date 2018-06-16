using System;
using System.Collections.Generic;

namespace Crowdfunding.Models
{
    public partial class Package
    {
        public Package()
        {
            Transaction = new HashSet<Transaction>();
        }

        public long PackagesId { get; set; }
        public long ProjectId { get; set; }
        public long? RewardsId { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }

        public Project Project { get; set; }
        public Reward Rewards { get; set; }
        public ICollection<Transaction> Transaction { get; set; }
    }
}
