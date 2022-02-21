using System.Diagnostics.CodeAnalysis;

namespace Profilum.GatewayService.Common
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum ResponseCodes
	{
		SUCCESS = 0,

		DATABASE_ERROR = 1,

		USER_NOT_FOUND = 2,

		NOT_FOUND_RECORDS = 4,

		DATABASE_TIME_OUT = 5,

		INVALID_PARAMETER = 6,

		TECHNICAL_ERROR = 10,

		FAILURE = 13,

		INTERNAL_BAD_REQUEST = 15,
		
        SOME_OR_ALL_ENTITIES_WERE_NOT_UPDATED = 20
	}
}