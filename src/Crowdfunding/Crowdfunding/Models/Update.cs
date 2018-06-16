using System;
using System.Collections.Generic;

namespace Crowdfunding.Models
{
    public partial class Update
    {
        public long UpdateId { get; set; }
        public long ProjectId { get; set; }
        public long MemberId { get; set; }
        public string UpdateText { get; set; }
        public DateTime Date { get; set; }

        public Member Member { get; set; }
        public Project Project { get; set; }
    }
}
