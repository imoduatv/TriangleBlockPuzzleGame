using ANMiniJSON;
using SA.Common.Util;
using System;
using System.Collections;
using System.Globalization;
using System.Threading;
using UnityEngine;

public class TwitterUserInfo
{
	private string _id;

	private string _description;

	private string _name;

	private string _screen_name;

	private string _location;

	private string _lang;

	private string _rawJSON;

	private string _profile_image_url;

	private string _profile_image_url_https;

	private string _profile_background_image_url;

	private string _profile_background_image_url_https;

	private Texture2D _profile_image;

	private Texture2D _profile_background;

	private Color _profile_background_color;

	private Color _profile_text_color;

	private int _friends_count;

	private int _statuses_count;

	private TwitterStatus _status;

	public string rawJSON => _rawJSON;

	public string id => _id;

	public string name => _name;

	public string description => _description;

	public string screen_name => _screen_name;

	public string location => _location;

	public string lang => _lang;

	public string profile_image_url => _profile_image_url;

	public string profile_image_url_https => _profile_image_url_https;

	public string profile_background_image_url => _profile_background_image_url;

	public string profile_background_image_url_https => _profile_background_image_url_https;

	public int friends_count => _friends_count;

	public int statuses_count => _statuses_count;

	public TwitterStatus status => _status;

	public Texture2D profile_image => _profile_image;

	public Texture2D profile_background => _profile_background;

	public Color profile_background_color => _profile_background_color;

	public Color profile_text_color => _profile_text_color;

	public event Action<Texture2D> ActionProfileImageLoaded;

	public event Action<Texture2D> ActionProfileBackgroundImageLoaded;

	public TwitterUserInfo(string data)
	{
		this.ActionProfileImageLoaded = delegate
		{
		};
		this.ActionProfileBackgroundImageLoaded = delegate
		{
		};
		_profile_background_color = Color.clear;
		_profile_text_color = Color.clear;
		//base._002Ector();
		_rawJSON = data;
		IDictionary dictionary = Json.Deserialize(_rawJSON) as IDictionary;
		_id = Convert.ToString(dictionary["id"]);
		_name = Convert.ToString(dictionary["name"]);
		_description = Convert.ToString(dictionary["description"]);
		_screen_name = Convert.ToString(dictionary["screen_name"]);
		_lang = Convert.ToString(dictionary["lang"]);
		_location = Convert.ToString(dictionary["location"]);
		_profile_image_url = Convert.ToString(dictionary["profile_image_url"]);
		_profile_image_url_https = Convert.ToString(dictionary["profile_image_url_https"]);
		_profile_background_image_url = Convert.ToString(dictionary["profile_background_image_url"]);
		_profile_background_image_url_https = Convert.ToString(dictionary["profile_background_image_url_https"]);
		_friends_count = Convert.ToInt32(dictionary["friends_count"]);
		_statuses_count = Convert.ToInt32(dictionary["statuses_count"]);
		_profile_text_color = HexToColor(Convert.ToString(dictionary["profile_text_color"]));
		_profile_background_color = HexToColor(Convert.ToString(dictionary["profile_background_color"]));
		_status = new TwitterStatus(dictionary["status"] as IDictionary);
	}

	public TwitterUserInfo(IDictionary JSON)
	{
		this.ActionProfileImageLoaded = delegate
		{
		};
		this.ActionProfileBackgroundImageLoaded = delegate
		{
		};
		_profile_background_color = Color.clear;
		_profile_text_color = Color.clear;
		//base._002Ector();
		_id = Convert.ToString(JSON["id"]);
		_name = Convert.ToString(JSON["name"]);
		_description = Convert.ToString(JSON["description"]);
		_screen_name = Convert.ToString(JSON["screen_name"]);
		_lang = Convert.ToString(JSON["lang"]);
		_location = Convert.ToString(JSON["location"]);
		_profile_image_url = Convert.ToString(JSON["profile_image_url"]);
		_profile_image_url_https = Convert.ToString(JSON["profile_image_url_https"]);
		_profile_background_image_url = Convert.ToString(JSON["profile_background_image_url"]);
		_profile_background_image_url_https = Convert.ToString(JSON["profile_background_image_url_https"]);
		_friends_count = Convert.ToInt32(JSON["friends_count"]);
		_statuses_count = Convert.ToInt32(JSON["statuses_count"]);
		_profile_text_color = HexToColor(Convert.ToString(JSON["profile_text_color"]));
		_profile_background_color = HexToColor(Convert.ToString(JSON["profile_background_color"]));
	}

	public void LoadProfileImage()
	{
		if (_profile_image != null)
		{
			this.ActionProfileImageLoaded(_profile_image);
		}
		else
		{
			Loader.LoadWebTexture(_profile_image_url_https, OnProfileImageLoaded);
		}
	}

	public void LoadBackgroundImage()
	{
		if (_profile_background != null)
		{
			this.ActionProfileBackgroundImageLoaded(_profile_background);
		}
		else
		{
			Loader.LoadWebTexture(_profile_background_image_url_https, OnProfileBackgroundLoaded);
		}
	}

	private void OnProfileImageLoaded(Texture2D img)
	{
		if (this != null)
		{
			_profile_image = img;
			this.ActionProfileImageLoaded(_profile_image);
		}
	}

	private void OnProfileBackgroundLoaded(Texture2D img)
	{
		if (this != null)
		{
			_profile_background = img;
			this.ActionProfileBackgroundImageLoaded(_profile_background);
		}
	}

	private Color HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
		return new Color32(r, g, b, byte.MaxValue);
	}
}
