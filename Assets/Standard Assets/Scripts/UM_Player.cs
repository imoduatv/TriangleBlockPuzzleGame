using System;
using System.Threading;
using UnityEngine;

public class UM_Player
{
	private GK_Player _GK_Player;

	private GooglePlayerTemplate _GP_Player;

	private GC_Player _GC_Player;

	public string PlayerId
	{
		get
		{
			switch (Application.platform)
			{
			case RuntimePlatform.Android:
				if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon)
				{
					return _GC_Player.PlayerId;
				}
				return _GP_Player.playerId;
			case RuntimePlatform.IPhonePlayer:
				return _GK_Player.Id;
			default:
				return string.Empty;
			}
		}
	}

	public string Name
	{
		get
		{
			switch (Application.platform)
			{
			case RuntimePlatform.Android:
				if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon)
				{
					return _GC_Player.Name;
				}
				return _GP_Player.name;
			case RuntimePlatform.IPhonePlayer:
				return _GK_Player.Alias;
			default:
				return string.Empty;
			}
		}
	}

	[Obsolete("Avatar is deprectaed, please use SmallPhoto instead")]
	public Texture2D Avatar
	{
		get
		{
			switch (Application.platform)
			{
			case RuntimePlatform.Android:
				if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon)
				{
					return _GC_Player.Avatar;
				}
				return _GP_Player.image;
			case RuntimePlatform.IPhonePlayer:
				return _GK_Player.SmallPhoto;
			default:
				return null;
			}
		}
	}

	public Texture2D SmallPhoto
	{
		get
		{
			switch (Application.platform)
			{
			case RuntimePlatform.Android:
				if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon)
				{
					return _GC_Player.Avatar;
				}
				return _GP_Player.icon;
			case RuntimePlatform.IPhonePlayer:
				return _GK_Player.SmallPhoto;
			default:
				return null;
			}
		}
	}

	public Texture2D BigPhoto
	{
		get
		{
			switch (Application.platform)
			{
			case RuntimePlatform.Android:
				if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon)
				{
					return new Texture2D(1, 1);
				}
				return _GP_Player.image;
			case RuntimePlatform.IPhonePlayer:
				return _GK_Player.BigPhoto;
			default:
				return null;
			}
		}
	}

	public GK_Player GameCenterPlayer => _GK_Player;

	public GooglePlayerTemplate GooglePlayPlayer => _GP_Player;

	public GC_Player GameCirclePlayer => _GC_Player;

	public event Action<Texture2D> BigPhotoLoaded;

	public event Action<Texture2D> SmallPhotoLoaded;

	public UM_Player(GK_Player gk, GooglePlayerTemplate gp, GC_Player gc)
	{
		this.BigPhotoLoaded = delegate
		{
		};
		this.SmallPhotoLoaded = delegate
		{
		};
		//base._002Ector();
		_GK_Player = gk;
		_GP_Player = gp;
		_GC_Player = gc;
		if (_GK_Player != null)
		{
			_GK_Player.OnPlayerPhotoLoaded += HandleOnPlayerPhotoLoaded;
		}
		if (_GP_Player != null)
		{
			_GP_Player.BigPhotoLoaded += HandleBigPhotoLoaded;
			_GP_Player.SmallPhotoLoaded += HandleSmallPhotoLoaded;
		}
		if (_GC_Player != null)
		{
			_GC_Player.AvatarLoaded += AmazonPlayerAvatarLoaded;
		}
	}

	public void LoadBigPhoto()
	{
		switch (Application.platform)
		{
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Android)
			{
				_GP_Player.LoadImage();
			}
			break;
		case RuntimePlatform.IPhonePlayer:
			_GK_Player.LoadPhoto(GK_PhotoSize.GKPhotoSizeNormal);
			break;
		}
	}

	public void LoadSmallPhoto()
	{
		switch (Application.platform)
		{
		case RuntimePlatform.Android:
			if (UltimateMobileSettings.Instance.PlatformEngine == UM_PlatformDependencies.Amazon)
			{
				_GC_Player.LoadAvatar();
			}
			else
			{
				_GP_Player.LoadIcon();
			}
			break;
		case RuntimePlatform.IPhonePlayer:
			_GK_Player.LoadPhoto(GK_PhotoSize.GKPhotoSizeSmall);
			break;
		}
	}

	private void HandleSmallPhotoLoaded(Texture2D tex)
	{
		this.SmallPhotoLoaded(tex);
	}

	private void HandleBigPhotoLoaded(Texture2D tex)
	{
		this.BigPhotoLoaded(tex);
	}

	private void HandleOnPlayerPhotoLoaded(GK_UserPhotoLoadResult res)
	{
		if (res.IsSucceeded)
		{
			if (res.Size == GK_PhotoSize.GKPhotoSizeSmall)
			{
				this.SmallPhotoLoaded(res.Photo);
			}
			else
			{
				this.BigPhotoLoaded(res.Photo);
			}
		}
	}

	private void AmazonPlayerAvatarLoaded(Texture2D avatar)
	{
		_GC_Player.AvatarLoaded -= AmazonPlayerAvatarLoaded;
		this.SmallPhotoLoaded(avatar);
	}
}
