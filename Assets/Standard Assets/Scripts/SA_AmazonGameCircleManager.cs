using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SA_AmazonGameCircleManager : AMN_Singleton<SA_AmazonGameCircleManager>
{
	private GC_Player _player;

	private Dictionary<string, GC_Player> _Players;

	private bool _isInitialized;

	public bool IsInitialized => _isInitialized;

	public GC_Player Player
	{
		get
		{
			return _player;
		}
		set
		{
			_player = value;
		}
	}

	public Dictionary<string, GC_Player> Players => _Players;

	public List<GC_Achievement> Achievements => AmazonNativeSettings.Instance.Achievements;

	public List<GC_Leaderboard> Leaderboards => AmazonNativeSettings.Instance.Leaderboards;

	public event Action<AMN_InitializeResult> OnInitializeResult;

	public event Action<AMN_RequestPlayerDataResult> OnRequestPlayerDataReceived;

	public event Action<AMN_RequestAchievementsResult> OnRequestAchievementsReceived;

	public event Action<AMN_UpdateAchievementResult> OnUpdateAchievementReceived;

	public event Action<AMN_RequestLeaderboardsResult> OnRequestLeaderboardsReceived;

	public event Action<AMN_SubmitLeaderboardResult> OnSubmitLeaderboardReceived;

	public event Action<AMN_LocalPlayerScoreLoadedResult> OnLocalPlayerScoreLoaded;

	public event Action<AMN_ScoresLoadedResult> OnScoresLoaded;

	public SA_AmazonGameCircleManager()
	{
		this.OnInitializeResult = delegate
		{
		};
		this.OnRequestPlayerDataReceived = delegate
		{
		};
		this.OnRequestAchievementsReceived = delegate
		{
		};
		this.OnUpdateAchievementReceived = delegate
		{
		};
		this.OnRequestLeaderboardsReceived = delegate
		{
		};
		this.OnSubmitLeaderboardReceived = delegate
		{
		};
		this.OnLocalPlayerScoreLoaded = delegate
		{
		};
		this.OnScoresLoaded = delegate
		{
		};
		_Players = new Dictionary<string, GC_Player>();
		//base._002Ector();
	}

	private void Awake()
	{
		SubscribeToEvents();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnServiceConnected()
	{
		AMN_InitializeResult obj = new AMN_InitializeResult(success: true);
		this.OnInitializeResult(obj);
	}

	private void OnServiceDisconnected(string error)
	{
		AMN_InitializeResult obj = new AMN_InitializeResult(error);
		this.OnInitializeResult(obj);
	}

	public void Connect()
	{
		if (!_isInitialized)
		{
			Init();
		}
	}

	public void Disconnect()
	{
	}

	public void ShowGCOverlay()
	{
	}

	public void ShowSignInPage()
	{
	}

	public void RetrieveLocalPlayer()
	{
	}

	public void ShowAchievementsOverlay()
	{
	}

	public void RequestAchievements()
	{
	}

	public GC_Achievement GetAchievement(string id)
	{
		return null;
	}

	public void UpdateAchievementProgress(string achieve_id, float score)
	{
	}

	public void ShowLeaderboardsOverlay()
	{
	}

	public void RequestLeaderboards()
	{
	}

	public void SubmitLeaderBoardProgress(string leaderBId, long score)
	{
	}

	public GC_Leaderboard GetLeaderboard(string id)
	{
		return null;
	}

	public void LoadLocalPlayerScores(string id, GC_ScoreTimeSpan timeSpan)
	{
	}

	public void LoadTopScores(string id, GC_ScoreTimeSpan timeSpan)
	{
	}

	public void AddPlayer(GC_Player player)
	{
		if (!_Players.ContainsKey(player.PlayerId))
		{
			_Players.Add(player.PlayerId, player);
		}
	}

	public GC_Player GetPlayerById(string id)
	{
		if (_Players.ContainsKey(id))
		{
			return _Players[id];
		}
		return null;
	}

	private void Init()
	{
		_isInitialized = true;
	}

	private void SubscribeToEvents()
	{
	}
}
