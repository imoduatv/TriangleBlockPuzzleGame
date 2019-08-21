public class UM_ImageSaveResult : UM_BaseResult
{
	private string _imagePath;

	public string imagePath => _imagePath;

	public UM_ImageSaveResult(string path, bool res)
	{
		_imagePath = path;
		_IsSucceeded = res;
	}
}
