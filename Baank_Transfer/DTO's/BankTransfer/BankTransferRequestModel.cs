namespace Baank_Transfer.DTO_s.BankTransfer
{
    public class BankTransferRequestModel
    {
        public decimal amount { get; set; }
        public string BeneficiaryAccountNumber { get; set; }
        public string BeneficiaryAccountName { get; set; }
        public string BeneficiaryBankCode { get; set; }
     

    }
}
