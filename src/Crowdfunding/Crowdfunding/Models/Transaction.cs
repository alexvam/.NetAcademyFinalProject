using System;
using System.Collections.Generic;

namespace Crowdfunding.Models
{
    public partial class Transaction
    {
        public long TransactionId { get; set; }
        public long MemberId { get; set; }
        public long ProjectId { get; set; }
        public decimal Contribution { get; set; }
        public DateTime Date { get; set; }
        public long PackagesId { get; set; }

        public Member Member { get; set; }
        public Package Packages { get; set; }
        public Project Project { get; set; }
    }
}
