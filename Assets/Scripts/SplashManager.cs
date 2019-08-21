using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
	public GameObject LogoTeam;

	public GameObject LogoGame;

	private void Start()
	{
		Invoke("LoadNewScene", 2f);
	}

	private void ShowLogoTeam()
	{
		LogoTeam.SetActive(value: true);
		LogoGame.SetActive(value: false);
	}

	private void ShowLogoGame()
	{
		LogoTeam.SetActive(value: false);
		LogoGame.SetActive(value: true);
	}

	private void LoadNewScene()
	{
		SceneManager.LoadScene("Triangle-NewUI-2");
	}
}
