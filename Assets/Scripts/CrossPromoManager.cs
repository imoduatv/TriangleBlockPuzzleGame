using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;

public class CrossPromoManager : Singleton<CrossPromoManager>
{
	public Sprite WhiteImage;

	public Sprite CancelImage;

	public Camera CameraAds;

	public GameObject EventSystemGO;

	private string GET_ADS_URL = "http://crosspromo.dtac.vn/api/api_ads/getAds";

	private string CLICK_ADS_URL = "http://crosspromo.dtac.vn/api/api_ads/onClicks";

	public string appID;

	private string deviceType;

	private string ads_id;

	private BannerCache bannerCache;

	private static bool isShow;

	private bool isCancelShowAds;

	private GameObject _Banner;

	private void Awake()
	{
		EventSystemGO = GameObject.Find("EventSystem");
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
		if (Screen.height > Screen.width)
		{
			UnityEngine.Debug.Log("Potrait");
			CameraAds.orthographicSize = 5f;
		}
		else
		{
			CameraAds.orthographicSize = 2.8f;
			UnityEngine.Debug.Log("Landscape");
		}
		deviceType = "Android";
		if (!isShow)
		{
			isShow = true;
			if (GameData.Instance().GetIsAds())
			{
				sendRequesGetAds();
			}
		}
	}

	public void sendRequesGetAds()
	{
		UnityEngine.Debug.Log("sendRequesGetAds");
		UnityEngine.Debug.Log("appID: " + appID);
		UnityEngine.Debug.Log("deviceType: " + deviceType);
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("appID", appID);
		wWWForm.AddField("device", deviceType);
		WWW www = new WWW(GET_ADS_URL, wWWForm);
		StartCoroutine(WaitForRequest(www, isRequest: true));
	}

	public void sendRequestClickAds()
	{
		UnityEngine.Debug.Log("sendRequestClickAds");
		UnityEngine.Debug.Log("appID: " + appID);
		UnityEngine.Debug.Log("deviceType: " + deviceType);
		UnityEngine.Debug.Log("ads_id: " + ads_id);
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("appID", appID);
		wWWForm.AddField("device", deviceType);
		wWWForm.AddField("ads_id", ads_id);
		WWW www = new WWW(CLICK_ADS_URL, wWWForm);
		StartCoroutine(WaitForRequest(www, isRequest: false));
	}

	public void CancelShowAds()
	{
		isCancelShowAds = true;
	}

	public bool ShowAdsFromCache()
	{
		if (bannerCache == null)
		{
			return false;
		}
		Sprite sprite = bannerCache.sprite;
		string storeLink = bannerCache.storeLink;
		CreateBannerAds(sprite, storeLink);
		bannerCache = null;
		return true;
	}

	public void CancelBannerClick()
	{
		if (EventSystemGO != null)
		{
			EventSystemGO.SetActive(value: true);
		}
		UnityEngine.Object.Destroy(_Banner);
	}

	private IEnumerator WaitForRequest(WWW www, bool isRequest)
	{
		UnityEngine.Debug.Log("Wait");
		yield return www;
		if (www.error == null)
		{
			UnityEngine.Debug.Log("Reponse");
			string text = www.text;
			UnityEngine.Debug.Log(text);
			if (isRequest)
			{
				DataAppPromo dataAppPromo = JsonConvert.DeserializeObject<DataAppPromo>(www.text);
				try
				{
					UnityEngine.Debug.Log(dataAppPromo.data[0].id);
					UnityEngine.Debug.Log(dataAppPromo.data[0].name);
					UnityEngine.Debug.Log(dataAppPromo.data[0].icon_url);
					UnityEngine.Debug.Log(dataAppPromo.data[0].link);
					UnityEngine.Debug.Log(dataAppPromo.data[0].ads_data[0].id_ads);
					UnityEngine.Debug.Log(dataAppPromo.data[0].ads_data[0].photo_url);
					UnityEngine.Debug.Log(dataAppPromo.data[0].ads_data[0].type);
					ads_id = dataAppPromo.data[0].ads_data[0].id_ads;
					string photo_url = dataAppPromo.data[0].ads_data[0].photo_url;
					string link = dataAppPromo.data[0].link;
					StartCoroutine(LoadBannerAds(photo_url, link));
				}
				catch (Exception message)
				{
					UnityEngine.Debug.Log(message);
					if (EventSystemGO != null)
					{
						EventSystemGO.SetActive(value: true);
					}
				}
			}
		}
		else
		{
			UnityEngine.Debug.Log("WWW Error: " + www.error);
			if (EventSystemGO != null)
			{
				EventSystemGO.SetActive(value: true);
			}
		}
	}

