namespace Baank_Transfer.DTO_s.BankTransfer
{
    public class BankTransferResponseModel : BaseResponse<BankTransferDto>
    {
        public BankTransferDto data { get; set; }
    }
}
