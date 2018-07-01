using System;
using System.Collections.Generic;

namespace Crowdfunding.Models
{
    public partial class Comment
    {
        public long CommentId { get; set; }
        public long MemberId { get; set; }
        public long ProjectId { get; set; }
        public string Comment1 { get; set; } 
        public DateTime Date { get; set; }

        public Member Member { get; set; }
        public Project Project { get; set; }
    }
}
