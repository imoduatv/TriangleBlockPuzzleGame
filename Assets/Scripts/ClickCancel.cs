using UnityEngine;

public class ClickCancel : MonoBehaviour
{
	public delegate void callback();

	public callback callbackCancelClick;

	private void OnMouseUp()
	{
		UnityEngine.Debug.Log("Cancel");
		if (callbackCancelClick != null)
		{
			callbackCancelClick();
			callbackCancelClick = null;
		}
	}
}
