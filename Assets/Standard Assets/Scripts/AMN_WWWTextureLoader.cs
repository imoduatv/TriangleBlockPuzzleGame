using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class AMN_WWWTextureLoader : MonoBehaviour
{
	private string _url;

	public event Action<Texture2D> OnLoad;

	public AMN_WWWTextureLoader()
	{
		this.OnLoad = delegate
		{
		};
		//base._002Ector();
	}

	public static AMN_WWWTextureLoader Create()
	{
		return new GameObject("WWWTextureLoader").AddComponent<AMN_WWWTextureLoader>();
	}

	public void LoadTexture(string url)
	{
		_url = url;
		StartCoroutine(LoadCoroutin());
	}

	private IEnumerator LoadCoroutin()
	{
		WWW www = new WWW(_url);
		yield return www;
		if (www.error == null)
		{
			this.OnLoad(www.texture);
		}
		else
		{
			this.OnLoad(null);
		}
	}
}
