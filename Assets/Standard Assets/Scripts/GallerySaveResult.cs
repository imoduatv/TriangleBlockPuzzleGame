using SA.Common.Models;

public class GallerySaveResult : Result
{
	private string _imagePath;

	public string imagePath => _imagePath;

	public GallerySaveResult(string path)
	{
		_imagePath = path;
	}

	public GallerySaveResult()
		: base(new Error())
	{
	}
}
