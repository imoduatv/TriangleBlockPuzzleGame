using UnityEngine;
using UnityEngine.UI;

public class CheatManager : Singleton<CheatManager>
{
	private const string CHEAT_KEY = "EnableCheat";

	public GameObject CheatPanel;

	public InputField InputField;

	public Button CloseCheat;

	public GameObject Marketing;

	public Text Mode;

	private int m_SoundCount;

	private void Awake()
	{
		int @int = PlayerPrefs.GetInt("EnableCheat", 0);
		UnityEngine.Debug.Log("isCheat:" + @int);
		if (@int > 0)
		{
			EnableCheat(isEnable: true);
		}
		else
		{
			EnableCheat(isEnable: false);
		}
		CheatPanel.SetActive(value: false);
		CloseCheat.onClick.AddListener(CloseCheatPanel);
	}

	private void Start()
	{
	}

	public void OpenCheatPanel()
	{
		CheatPanel.SetActive(value: true);
	}

	public void RaiseCount()
	{
		m_SoundCount++;
	}

	public void CheckOpenCheat()
	{
		if (m_SoundCount == 8)
		{
			OpenCheatPanel();
		}
		m_SoundCount = 0;
	}

	public void CloseCheatPanel()
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		if (InputField.text == "marketing")
		{
			EnableCheat(isEnable: true);
			PlayerPrefs.SetInt("EnableCheat", 1);
			empty2 = "Cheat marketing success";
		}
		else if (InputField.text == "testachievead")
		{
			Analytic.Instance.TestLogAchieveAd();
			empty2 = "Cheat achieve ad success";
		}
		else
		{
			empty2 = "Cheat code is wrong";
		}
		if (!Application.isEditor)
		{
			ShowPopup(empty, empty2);
		}
		else
		{
			UnityEngine.Debug.Log(empty2);
		}
		CheatPanel.SetActive(value: false);
	}

	public void EnableCheat(bool isEnable)
	{
		Marketing.SetActive(isEnable);
	}

	public void ChooseMode(int index)
	{
		GameManager.SetUpType setupType = (GameManager.SetUpType)index;
		Singleton<GameManager>.instance.SetupType = setupType;
		Mode.text = setupType.ToString();
	}

	private void ShowPopup(string title, string message)
	{
		MNPopup mNPopup = new MNPopup(title, message);
		mNPopup.AddAction("OK", null);
		mNPopup.Show();
	}
}
