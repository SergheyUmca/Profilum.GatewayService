namespace Profilum.AccountService.Common.BaseModels
{
    public class CustomError
    {
        public ResponseCodes ResponseCode { get; set; }

        public string? ResultMessage { get; set; }
    }
}