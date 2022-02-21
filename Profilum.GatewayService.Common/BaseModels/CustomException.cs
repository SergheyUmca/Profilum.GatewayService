namespace Profilum.GatewayService.Common.BaseModels
{
    public class CustomException : Exception
    {
	    // ReSharper disable once MemberCanBePrivate.Global
	    public List<CustomError> Errors { get;}

	    // ReSharper disable once MemberCanBePrivate.Global
	    public CustomError LastError => Errors.LastOrDefault();
		
		public ResponseCodes LastResultCode => LastError?.ResponseCode ?? ResponseCodes.TECHNICAL_ERROR;

		public string? LastErrorMessage => LastError?.ResultMessage;

		public CustomException(ResponseCodes responseCode, string? errorMessage)
		{
			Errors ??= new List<CustomError>();

			Errors.Add(new CustomError
			{
				ResponseCode = responseCode,
				ResultMessage = errorMessage
			});
		}
		public CustomException(IEnumerable<CustomError> errors)
		{
			Errors ??= new List<CustomError>();
			Errors.AddRange(errors);
		}
	}
}