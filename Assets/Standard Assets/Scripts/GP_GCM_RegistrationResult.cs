public class GP_GCM_RegistrationResult : GooglePlayResult
{
	private string _RegistrationDeviceId = string.Empty;

	public string RegistrationDeviceId => _RegistrationDeviceId;

	public GP_GCM_RegistrationResult()
		: base(GP_GamesStatusCodes.STATUS_INTERNAL_ERROR)
	{
	}

	public GP_GCM_RegistrationResult(string id)
		: base(GP_GamesStatusCodes.STATUS_OK)
	{
		_RegistrationDeviceId = id;
	}
}
