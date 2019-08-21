using SA.Common.Models;
using System;
using System.Threading;
using UnityEngine;

public class GC_Player
{
	private string _playerId = string.Empty;

	private string _name = string.Empty;

	private string _avatarUrl = string.Empty;

	private Texture2D _avatar;

	public string PlayerId => _playerId;

	public string Name => _name;

	public string AvatarUrl => _avatarUrl;

	public Texture2D Avatar => _avatar;

	public event Action<Texture2D> AvatarLoaded;

	public GC_Player()
	{
		this.AvatarLoaded = delegate
		{
		};
		//base._002Ector();
	}

	public void LoadAvatar()
	{
		if (_avatar != null)
		{
			this.AvatarLoaded(_avatar);
			return;
		}
		UnityEngine.Debug.Log("Amazon Player Avatar Started to Load!");
		WWWTextureLoader wWWTextureLoader = WWWTextureLoader.Create();
		wWWTextureLoader.OnLoad += OnProfileImageLoaded;
		wWWTextureLoader.LoadTexture(_avatarUrl);
	}

	private void OnProfileImageLoaded(Texture2D texture)
	{
		UnityEngine.Debug.Log("Amazon Player OnProfileImageLoaded" + texture);
		_avatar = texture;
		this.AvatarLoaded(_avatar);
	}
}
