// ReSharper disable ConstantNullCoalescingCondition
namespace Profilum.GatewayService.Common.BaseModels
{
	// ReSharper disable once ClassNeverInstantiated.Global
	public class AppResponse
	{
		public class Response
		{
			public bool IsCriticalCode =>
				ResultCode is not (ResponseCodes.SUCCESS or ResponseCodes.NOT_FOUND_RECORDS);

			public bool IsSuccess => ResultCode == ResponseCodes.SUCCESS;

			public ResponseCodes ResultCode { get; protected init; } = ResponseCodes.SUCCESS;

			protected List<CustomError> Errors { get; set; }

			// ReSharper disable once ConstantConditionalAccessQualifier
			public string? LastResultMessage => Errors?.LastOrDefault()?.ResultMessage;
		}

		public class Response<T> : Response
		{
			protected Response()
			{}
			
	        public Response(T data) => Data = data;

	        public T Data { get; }
        }

		public class ErrorResponse : Response
		{
			public ErrorResponse(string? errorMessage, ResponseCodes responseCode = ResponseCodes.TECHNICAL_ERROR)
			{
				ResultCode = responseCode;

				Errors ??= new List<CustomError>();
				
				Errors.Add(new CustomError
				{
					ResponseCode = responseCode,
					ResultMessage = errorMessage
				});
			}
		}

		public class ErrorResponse<T> : Response<T>
		{
			public ErrorResponse(string? errorMessage, ResponseCodes responseCode = ResponseCodes.TECHNICAL_ERROR)
			{
				ResultCode = responseCode;

				Errors ??= new List<CustomError>();
				
				Errors.Add(new CustomError
				{
					ResponseCode = responseCode,
					ResultMessage = errorMessage
				});
			}
		}
	}
}