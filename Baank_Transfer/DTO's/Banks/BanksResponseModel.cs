using System.Collections.Generic;

namespace Baank_Transfer.DTO_s
{
    public class BanksResponseModel : BaseResponse<List<BankDto>>
    {
        public List<BankDto> data { get; set; } = new List<BankDto>();
    }
}
