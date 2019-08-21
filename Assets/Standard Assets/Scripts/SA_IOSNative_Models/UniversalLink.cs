using System;

namespace SA.IOSNative.Models
{
	public class UniversalLink
	{
		private Uri _URI;

		private string _AbsoluteUrl = string.Empty;

		public bool IsEmpty => _AbsoluteUrl.Equals(string.Empty);

		public Uri URI => _URI;

		public string Host => _URI.Host;

		public string AbsoluteUrl => _AbsoluteUrl;

		public UniversalLink(string absoluteUrl)
		{
			_AbsoluteUrl = absoluteUrl;
			if (_AbsoluteUrl.Length > 0)
			{
				_URI = new Uri(_AbsoluteUrl);
			}
		}
	}
}
