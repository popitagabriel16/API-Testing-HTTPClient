using System;
using System.Collections.Generic;
using System.Text;

namespace APITesting.TDDModels
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Avatar { get; set; }
    }

    public class Support
    {
        public string Url { get; set; }
        public string Text { get; set; }
    }

    public class UserData
    {
        public int Page { get; set; }
        public int Per_page { get; set; }
        public int Total { get; set; }
        public int Total_pages { get; set; }
        public List<User> Data { get; set; }
        public Support Support { get; set; }
    }

    public class Job
    {
        public string name { get; set; }
        public string job { get; set; }
        public DateTime updatedAt { get; set; }
    }
}
