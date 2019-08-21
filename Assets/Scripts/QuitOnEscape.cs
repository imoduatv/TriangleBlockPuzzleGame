using UnityEngine;

public class QuitOnEscape : MonoBehaviour
{
	private bool isShowDialogQuit;

	public void ShowDialogQuit()
	{
		if (isShowDialogQuit)
		{
			isShowDialogQuit = false;
			return;
		}
		isShowDialogQuit = true;
		UnityEngine.Debug.Log("Show dialog quit");
		MNPopup mNPopup = new MNPopup("Confirm", "Quit Application?");
		mNPopup.AddAction("Yes", delegate
		{
			Application.Quit();
		});
		mNPopup.AddAction("No", delegate
		{
			isShowDialogQuit = false;
		});
		mNPopup.AddDismissListener(delegate
		{
			isShowDialogQuit = false;
		});
		mNPopup.Show();
	}
}
