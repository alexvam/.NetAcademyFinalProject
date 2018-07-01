using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crowdfunding.Models
{
    public class ProjectsPackages
    {


        public long ProjectId { get; set; }
        public long PackagesId { get; set; }
        public long TransactionId { get; set; }
        public decimal Price { get; set; }
        public string ProjectName { get; set; }
        public decimal Target { get; set; }
        public string ProjectDescription { get; set; }
        public byte ProjectCategoryId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CategoryDescription { get; set; }
        public string ProjectLocation { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ItemsIncluded { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int Amount { get; set; }
        //public IEnumerable<Package> ListPackages { get; set; }
        //public IEnumerable<Reward> ListRewards { get; set; }
        public IEnumerable<Package> PackageRewards { get; set; }
        public IEnumerable<Transaction>ListBackers { get; set; }
        public long CommentId { get; set; }
        public string Comment1 { get; set; }
        public DateTime Date { get; set; }
        public long MemberId { get; set; }
        //public IEnumerable<Comment> ListComments { get; set; }
        public string FirstName { get; set; }
        public IEnumerable<Comment> CommentMember { get; set; }
        public string EmailAddress { get; set; }



        public Project Project { get; set; }
        public Transaction Transaction { get; set; }
        public ICollection<Package> Package { get; set; }
        public ICollection<Reward> Reward { get; set; }
        public ProjectCategory ProjectCategory { get; set; }
        public ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Member> Members { get; set; }

  

    

    }
}
