using System;
using System.Collections.Generic;

namespace MicroBlog.Helpers
{
    public class EmailMessage
    {
        public List<EmailAddress> To { get; set; }
        public EmailAddress From { get; set; }
        public List<EmailAddress> Bcc { get; set; }
        public List<EmailAddress> CC { get; set; }
        public IDictionary<string,string> Substitutions { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string TemplateId { get; set; }
    }
}
