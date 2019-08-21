using SA.Common.Models;
using System.Collections.Generic;

public class MP_MediaPickerResult : Result
{
	private List<MP_MediaItem> _SelectedmediaItems;

	public List<MP_MediaItem> SelectedmediaItems => _SelectedmediaItems;

	public List<MP_MediaItem> Items => SelectedmediaItems;

	public MP_MediaPickerResult(List<MP_MediaItem> selectedItems)
	{
		_SelectedmediaItems = selectedItems;
	}

	public MP_MediaPickerResult(string errorData)
		: base(new Error(errorData))
	{
	}
}
