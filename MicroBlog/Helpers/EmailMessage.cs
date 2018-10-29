using System;
using System.Collections.Generic;

namespace MicroBlog.Helpers
{
    public class EmailMessage
    {
        public List<EmailAddress> To { get; set; } = new List<EmailAddress>();
        public EmailAddress From { get; set; }
        public List<EmailAddress> Bcc { get; set; } = new List<EmailAddress>();
        public List<EmailAddress> CC { get; set; } = new List<EmailAddress>();
        public IDictionary<string, string> Substitutions { get; set; } = new Dictionary<string, string>();
        public string Subject { get; set; }
        public string Content { get; set; }
        public string TemplateId { get; set; }
    }
}
