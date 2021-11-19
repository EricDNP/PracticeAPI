using System;
using Newtonsoft.Json;

namespace PracticeAPI.Models
{
    public class Client : iEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }
    }

    public class PrivateClient : Client, iEntity {}

    public class PublicClient : Client, iEntity {}
}