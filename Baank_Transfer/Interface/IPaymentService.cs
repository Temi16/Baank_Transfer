using System.Threading.Tasks;
using Baank_Transfer.DTO_s;
using Baank_Transfer.DTO_s.BankTransfer;
using Baank_Transfer.DTO_s.GenerateRecipient;
using Baank_Transfer.DTO_s.Transaction;

namespace Baank_Transfer
{
    public interface IPaymentService
    {
        Task<BanksResponseModel> ListOfBanks();
        Task<ValidateAccountNumberResponseModel> ValidateAccountNumber(string code, string accountNumber);
        Task<BankTransferResponseModel> BankTranfer(BankTransferRequestModel model);
        Task<GenerateRecipientResponseModel> GenerateRecipients(string accountNumber, string accountName, string bankCode);
        Task<TransactionResponseModel> GetTransaction(string transferId);
    }
}
