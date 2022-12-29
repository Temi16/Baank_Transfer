namespace Baank_Transfer.DTO_s.Transaction
{
    public class TransactionResponseModel : BaseResponse<TransactionDto>
    {
        public TransactionDto data { get; set; }
    }
}
