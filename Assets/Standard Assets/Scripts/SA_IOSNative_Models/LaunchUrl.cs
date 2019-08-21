using System;

namespace SA.IOSNative.Models
{
	public class LaunchUrl
	{
		private Uri _URI;

		private string _AbsoluteUrl = string.Empty;

		private string _SourceApplication = string.Empty;

		public bool IsEmpty => _AbsoluteUrl.Equals(string.Empty);

		public Uri URI => _URI;

		public string Host => _URI.Host;

		public string AbsoluteUrl => _AbsoluteUrl;

		public string SourceApplication => _SourceApplication;

		public LaunchUrl(string data)
		{
			string[] array = data.Split('|');
			_AbsoluteUrl = array[0];
			_SourceApplication = array[1];
			if (_AbsoluteUrl.Length > 0)
			{
				_URI = new Uri(_AbsoluteUrl);
			}
		}

		public LaunchUrl(string absoluteUrl, string sourceApplication)
		{
			_AbsoluteUrl = absoluteUrl;
			_SourceApplication = sourceApplication;
			if (_AbsoluteUrl.Length > 0)
			{
				_URI = new Uri(_AbsoluteUrl);
			}
		}
	}
}
