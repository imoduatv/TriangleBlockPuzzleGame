using System;
using System.Threading;

public class AmazonAdBanner
{
	public enum BannerAligns
	{
		Top,
		TopLeft,
		TopRight,
		Bottom,
		BottomLeft,
		BottomRight
	}

	private int _id;

	private BannerAligns _position;

	private AMN_AdProperties _properties;

	private bool _isLoaded;

	private bool _isOnScreen;

	private int _width;

	private int _height;

	public int Id => _id;

	public bool IsLoaded => _isLoaded;

	public bool IsOnScreen => _isOnScreen;

	public int Width => _width;

	public int Height => _height;

	public AMN_AdProperties Properties => _properties;

	public event Action<AmazonAdBanner> OnLoadedAction;

	public event Action<AmazonAdBanner> OnFailedLoadingAction;

	public event Action<AmazonAdBanner> OnExpandedAction;

	public event Action<AmazonAdBanner> OnDismissedAction;

	public event Action<AmazonAdBanner> OnCollapsedAction;

	public AmazonAdBanner(BannerAligns position, int id)
	{
		this.OnLoadedAction = delegate
		{
		};
		this.OnFailedLoadingAction = delegate
		{
		};
		this.OnExpandedAction = delegate
		{
		};
		this.OnDismissedAction = delegate
		{
		};
		this.OnCollapsedAction = delegate
		{
		};
		//base._002Ector();
		_id = id;
		_position = position;
		AMN_AdvertisingProxy.CreateBanner(GetPosition(_position), _id);
	}

	public void SetProperties(int width, int height, AMN_AdProperties props)
	{
		_width = width;
		_height = height;
		_properties = props;
	}

	public void Hide(bool hide)
	{
		AMN_AdvertisingProxy.HideBanner(hide, _id);
	}

	public void Destroy()
	{
		AMN_AdvertisingProxy.DestroyBanner(_id);
	}

	public void Refresh()
	{
		AMN_AdvertisingProxy.RefreshBanner(_id);
	}

	public void HandleOnBannerAdLoaded()
	{
		_isLoaded = true;
		this.OnLoadedAction(this);
	}

	public void HandleOnBannerAdFailedToLoad()
	{
		this.OnFailedLoadingAction(this);
	}

	public void HandleOnBannerAdExpanded()
	{
		_isOnScreen = true;
		this.OnExpandedAction(this);
	}

	public void HandleOnBannerAdDismissed()
	{
		_isOnScreen = false;
		this.OnDismissedAction(this);
	}

	public void HandleOnBannerAdCollapsed()
	{
		_isOnScreen = false;
		this.OnCollapsedAction(this);
	}

	private string GetPosition(BannerAligns BannerAlign)
	{
		string result = "BM";
		switch (BannerAlign)
		{
		case BannerAligns.Top:
			result = "TM";
			break;
		case BannerAligns.TopLeft:
			result = "TL";
			break;
		case BannerAligns.TopRight:
			result = "TR";
			break;
		case BannerAligns.Bottom:
			result = "BM";
			break;
		case BannerAligns.BottomLeft:
			result = "BL";
			break;
		case BannerAligns.BottomRight:
			result = "BR";
			break;
		}
		return result;
	}
}
