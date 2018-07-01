using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crowdfunding.Models
{
    public partial class Member
    {
        public Member()
        {
            Comment = new HashSet<Comment>();
            Project = new HashSet<Project>();
            Transaction = new HashSet<Transaction>();
            Update = new HashSet<Update>();
        }

        public long MemberId { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public DateTime? Birthday { get; set; }

        public ICollection<Comment> Comment { get; set; }
        public ICollection<Project> Project { get; set; }
        public ICollection<Transaction> Transaction { get; set; }
        public ICollection<Update> Update { get; set; }
    }
}
