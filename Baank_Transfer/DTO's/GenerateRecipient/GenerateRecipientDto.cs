using System;

namespace Baank_Transfer.DTO_s.GenerateRecipient
{
    public class GenerateRecipientDto
    {
        public bool active { get; set; }
        public DateTime createdAt { get; set; }
        public string currency { get; set; }
        public string domain { get; set; }
        public int id { get; set; }
        public int integration { get; set; }
        public string name { get; set; }
        public string reference { get; set; }
        public string reason { get; set; }
        public string recipient_code { get; set; }
        public string type { get; set; }
    }
}
