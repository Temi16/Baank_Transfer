namespace Baank_Transfer.DTO_s
{
    public class BaseResponse<T>
    {
        public string message { get; set; }

        public bool status { get; set; } = false;
    }
}
