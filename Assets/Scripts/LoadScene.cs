using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
	public string SceneName;

	public float time;

	private void Start()
	{
		StartCoroutine(IE_LoadNextScene(time));
	}

	private IEnumerator IE_LoadNextScene(float time)
	{
		yield return new WaitForSecondsRealtime(time);
		SceneManager.LoadScene(SceneName);
	}
}