	public void CreateBannerAds(Sprite sprite, string storeLink)
	{
		GameObject gameObject = CreateLayout();
		GameObject gameObject2 = CreateBanner(sprite, storeLink);
		GameObject gameObject3 = CreateCancelButton();
		Vector2 size = gameObject2.GetComponent<BoxCollider2D>().size;
		float num = (0f - size.x) / 2f;
		Vector2 size2 = gameObject3.GetComponent<BoxCollider2D>().size;
		float x = num + size2.x / 2f;
		Vector2 size3 = gameObject2.GetComponent<BoxCollider2D>().size;
		float num2 = (0f - size3.y) / 2f;
		Vector2 size4 = gameObject3.GetComponent<BoxCollider2D>().size;
		float y = num2 + size4.y / 2f;
		gameObject3.transform.localPosition = new Vector3(x, y, -1f);
		gameObject3.transform.parent = gameObject2.transform;
		gameObject2.transform.parent = gameObject.transform;
		_Banner = gameObject;
		if (EventSystemGO != null)
		{
			EventSystemGO.SetActive(value: false);
		}
	}

	public IEnumerator LoadBannerAds(string photoURL, string storeLink)
	{
		WWW www3 = new WWW(photoURL);
		yield return www3;
		Sprite sprite = Sprite.Create(www3.texture, new Rect(0f, 0f, www3.texture.width, www3.texture.height), new Vector2(0.5f, 0.5f));
		if (isCancelShowAds)
		{
			bannerCache = new BannerCache();
			bannerCache.sprite = sprite;
			bannerCache.storeLink = storeLink;
		}
		else
		{
			CreateBannerAds(sprite, storeLink);
		}
	}

	private GameObject CreateLayout()
	{
		GameObject gameObject = new GameObject("Layout Banner");
		gameObject.layer = LayerMask.NameToLayer("ads");
		gameObject.AddComponent<SpriteRenderer>();
		SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
		component.sprite = WhiteImage;
		component.sortingLayerName = "ADS";
		component.sortingOrder = -1;
		component.color = new Color(0f, 0f, 0f, 0.5f);
		BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
		boxCollider2D.isTrigger = true;
		gameObject.transform.localScale = new Vector3(10f, 10f, 1f);
		return gameObject;
	}

	private GameObject CreateBanner(Sprite sprite, string storeLink)
	{
		GameObject gameObject = new GameObject("Banner");
		gameObject.layer = LayerMask.NameToLayer("ads");
		gameObject.AddComponent<SpriteRenderer>();
		SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
		component.sprite = sprite;
		component.sortingLayerName = "ADS";
		component.sortingOrder = 1;
		BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
		boxCollider2D.isTrigger = true;
		ClickPromo clickPromo = gameObject.AddComponent<ClickPromo>();
		clickPromo.setURL(storeLink);
		ClickPromo clickPromo2 = clickPromo;
		clickPromo2.callbackSendClickRequest = (ClickPromo.callback)Delegate.Combine(clickPromo2.callbackSendClickRequest, new ClickPromo.callback(sendRequestClickAds));
		return gameObject;
	}

	private GameObject CreateCancelButton()
	{
		GameObject gameObject = new GameObject("Cancel Button");
		gameObject.layer = LayerMask.NameToLayer("ads");
		gameObject.AddComponent<SpriteRenderer>();
		SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
		component.sprite = CancelImage;
		component.sortingLayerName = "ADS";
		component.sortingOrder = 3;
		BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
		boxCollider2D.isTrigger = true;
		ClickCancel clickCancel = gameObject.AddComponent<ClickCancel>();
		ClickCancel clickCancel2 = clickCancel;
		clickCancel2.callbackCancelClick = (ClickCancel.callback)Delegate.Combine(clickCancel2.callbackCancelClick, new ClickCancel.callback(CancelBannerClick));
		return gameObject;
	}

	private void Test()
	{
		testJson testJson = new testJson();
		testJson.a = 5;
		testJson.c = 4.98f;
		testJson.b = "Simple Object";
		testJson value = testJson;
		string text = JsonConvert.SerializeObject(value);
		UnityEngine.Debug.Log(text);
		testJson testJson2 = JsonConvert.DeserializeObject<testJson>(text);
		UnityEngine.Debug.Log(testJson2.a);
		UnityEngine.Debug.Log(testJson2.b);
		UnityEngine.Debug.Log(testJson2.c);
	}
}
