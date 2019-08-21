using System;

namespace Root
{
	public interface IAdsRewardService
	{
		void Init();

		void ShowAds(Action closeCallback = null, Action skipCallback = null, Action showFailCallback = null);

		bool IsAdsAvailable();
	}
}
