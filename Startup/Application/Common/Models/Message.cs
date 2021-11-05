using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace Application.Common.Models
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Link { get; set; }

        public Message(IEnumerable<string> to, string subject, string link)
        {
            To = new List<MailboxAddress>();

            To.AddRange(to.Select(x => MailboxAddress.Parse(x)));
            Subject = subject;
            Link = link;
        }
    }
}
