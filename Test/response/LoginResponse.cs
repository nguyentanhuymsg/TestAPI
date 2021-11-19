using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    public class LoginResponse
    {
        public string token { get; set; }
        public string name { get; set; }
        public string role { get; set; }
        public string code { get; set; }
        public string message { get; set; }
    }
}
