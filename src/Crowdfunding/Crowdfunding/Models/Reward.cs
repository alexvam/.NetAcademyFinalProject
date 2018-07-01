using System;
using System.Collections.Generic;

namespace Crowdfunding.Models
{
    public partial class Reward
    {
        public Reward()
        {
            Package = new HashSet<Package>();
        }
        
        public long RewardsId { get; set; }
        public long ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ItemsIncluded { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int Amount { get; set; }

        public Project Project { get; set; }
        public ICollection<Package> Package { get; set; }
    }
}
