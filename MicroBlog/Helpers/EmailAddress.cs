using System;
namespace MicroBlog.Helpers
{
    public class EmailAddress
    {
        public string Name
        {

            get
            {
                return FirstName + " " + LastName;
            }

            set {}
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }
    }
}
