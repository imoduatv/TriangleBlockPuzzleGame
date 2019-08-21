using UnityEngine;

public class ClickPromo : MonoBehaviour
{
	public delegate void callback();

	public callback callbackSendClickRequest;

	public string Url;

	public void setURL(string url)
	{
		Url = url;
	}

	private void OnMouseUp()
	{
		UnityEngine.Debug.Log("Click Promo");
		if (Url != null)
		{
			UnityEngine.Debug.Log("URL: " + Url);
			if (callbackSendClickRequest != null)
			{
				callbackSendClickRequest();
				callbackSendClickRequest = null;
			}
			Application.OpenURL(Url);
		}
	}
}
