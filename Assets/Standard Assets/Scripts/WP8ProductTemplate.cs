using System;
using System.Threading;
using UnityEngine;

public class WP8ProductTemplate
{
	private Texture2D _texture;

	public string Description
	{
		get;
		set;
	}

	public string ImgURI
	{
		get;
		set;
	}

	public string Name
	{
		get;
		set;
	}

	public string ProductId
	{
		get;
		set;
	}

	public string Price
	{
		get;
		set;
	}

	public string TransactionID
	{
		get;
		set;
	}

	public WP8PurchaseProductType Type
	{
		get;
		set;
	}

	[Obsolete("This property is obsolete. Use 'IsPurchased' property instead")]
	public bool isPurchased
	{
		get
		{
			return IsPurchased;
		}
		set
		{
			IsPurchased = value;
		}
	}

	public bool IsPurchased
	{
		get;
		set;
	}

	[Obsolete("This property is obsolete. Use 'Texture' property instead")]
	public Texture2D texture => _texture;

	public Texture2D Texture => _texture;

	public event Action<Texture2D> ProdcutImageLoaded;

	public WP8ProductTemplate()
	{
		this.ProdcutImageLoaded = delegate
		{
		};
		//base._002Ector();
	}

	public void LoadProductImage()
	{
		if (_texture != null)
		{
			this.ProdcutImageLoaded(_texture);
			return;
		}
		WPN_TextureLoader wPN_TextureLoader = WPN_TextureLoader.Create();
		wPN_TextureLoader.TextureLoaded += HandleTextureLoaded;
		wPN_TextureLoader.LoadTexture(ImgURI);
	}

	private void HandleTextureLoaded(Texture2D texture)
	{
		_texture = texture;
		this.ProdcutImageLoaded(_texture);
	}
}
