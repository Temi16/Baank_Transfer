using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Baank_Transfer.DTO_s;
using System.Linq;
using System.Security.Policy;
using Baank_Transfer.DTO_s.BankTransfer;
using Baank_Transfer.DTO_s.GenerateRecipient;
using Baank_Transfer.DTO_s.Transaction;

namespace Baank_Transfer
{
    public class PaymentService : IPaymentService
    {
        public async Task<BanksResponseModel> ListOfBanks()
        {
            var getHttpClient = new HttpClient();
            getHttpClient.DefaultRequestHeaders.Accept.Clear();
            getHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var baseUri = getHttpClient.BaseAddress = new Uri($"https://api.paystack.co/bank");
            getHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sk_test_c43bd7866c1bd0dd38c7bfcea1c03290ae02d5d3");
            var banksResponseModel = new BanksResponseModel();
            var response = await getHttpClient.GetAsync(baseUri);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<BanksResponseModel>(responseString);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                banksResponseModel.status = true;
                banksResponseModel.data = responseObject.data;
                banksResponseModel.message = responseObject.message;
                return banksResponseModel;
            }
            banksResponseModel.status = false;
            banksResponseModel.message = responseObject.message;
            return banksResponseModel;
        }

        public async Task<ValidateAccountNumberResponseModel> ValidateAccountNumber(string code, string accountNumber)
        {
            var getHttpClient = new HttpClient();
            getHttpClient.DefaultRequestHeaders.Accept.Clear();
            getHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var baseUri = getHttpClient.BaseAddress = new Uri($"https://api.paystack.co/bank/resolve?account_number={accountNumber}&bank_code={code}");
            getHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sk_test_6483775b59a2152f947af8583a987e98eb5c7af2");
            var response = await getHttpClient.GetAsync(baseUri);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<ValidateAccountNumberResponseModel>(responseString);
            var validateAccountNumberResponseModel = new ValidateAccountNumberResponseModel();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                validateAccountNumberResponseModel.status = true;
                validateAccountNumberResponseModel.data = new ValidateAccountNumberDto
                {
                    account_number = responseObject.data.account_number,
                    account_name = responseObject.data.account_name,
                    Code = code,
                    bank_id = responseObject.data.bank_id
                };
                validateAccountNumberResponseModel.message = responseObject.message;
                return validateAccountNumberResponseModel;
            }
            validateAccountNumberResponseModel.status = false;
            validateAccountNumberResponseModel.message = responseObject.message;
            return validateAccountNumberResponseModel;
        }
        public async Task<BankTransferResponseModel> BankTranfer(BankTransferRequestModel model)
        {
            var getHttpClient = new HttpClient();
            getHttpClient.DefaultRequestHeaders.Accept.Clear();
            getHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var baseUri = $"https://api.paystack.co/transfer";
            getHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sk_test_6927d4008aa382df0f8c2b8c666bb77b249b4eb5");
            var bankTransferResponseModel = new BankTransferResponseModel();
            var generateRecipient = await GenerateRecipients(model.BeneficiaryAccountNumber, model.BeneficiaryAccountName, model.BeneficiaryBankCode);
            if(generateRecipient.status != false)
            {
                var response = await getHttpClient.PostAsJsonAsync(baseUri, new
                {
                    source = "balance",
                    amount = model.amount,
                    recipient = generateRecipient.data.recipient_code,
                    reason = "Holiday Flexing"
                });
                var responseString = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<BankTransferResponseModel>(responseString);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    bankTransferResponseModel.status = true;
                    bankTransferResponseModel.message = responseObject.message;
                    bankTransferResponseModel.data = new BankTransferDto
                    {
                        amount = responseObject.data.amount,
                        BeneficiaryAccountNumber = model.BeneficiaryAccountNumber,
                        BeneficiaryAccountName = model.BeneficiaryAccountName,
                        BeneficiaryBankCode = model.BeneficiaryBankCode,
                        reference = responseObject.data.reference,
                        createdAt = responseObject.data.createdAt,
                        currency = responseObject.data.currency,
                        SesssionId = $"{responseObject.data.reference}||{responseObject.data.id}",
                        responseCode = "00",
                        recipient = responseObject.data.recipient,
                        id = responseObject.data.id,
                        ResponseMessage = "Transfer Successfull",
                        ResponseStatus = "SUCCESS"
                    };
                    return bankTransferResponseModel;
                }
                bankTransferResponseModel.status = false;
                bankTransferResponseModel.message = responseObject.message;
                bankTransferResponseModel.data = new BankTransferDto
                {
                    amount = responseObject.data.amount,
                    BeneficiaryAccountNumber = model.BeneficiaryAccountNumber,
                    BeneficiaryAccountName = model.BeneficiaryAccountName,
                    BeneficiaryBankCode = model.BeneficiaryBankCode,
                    responseCode = "99",
                    ResponseMessage = "Transfer Not Successfull",
                    ResponseStatus = "Failure"
                };
                return bankTransferResponseModel;
            }
            bankTransferResponseModel.status = false;
            bankTransferResponseModel.message = generateRecipient.message;
            bankTransferResponseModel.data = new BankTransferDto
            {
                amount = (int)model.amount,
                BeneficiaryAccountNumber = model.BeneficiaryAccountNumber,
                BeneficiaryAccountName = model.BeneficiaryAccountName,
                BeneficiaryBankCode = model.BeneficiaryBankCode,
                responseCode = "99",
                ResponseMessage = "Transfer Not Successfull",
                ResponseStatus = "Failure"
            };
            return bankTransferResponseModel;

        }
        public async Task<GenerateRecipientResponseModel> GenerateRecipients(string accountNumber, string accountName, string bankCode)
        {

            var getHttpClient = new HttpClient();
            getHttpClient.DefaultRequestHeaders.Accept.Clear();
            getHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var baseUri = getHttpClient.BaseAddress = new Uri($"https://api.paystack.co/transferrecipient");
            getHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sk_test_6927d4008aa382df0f8c2b8c666bb77b249b4eb5");
            var response = await getHttpClient.PostAsJsonAsync(baseUri, new
            {
                type = "nuban",
                name = accountName,
                account_number = accountNumber,
                bank_code = bankCode,
                currency = "NGN",
            });
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<GenerateRecipientResponseModel>(responseString);
            var generateRecipientResponseModel = new GenerateRecipientResponseModel();
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
               
                generateRecipientResponseModel.status = true;
                generateRecipientResponseModel.message = responseObject.message;
                generateRecipientResponseModel.data = responseObject.data;
                return generateRecipientResponseModel;
            }

            generateRecipientResponseModel.status = false;
            generateRecipientResponseModel.message = responseObject.message;
            return generateRecipientResponseModel;
        }

        public async Task<TransactionResponseModel> GetTransaction(string transferId)
        {

            var getHttpClient = new HttpClient();
            getHttpClient.DefaultRequestHeaders.Accept.Clear();
            getHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var baseUri = getHttpClient.BaseAddress = new Uri($"https://api.paystack.co/transfer/{transferId}");
            getHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sk_test_6927d4008aa382df0f8c2b8c666bb77b249b4eb5");
            var response = await getHttpClient.GetAsync(baseUri);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<TransactionResponseModel>(responseString);
            var transactionResponseModel = new TransactionResponseModel();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                transactionResponseModel.status = true;
                transactionResponseModel.message = responseObject.message;
                transactionResponseModel.data = new TransactionDto
                {
                    amount = responseObject.data.amount,
                    BeneficiaryAccountNumber = responseObject.data.recipient.details.account_number,
                    BeneficiaryAccountName = responseObject.data.recipient.details.account_name,
                    BeneficiaryBankCode = responseObject.data.recipient.details.bank_code,
                    BeneficiaryBankName = responseObject.data.recipient.details.bank_name,
                    domain = responseObject.data.domain,
                    reference = responseObject.data.reference,
                    createdAt = responseObject.data.createdAt,
                    currency = responseObject.data.currency,
                    SesssionId = $"{responseObject.data.reference}||{responseObject.data.id}",
                    responseCode = "00",
                    recipient = responseObject.data.recipient,
                    id = responseObject.data.id,
                    ResponseStatus = "SUCCESS"
                };
                return transactionResponseModel;
            }

            transactionResponseModel.status = false;
            transactionResponseModel.message = responseObject.message;
            return transactionResponseModel;
        }
    }
}
