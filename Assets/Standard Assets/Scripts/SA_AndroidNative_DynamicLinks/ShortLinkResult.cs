using SA.Common.Models;

namespace SA.AndroidNative.DynamicLinks
{
	public class ShortLinkResult : Result
	{
		private string shortLink = string.Empty;

		public string ShortLink => shortLink;

		public ShortLinkResult(Error error)
			: base(error)
		{
		}

		public ShortLinkResult(string link)
		{
			shortLink = link;
		}
	}
}
