using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

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


        public void MailUser(Transaction item)
        {

            var message = new MailMessage
            {
                Body = "The project you have supported has been funded successfully",
                From = new MailAddress("amzantlmsr@gmail.com", "Crowfunding"),
                Subject = item.Project.ProjectName,
            };

            message.To.Add(new MailAddress(item.Member.EmailAddress));


            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.Port = 587;
            client.Credentials = new NetworkCredential("amzantlmsr", "paok1926");
            client.EnableSsl = true;
            client.SendAsync(message, item.Member.EmailAddress);
        }        

    }

}
