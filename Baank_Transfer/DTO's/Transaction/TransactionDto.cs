namespace Baank_Transfer.DTO_s.Transaction
{
    public class TransactionDto
    {
        public int amount { get; set; }
        public string BeneficiaryAccountNumber { get; set; }
        public string BeneficiaryAccountName { get; set; }
        public string BeneficiaryBankCode { get; set; }
        public string BeneficiaryBankName { get; set; }
        public string reference { get; set; }
        public string currency { get; set; }
        public string SesssionId { get; set; }
        public string responseCode { get; set; }
        public string createdAt { get; set; }
        public int id { get; set; }
        public string ResponseStatus { get; set; }
        public string domain { get; set; }
        public RecipientDto recipient { get; set; }
        //public string ResponseMessage { get; set; }
        //public string ResponseStatus { get; set; }
    }
}
