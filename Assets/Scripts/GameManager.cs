using Components;
using Dta.TenTen;
using Dta.TenTen.Hexagonal;
using Dta.TenTen.Normal;
using Dta.TenTen.Triangle;
using Entitas;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
	public enum SetUpType
	{
		None,
		BadPlayer,
		Legendary,
		CantDecide,
		GoodPlayer
	}

	public enum Season
	{
		Summer,
		Spring,
		Autumn,
		Winter
	}

	public enum TutStep
	{
		Tut1,
		Tut2,
		Tut3,
		None
	}

	public enum ABBanner
	{
		Normal,
		Top
	}

	public enum ABGiftEffect
	{
		NoLight,
		Light
	}

	public enum ABSkill
	{
		No,
		Price_5_10,
		Price_10_20
	}

	public enum ABFullAds
	{
		Appotax,
		Appodeal
	}

	[Header("Notification Config")]
	[SerializeField]
	private string title;

	[SerializeField]
	private string message;

	[SerializeField]
	private int timeSchedule;

	public bool isTestScore;

	public int scoreTest;

	public bool isMobileLog;

	public Season season;

	public SetUpType SetupType;

	private TutStep curTutStep;

	private string[] notiMsg = new string[5]
	{
		"The more you play the more logical you are. Test with your friends now!",
		"Play now, Connect Shapes, No Matter Where You Are.",
		"Build and destroy triangle blocks, Do brain exercise now!!",
		"Build and destroy triangle blocks, Play & relax now!!",
		"Play Trigon game and compete with your friends now"
	};

	public const string TUT_STEP_KEY = "Tut.Step.Key";

	public const int SHAPE_NUMBER = 3;

	public const int POINT_GRID = 1;

	public bool isCanDrag = true;

	public BoardType gameType;

	public bool isBomb;

	public GameObject objGrids;

	public GameObject prefabBlockNormal;

	public GameObject prefabShapeNormal;

	public GameObject prefabShadowNormal;

	public GameObject prefabBlockHex;

	public GameObject prefabShapeHex;

	public GameObject prefabShadowHex;

	public GameObject prefabBlockTriangle;

	public GameObject prefabShapeTriangle;

	public GameObject prefabShadowTriangle;

	public GameObject prefabShapeDrag;

	public GameObject prefabShapeShadow;

	public GameObject prefabBombText;

	public UnityEngine.Transform bombTextRoot;

	private Board gridBoardCom;

	private Entity boardEntity;

	public Vector2 gridBoxSize;

	public Vector2 gridHexBoxSize;

	public Vector2 gridTriangleBoxSize;

	public Vector2 shadowNormalDelta;

	public Vector2 shadowHexDelta;

	public Vector2 shadowTriDelta;

	public Vector2 shapeGridSize;

	public Vector2 shapeGridBigSize;

	public UnityEngine.Transform curShape;

	public Entity shapeEntity;

	private Entity backUpShapeEntity;

	private int backUpScore;

	private int backUpMoney;

	private bool isRecentlyUndo;

	private Vector3 backUpShapePos;

	private List<MyVector2> undoNextShapes = new List<MyVector2>();

	private List<MyVector2> undoCacheShapes = new List<MyVector2>();

	private int curColorID;

	public UnityEngine.Transform shapeCenter;

	public int shapeCount;

	private bool m_IsHasDataNormal;

	private bool m_IsHasDataBomb;

	private Entity shapeShadow;

	private ITable table;

	private Dta.TenTen.Triangle.Table triangeTable;

	private UIManager uiManager;

	private ServicesManager serviceManager;

	private SaveGameData m_SaveDataNormal;

	private SaveGameData m_SaveDataBomb;

	private CameraShake cameraShake;

	private EventXmasController eventXmasController;

	public float hexFactorY = 1f;

	public bool isAds = true;

	public int testRotateTime;

	[Header("Material shape")]
	[SerializeField]
	public Material m_NormalMat;

	[SerializeField]
	private Material m_DeadMat;

	[SerializeField]
	private Material m_GameOverMat;

	private float[,] timeDelayAnim;

	private Vector2 shapeFillPos;

	private GameState m_State = GameState.Home;

	private MaterialData m_MaterialData;

	[SerializeField]
	private bool m_IsPlaySpin;

	[SerializeField]
	private bool m_IsShowEffectCombo;

	[SerializeField]
	private bool m_IsChangeColorClear;

	[SerializeField]
	private bool m_IsAlways3Shapes;

	[SerializeField]
	private Ratio2 m_Ratio;

	[SerializeField]
	private AdShow m_AdShow;

	[SerializeField]
	private ScoreEffect m_ScoreEffect;

	[SerializeField]
	private ShowAdResume m_ShowAdResume;

	[SerializeField]
	private GiftTime m_GiftTime;

	[SerializeField]
	private ContinueMode m_ContinueMode = ContinueMode.No;

	[SerializeField]
	private SpinFirstReward m_SpinFirst = SpinFirstReward.R_25;

	public ABBanner AB_Banner;

	private bool m_IsPlaySoundRecord;

	public ABGiftEffect AB_GiftEffect;

	public ABSkill AB_Skill;

	public ABFullAds AB_FullAds;

	[SerializeField]
	private bool m_IsNewRecord;

	private int nextScore100 = 100;

	private bool m_IsCanContinue;

	private bool m_IsNewMatch;

	private string m_IsCanContinueKey = "Can_Continue";

	public int MoveCount;

	public bool IsLongScreen;

	private int m_NotiId = 1;

	private bool isCanUseSkillNew3Shapes = true;

	private WaitForSeconds waitSkill = new WaitForSeconds(1f);

	private bool isCanUseSkillUndo = true;

	private bool isNeedCheckDead;

	private bool isUseNewThree;

	public bool IsCanClearLine;

	public UnityEngine.Color m_ColorShapeClear;

	private UnityEngine.Color borderColor = new UnityEngine.Color(1f, 1f, 1f, 0.5f);

	private List<Entity> m_ListLerp;

	private DateTime m_LastTimePause = DateTime.Now;

	public TutStep CurrentTut => curTutStep;

	public GameState State
	{
		get
		{
			return m_State;
		}
		set
		{
			m_State = value;
		}
	}

	public bool IsPlaySpin => m_IsPlaySpin;

	public bool IsShowEffectCombo => m_IsShowEffectCombo;

	public bool IsChangeColorClear => m_IsChangeColorClear;

	public bool IsAlways3Shapes => m_IsAlways3Shapes;

	public Ratio2 RatioShape => m_Ratio;

	public AdShow AdShowRatio => m_AdShow;

	public ScoreEffect ScoreEffect => m_ScoreEffect;

	public ShowAdResume ShowAdResume => m_ShowAdResume;

	public GiftTime GiftTime => m_GiftTime;

	public ContinueMode ContinueMode => m_ContinueMode;

	public SpinFirstReward SpinFirst => m_SpinFirst;

	public bool IsNewRecord
	{
		get
		{
			return m_IsNewRecord;
		}
		set
		{
			m_IsNewRecord = value;
		}
	}

	public bool IsCanContinue => m_IsCanContinue;

	private void Awake()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.SetInt("IsNewRatio", 2);
		}
		else
		{
			PlayerPrefs.SetInt("IsNewRatio", 1);
		}
		if (!PlayerPrefs.HasKey("FirstDate"))
		{
			PlayerPrefs.SetString("FirstDate", DateTime.Now.ToString());
		}
		Analytic.Instance.InitFB();
		float num = (float)Screen.height * 1f / (float)Screen.width;
		if (num > 1.882353f)
		{
			IsLongScreen = true;
		}
		else
		{
			IsLongScreen = false;
		}
	}

	private void Start()
	{
		if (Application.isMobilePlatform)
		{
			UnityEngine.Debug.unityLogger.logEnabled = isMobileLog;
		}
		m_IsPlaySpin = false;
		m_Ratio = (Ratio2)PlayerPrefs.GetInt("IsNewRatio");
		m_AdShow = (AdShow)PlayerPrefs.GetInt("Ad Frequency");
		AB_FullAds = ABFullAds.Appodeal;
		m_AdShow = AdShow.Ad_0s;
		m_ScoreEffect = ScoreEffect.None;
		m_ShowAdResume = ShowAdResume.ShowAd;
		m_GiftTime = GiftTime.Three;
		m_SpinFirst = SpinFirstReward.R_75;
		AB_Banner = ABBanner.Normal;
		AB_Skill = ABSkill.No;
		int startSkill3Cost;
		int startSkillUndoCost;
		if (AB_Skill == ABSkill.Price_10_20)
		{
			startSkill3Cost = 20;
			startSkillUndoCost = 10;
		}
		else if (AB_Skill == ABSkill.Price_5_10)
		{
			startSkill3Cost = 10;
			startSkillUndoCost = 5;
		}
		else
		{
			startSkill3Cost = 0;
			startSkillUndoCost = 0;
		}
		GameData.Instance().SetStartCost(startSkill3Cost, startSkillUndoCost);
		AB_GiftEffect = ABGiftEffect.NoLight;
		m_ContinueMode = ContinueMode.No;
		m_IsAlways3Shapes = false;
		switch (1)
		{
		case 0:
			m_IsShowEffectCombo = true;
			m_IsChangeColorClear = true;
			break;
		case 1:
			m_IsShowEffectCombo = true;
			m_IsChangeColorClear = false;
			break;
		case 2:
			m_IsShowEffectCombo = false;
			m_IsChangeColorClear = false;
			break;
		}
		Application.targetFrameRate = 60;
		m_MaterialData = Resources.Load<MaterialData>("MaterialData");
		Singleton<ServicesManager>.instance.InitServices(isShowAds: true, isHaveIAP: true);
		serviceManager = Singleton<ServicesManager>.instance;
		LoadSaveGame();
		int num = (int)(curTutStep = (TutStep)ServicesManager.DataNormal().GetInt("Tut.Step.Key", 0));
		if (Singleton<TrailerTest>.instance.IsTrailer)
		{
			curTutStep = TutStep.None;
			m_IsAlways3Shapes = true;
			ClearSaveGame();
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			StartCoroutine(InitGameServices());
		}
		base.gameObject.AddComponent<SystemController>().Init();
		Singleton<AssetManagers>.Instance.InitColor();
		uiManager = UnityEngine.Object.FindObjectOfType<UIManager>();
		eventXmasController = new EventXmasController();
		eventXmasController.ResetCurrentData();
		uiManager.Init();
		isAds = GameData.Instance().GetIsAds();
		if (isAds)
		{
			UnityEngine.Debug.Log("Load full ads");
			Singleton<ServicesManager>.Instance.LoadFullAds();
		}
		uiManager.ShowHomeMenu();
		cameraShake = UnityEngine.Object.FindObjectOfType<CameraShake>();
		prefabBlockNormal.CreatePool();
		prefabShapeNormal.CreatePool();
		prefabShadowNormal.CreatePool();
		prefabBlockHex.CreatePool();
		prefabShapeHex.CreatePool();
		prefabShadowHex.CreatePool();
		prefabBlockTriangle.CreatePool();
		prefabShapeTriangle.CreatePool();
		prefabShadowTriangle.CreatePool();
		prefabShapeDrag.CreatePool();
		prefabShapeShadow.CreatePool();
		prefabBombText.CreatePool();
	}

	public void SkillCreateNewShapes3()
	{
		if (!IsRunTutorial() && State == GameState.Playing && isCanUseSkillNew3Shapes)
		{
			isUseNewThree = true;
			isCanUseSkillNew3Shapes = false;
			isCanUseSkillUndo = false;
			DecreaseMoneyUseSkill(isNew3: true);
			StopUndo();
			StartCoroutine(IE_WaitAnimSkill());
			StartCoroutine(IE_CreateNewShapes3());
		}
	}

	private IEnumerator IE_WaitAnimSkill()
	{
		yield return waitSkill;
		isCanUseSkillNew3Shapes = true;
		isCanUseSkillUndo = true;
	}

	public void SkillUndo()
	{
		Undo();
	}

	public void LoadSaveGame()
	{
		SaveGameManager.instance.LoadGame();
		m_IsHasDataNormal = SaveGameManager.instance.IsHasDataNormal;
		m_IsHasDataBomb = SaveGameManager.instance.IsHasDataBomb;
		m_SaveDataNormal = SaveGameManager.instance.SaveDataNormal;
		m_SaveDataBomb = SaveGameManager.instance.SaveDataBomb;
	}

	private IEnumerator InitGameServices()
	{
		yield return new WaitForSeconds(0.5f);
	}

	public void SetNoAds()
	{
		GameData.Instance().SetIsAds(value: false);
		Singleton<ServicesManager>.Instance.HideBanner();
	}

	public void SetPremium()
	{
		SetNoAds();
		GameData.Instance().SetIsPremium(value: true);
		for (int i = 0; i < 21; i++)
		{
			GameData.Instance().SetUnlockTheme(i, value: true);
		}
	}

	public void ShowFullAds()
	{
		isAds = GameData.Instance().GetIsAds();
		if (isAds)
		{
			Singleton<ServicesManager>.Instance.ShowFullAds();
		}
	}

	public void StartMainGame()
	{
		Singleton<SoundManager>.Instance.PlayStart();
		m_IsNewRecord = false;
		m_IsPlaySoundRecord = false;
		backUpShapeEntity = null;
		undoNextShapes.Clear();
		undoCacheShapes.Clear();
		isRecentlyUndo = false;
		Dta.TenTen.Triangle.ShapeTypeUtil.ResetIndex();
		m_State = GameState.Playing;
		m_IsCanContinue = true;
		if (isBomb && m_IsHasDataBomb)
		{
			GameData.Instance().StartScoring(m_SaveDataBomb.m_CurrentScore);
		}
		else if (!isBomb && m_IsHasDataNormal)
		{
			GameData.Instance().StartScoring(m_SaveDataNormal.m_CurrentScore);
			MoveCount = GameData.Instance().GetMoveCountLast();
			uiManager.LastPlayTime = GameData.Instance().GetTimePlayLast();
			eventXmasController.SetDataFromLoadGame(m_SaveDataNormal.m_CurrentScore);
			m_IsNewMatch = false;
		}
		else
		{
			MoveCount = 0;
			uiManager.LastPlayTime = 0f;
			eventXmasController.ResetCurrentData();
			if (isTestScore)
			{
				GameData.Instance().StartScoring(scoreTest);
			}
			else
			{
				GameData.Instance().StartScoring(0);
			}
			GameData.Instance().ClearCostSkill();
			m_IsNewMatch = true;
		}
		if (SetupType == SetUpType.Legendary)
		{
			m_IsNewMatch = true;
			GameData.Instance().StartScoring(200);
			GameData.Instance().SaveHighScore(BoardType.Triangle, isBomb: false, 300);
		}
		Singleton<UIManager>.instance.ReloadCostSkill();
		UpdateScore(isServer: true, isStartGame: true);
		int currentScore = GameData.Instance().GetCurrentScore();
		nextScore100 = currentScore / 100 * 100 + 100;
		switch (gameType)
		{
		case BoardType.Normal:
			StartGame(10);
			break;
		case BoardType.Hexagonal:
			StartGame(9);
			break;
		case BoardType.Triangle:
			StartGame(8);
			break;
		}
	}

	private void StartGame(int size)
	{
		switch (gameType)
		{
		case BoardType.Normal:
			table = new Dta.TenTen.Normal.Table(size, size);
			break;
		case BoardType.Hexagonal:
			table = new Dta.TenTen.Hexagonal.Table(size);
			break;
		case BoardType.Triangle:
			triangeTable = new Dta.TenTen.Triangle.Table(8, 16);
			table = triangeTable;
			break;
		}
		if (shapeShadow != null && !shapeShadow.isDestroy)
		{
			shapeShadow.isDestroy = true;
			shapeShadow = null;
		}
		table.Reset();
		table.SetBombMode(isBomb);
		if (m_IsHasDataNormal && !isBomb)
		{
			table.LoadTable(m_SaveDataNormal.m_BlockDatas, m_SaveDataNormal.m_IsBombs, m_SaveDataNormal.m_BombTimes);
		}
		else if (m_IsHasDataBomb && isBomb)
		{
			table.LoadTable(m_SaveDataBomb.m_BlockDatas, m_SaveDataBomb.m_IsBombs, m_SaveDataBomb.m_BombTimes);
		}
		UnityEngine.BoxCollider2D component = objGrids.GetComponent<UnityEngine.BoxCollider2D>();
		float num = (float)Screen.height * 1f / (float)Screen.width;
		if (num > 1.882353f)
		{
			Vector2 size2 = component.size;
			Camera main = Camera.main;
			size2.x = main.orthographicSize * main.aspect * 2f * 0.9f;
			size2.y = size2.x;
			component.size = size2;
		}
		UnityEngine.Transform transform = objGrids.transform;
		gridBoardCom = new Board();
		gridBoardCom.rows = table.GetSizeX();
		gridBoardCom.cols = table.GetSizeY();
		Board board = gridBoardCom;
		Vector3 size3 = component.bounds.size;
		board.width = size3.x;
		Board board2 = gridBoardCom;
		Vector3 size4 = component.bounds.size;
		board2.height = size4.y;
		Board board3 = gridBoardCom;
		Vector3 center = component.bounds.center;
		board3.x = center.x - gridBoardCom.width / 2f;
		Board board4 = gridBoardCom;
		Vector3 center2 = component.bounds.center;
		board4.y = center2.y - gridBoardCom.height / 2f;
		gridBoardCom.tranform = transform;
		CreateBoard();
		shapeCount = 0;
		if (SetupType != 0)
		{
			curTutStep = TutStep.None;
			m_IsHasDataNormal = false;
			m_IsHasDataBomb = false;
			CreateBoardVideoAd();
		}
		else
		{
			CreateBoardTutorial();
		}
		CheckCreatNewShape(isFirstCheck: true);
	}

	private IEnumerator IE_CreateNewShapes3()
	{
		Pool pool = Pools.pool;
		Entity[] shapes = pool.GetEntities(Matcher.Shape);
		Entity[] array = shapes;
		foreach (Entity entity in array)
		{
			entity.isDestroy = true;
		}
		shapeCount = 0;
		yield return null;
		CheckCreatNewShape(isFirstCheck: false);
	}

	public void ClearBoard()
	{
		if (boardEntity == null)
		{
			return;
		}
		Pool pool = Pools.pool;
		Entity[] entities = pool.GetEntities(Matcher.Shape);
		Entity[] array = entities;
		foreach (Entity entity in array)
		{
			entity.isDestroy = true;
		}
		Entity[,] board = boardEntity.boardEntities.board;
		for (int j = 0; j < gridBoardCom.rows; j++)
		{
			for (int k = 0; k < gridBoardCom.cols; k++)
			{
				Entity entity2 = board[j, k];
				if (entity2 != null)
				{
					entity2.isDestroy = true;
					board[j, k] = null;
				}
			}
		}
		if (backUpShapeEntity != null)
		{
			backUpShapeEntity.isDestroy = true;
			backUpShapeEntity = null;
		}
		shapeEntity = null;
		curShape = null;
		boardEntity.isDestroy = true;
		boardEntity = null;
		if (shapeShadow != null && !shapeShadow.isDestroy)
		{
			shapeShadow.isDestroy = true;
			shapeShadow = null;
		}
	}

	private void CreateBoard()
	{
		Pool pool = Pools.pool;
		boardEntity = pool.CreateEntity();
		GameObject gameObject = prefabBlockNormal;
		gameObject = ((gameType != BoardType.Hexagonal) ? gameObject : prefabBlockHex);
		gameObject = ((gameType != BoardType.Triangle) ? gameObject : prefabBlockTriangle);
		boardEntity.AddPrefabChild(gameObject);
		boardEntity.AddBoard(gridBoardCom.rows, gridBoardCom.cols, gridBoardCom.x, gridBoardCom.y, gridBoardCom.width, gridBoardCom.height, gridBoardCom.tranform);
		boardEntity.AddBoardEntities(new Entity[gridBoardCom.rows, gridBoardCom.cols]);
		boardEntity.AddType(gameType);
	}

	public bool IsRunTutorial()
	{
		return curTutStep != TutStep.None;
	}

	public bool IsRunTutorial1st()
	{
		return curTutStep == TutStep.Tut1;
	}

	private void CreateBoardTutorial()
	{
		if (IsRunTutorial())
		{
			if (curTutStep == TutStep.Tut1)
			{
				triangeTable.Reset();
				triangeTable.SetTableTutorial1();
				SyncTableToBoard(isNeedCheckClear: false);
				StartCoroutine(uiManager.IEShowTutorial1());
			}
			else if (curTutStep == TutStep.Tut2)
			{
				triangeTable.Reset();
				triangeTable.SetTableTutorial2();
				SyncTableToBoard(isNeedCheckClear: false);
				StartCoroutine(uiManager.IEShowTutorial2(IsPlaySpin));
			}
			else if (curTutStep == TutStep.Tut3)
			{
				triangeTable.Reset();
				triangeTable.SetTableTutorial3();
				SyncTableToBoard(isNeedCheckClear: false);
				curTutStep = TutStep.None;
				ServicesManager.DataNormal().SetInt("Tut.Step.Key", (int)curTutStep);
			}
		}
	}

	private void CreateShapeFromSaveData()
	{
		List<ShapeData> list = null;
		if (m_IsHasDataNormal && !isBomb)
		{
			shapeCount = m_SaveDataNormal.m_ShapeData.Count;
			list = m_SaveDataNormal.m_ShapeData;
		}
		else if (m_IsHasDataBomb && isBomb)
		{
			shapeCount = m_SaveDataBomb.m_ShapeData.Count;
			list = m_SaveDataBomb.m_ShapeData;
		}
		else
		{
			shapeCount = 3;
		}
		if (IsRunTutorial())
		{
			shapeCount = 1;
		}
		Pool pool = Pools.pool;
		GameObject gameObject = prefabShapeNormal;
		gameObject = ((gameType != BoardType.Hexagonal) ? gameObject : prefabShapeHex);
		gameObject = ((gameType != BoardType.Triangle) ? gameObject : prefabShapeTriangle);
		for (int i = 0; i < shapeCount; i++)
		{
			int randomColorIndex = Singleton<AssetManagers>.Instance.GetRandomColorIndex();
			if (IsRunTutorial())
			{
				curColorID = 4;
				randomColorIndex = curColorID;
			}
			UnityEngine.Sprite randomSpriteIndex = Singleton<AssetManagers>.instance.GetRandomSpriteIndex();
			Entity entity = pool.CreateEntity();
			entity.AddPrefab(prefabShapeDrag);
			if (Singleton<TrailerTest>.instance.IsTrailer)
			{
				List<MyVector2> listShapeTrailer = Singleton<TrailerTest>.instance.ListShapeTrailer;
				if (listShapeTrailer.Count > 0)
				{
					Entity entity2 = entity;
					MyVector2 myVector = listShapeTrailer[0];
					int x = myVector.x;
					MyVector2 myVector2 = listShapeTrailer[0];
					entity2.AddShape(new Dta.TenTen.Shape(BoardType.Triangle, x, myVector2.y));
					listShapeTrailer.RemoveAt(0);
				}
				else
				{
					int shapeId = UnityEngine.Random.Range(0, 7);
					int rotateTime = UnityEngine.Random.Range(0, 7);
					entity.AddShape(new Dta.TenTen.Shape(BoardType.Triangle, shapeId, rotateTime));
				}
			}
			else if (list != null)
			{
				entity.AddShape(new Dta.TenTen.Shape(list[i]));
			}
			else
			{
				entity.AddShape(new Dta.TenTen.Shape(gameType, curTutStep, IsPlaySpin));
			}
			entity.AddPrefabChild(gameObject);
			entity.AddType(gameType);
			entity.AddColorID(randomColorIndex);
			entity.AddIndex(i);
		}
		StartCoroutine(IE_ShowBorder());
	}

	private IEnumerator IE_ShowBorder()
	{
		yield return null;
		Entity[] eShapes = Pools.pool.GetGroup(Matcher.AllOf(Matcher.Shape, Matcher.Prefab)).GetEntities();
		int count = eShapes.Length;
		Entity[] array = eShapes;
		foreach (Entity entity in array)
		{
			if (entity.isDestroy)
			{
				count--;
			}
		}
		if (count < 1)
		{
			yield break;
		}
		ThemeName currentTheme = GameData.Instance().GetCurrentTheme();
		bool isHaveBorder = Singleton<ThemeManager>.instance.GetThemeData(currentTheme).IsHaveBorder;
		Entity[] array2 = eShapes;
		foreach (Entity entity2 in array2)
		{
			if (!entity2.isDestroy && entity2.hasChilds)
			{
				table.IsPlaceable(entity2.shape.data);
				foreach (Entity datum in entity2.childs.data)
				{
					ShapeBorder component = datum.spriteRenderer.data.GetComponent<ShapeBorder>();
					component.LoadShapeBorder();
				}
			}
		}
	}

	private void CreateShape()
	{
		isNeedCheckDead = true;
		shapeCount = 3;
		Entity[] entities = Pools.pool.GetGroup(Matcher.AllOf(Matcher.Shape, Matcher.Transform)).GetEntities();
		if (entities.Length == 3)
		{
			return;
		}
		int num = (entities.Length <= 0) ? (-1) : entities.Length;
		for (int i = 0; i < entities.Length; i++)
		{
			for (int j = i; j < entities.Length; j++)
			{
				if (entities[i].index.data > entities[j].index.data)
				{
					Entity entity = entities[i];
					entities[i] = entities[j];
					entities[j] = entity;
				}
			}
		}
		for (int k = 0; k < entities.Length; k++)
		{
			entities[k].shape.data.IsNewIndex = true;
			if (entities[k].index.data != k)
			{
				entities[k].ReplaceIndex(k);
			}
		}
		if (num < 0)
		{
			num = 0;
		}
		if (IsRunTutorial())
		{
			shapeCount = 1;
		}
		Pool pool = Pools.pool;
		GameObject gameObject = prefabShapeNormal;
		gameObject = ((gameType != BoardType.Hexagonal) ? gameObject : prefabShapeHex);
		gameObject = ((gameType != BoardType.Triangle) ? gameObject : prefabShapeTriangle);
		for (int l = num; l < shapeCount; l++)
		{
			int randomColorIndex = Singleton<AssetManagers>.Instance.GetRandomColorIndex();
			if (IsRunTutorial())
			{
				curColorID = 4;
				randomColorIndex = curColorID;
			}
			UnityEngine.Sprite randomSpriteIndex = Singleton<AssetManagers>.instance.GetRandomSpriteIndex();
			Entity entity2 = pool.CreateEntity();
			entity2.AddPrefab(prefabShapeDrag);
			if (Singleton<TrailerTest>.Instance.IsTrailer)
			{
				List<MyVector2> listShapeTrailer = Singleton<TrailerTest>.instance.ListShapeTrailer;
				if (listShapeTrailer.Count > 0)
				{
					Entity entity3 = entity2;
					BoardType boardType = gameType;
					MyVector2 myVector = listShapeTrailer[0];
					int x = myVector.x;
					MyVector2 myVector2 = listShapeTrailer[0];
					entity3.AddShape(new Dta.TenTen.Shape(boardType, x, myVector2.y));
					listShapeTrailer.RemoveAt(0);
				}
				else
				{
					int shapeId = UnityEngine.Random.Range(0, 7);
					int rotateTime = UnityEngine.Random.Range(0, 7);
					entity2.AddShape(new Dta.TenTen.Shape(BoardType.Triangle, shapeId, rotateTime));
				}
			}
			else if (l < undoNextShapes.Count)
			{
				Entity entity4 = entity2;
				BoardType boardType2 = gameType;
				MyVector2 myVector3 = undoNextShapes[l];
				int x2 = myVector3.x;
				MyVector2 myVector4 = undoNextShapes[l];
				entity4.AddShape(new Dta.TenTen.Shape(boardType2, x2, myVector4.y));
			}
			else
			{
				Dta.TenTen.Shape shape = new Dta.TenTen.Shape(gameType, curTutStep, IsPlaySpin);
				if (isUseNewThree)
				{
					if (l == shapeCount - 1)
					{
						if (isNeedCheckDead)
						{
							for (int m = 0; m < 6; m++)
							{
								for (int n = 0; n < 6; n++)
								{
									shape = null;
									shape = new Dta.TenTen.Shape(BoardType.Triangle, m, n);
									if (table.IsPlaceable(shape))
									{
										break;
									}
								}
							}
						}
					}
					else if (isNeedCheckDead)
					{
						isNeedCheckDead = !table.IsPlaceable(shape);
					}
				}
				entity2.AddShape(shape);
			}
			entity2.AddPrefabChild(gameObject);
			entity2.AddType(gameType);
			entity2.AddColorID(randomColorIndex);
			entity2.AddIndex(l);
		}
		isUseNewThree = false;
		isRecentlyUndo = false;
		if (!IsCanClearLine)
		{
			Singleton<SoundManager>.Instance.PlayNewBatch();
		}
		StartCoroutine(IE_ShowBorder());
	}

	public void OnNewShapeCreated()
	{
		StartCoroutine(IE_DelayCheck());
	}

	private IEnumerator IE_DelayCheck()
	{
		yield return null;
		CheckDeadPiece();
		CheckCanPlayMore();
	}

	public void CreateShadow()
	{
		GameObject gameObject = prefabShadowNormal;
		gameObject = ((gameType != BoardType.Hexagonal) ? gameObject : prefabShadowHex);
		gameObject = ((gameType != BoardType.Triangle) ? gameObject : prefabShadowTriangle);
		shapeShadow = Pools.pool.CreateEntity();
		shapeShadow.AddShapeShadow(shapeEntity);
		shapeShadow.AddPrefab(prefabShapeShadow);
		shapeShadow.AddPrefabChild(gameObject);
		shapeShadow.AddScale(1.25f, 1.25f);
		if (shapeShadow.hasChilds)
		{
			foreach (Entity datum in shapeShadow.childs.data)
			{
				datum.grid.material = m_NormalMat;
				datum.ReplaceSprite(datum.sprite.data);
			}
		}
	}

	private void ShowBackUpShape()
	{
		if (backUpShapeEntity != null)
		{
			GameObject gameObject = prefabShapeNormal;
			gameObject = ((gameType != BoardType.Hexagonal) ? gameObject : prefabShapeHex);
			gameObject = ((gameType != BoardType.Triangle) ? gameObject : prefabShapeTriangle);
			Entity entity = backUpShapeEntity;
			entity.shape.data.IsBackUpShape = true;
			entity.shape.data.BackUpShapePos = backUpShapePos;
			entity.AddPrefab(prefabShapeDrag);
			entity.AddPrefabChild(gameObject);
			backUpShapeEntity = null;
			shapeCount++;
		}
	}

	public void CheckShowShadow()
	{
		if (shapeShadow == null || !shapeShadow.hasTransform || shapeEntity == null || !shapeEntity.hasTransform)
		{
			return;
		}
		GridCell gridCell = new GridCell(0, 0);
		float x = shapeGridBigSize.x;
		float y = shapeGridBigSize.y;
		float num = x * 0.5f;
		float num2 = y * 0.5f;
		Entity[,] board = boardEntity.boardEntities.board;
		GridCell firstGrid = shapeEntity.shape.data.firstGrid;
		Entity data = shapeEntity.firstGrid.data;
		Vector2 vector = data.transform.data.position;
		bool flag = true;
		Vector2 vector2 = default(Vector2);
		for (int i = 0; i < gridBoardCom.rows; i++)
		{
			for (int j = 0; j < gridBoardCom.cols; j++)
			{
				if (board[i, j] == null)
				{
					continue;
				}
				vector2.x = board[i, j].position.x;
				vector2.y = board[i, j].position.y;
				if (gameType == BoardType.Triangle && j % 2 != firstGrid.y % 2)
				{
					continue;
				}
				if (vector.x >= vector2.x - num && vector.x < vector2.x + num && vector.y >= vector2.y - num2 && vector.y < vector2.y + num2)
				{
					gridCell.x = i - firstGrid.x;
					gridCell.y = j - firstGrid.y;
					if (!table.IsTryPlaceOk(shapeEntity.shape.data, gridCell))
					{
						break;
					}
					UnityEngine.Transform data2 = shapeShadow.transform.data;
					data2.gameObject.SetActive(value: true);
					if (shapeShadow.hasGrid && i == shapeShadow.grid.row && j == shapeShadow.grid.col)
					{
						flag = false;
						return;
					}
					shapeShadow.ReplaceGrid(i, j);
					Vector3 position = default(Vector3);
					position.x = vector2.x - board[i, j].box.width / 2f;
					position.y = vector2.y - board[i, j].box.height / 2f;
					Vector3 position2 = data2.position;
					position.z = position2.z;
					data2.position = position;
					IsCanClearLine = false;
					if (m_IsChangeColorClear)
					{
						bool flag2 = triangeTable.IsCanClearLine(shapeEntity.shape.data, gridCell.x, gridCell.y);
						if (flag2)
						{
							List<GridCell> cellsCanClear = triangeTable.GetCellsCanClear();
							UnityEngine.Color color = Singleton<AssetManagers>.Instance.GetColor(shapeEntity.colorID.data);
							UnityEngine.Sprite sprite = Singleton<AssetManagers>.Instance.GetSprite(shapeEntity.colorID.data);
							flag = false;
							RestoreColorAllCells();
							ChangeColorCellsCanClear(cellsCanClear, color, sprite);
						}
						else
						{
							RestoreColorAllCells();
						}
						IsCanClearLine = flag2;
					}
					return;
				}
				if (shapeShadow.hasGrid)
				{
					shapeShadow.RemoveGrid();
				}
			}
		}
		if (flag)
		{
			RestoreColorAllCells();
		}
		shapeShadow.transform.data.gameObject.SetActive(value: false);
	}

	public bool TryPlaceShape()
	{
		m_IsPlaySoundRecord = false;
		GridCell gridCell = new GridCell(0, 0);
		float x = shapeGridBigSize.x;
		float y = shapeGridBigSize.y;
		float num = x * 0.5f;
		float num2 = y * 0.5f;
		Entity[,] board = boardEntity.boardEntities.board;
		curColorID = shapeEntity.colorID.data;
		GridCell firstGrid = shapeEntity.shape.data.firstGrid;
		Entity data = shapeEntity.firstGrid.data;
		Vector2 b = data.transform.data.position;
		Vector2 a = default(Vector2);
		for (int i = 0; i < gridBoardCom.rows; i++)
		{
			for (int j = 0; j < gridBoardCom.cols; j++)
			{
				if (board[i, j] == null)
				{
					continue;
				}
				a.x = board[i, j].position.x;
				a.y = board[i, j].position.y;
				if ((gameType != BoardType.Triangle || j % 2 == firstGrid.y % 2) && b.x >= a.x - num && b.x < a.x + num && b.y >= a.y - num2 && b.y < a.y + num2)
				{
					gridCell.x = i - firstGrid.x;
					gridCell.y = j - firstGrid.y;
					bool flag = table.Place(shapeEntity.shape.data, gridCell);
					IsCanClearLine = false;
					if (flag)
					{
						shapeFillPos = b;
						m_ColorShapeClear = Singleton<AssetManagers>.instance.GetColor(shapeEntity.colorID.data);
						Drag component = shapeEntity.transform.data.gameObject.GetComponent<Drag>();
						backUpShapePos = component.PlaceFitOnBoard(a - b);
						int value = shapeEntity.shape.data.GridsCount;
						if (curTutStep < TutStep.None)
						{
							value = 0;
						}
						backUpMoney = GameData.Instance().GetMoney();
						backUpScore = GameData.Instance().GetCurrentScore();
						GameData.Instance().AddScore(value);
						GameData.Instance().UpdateHighScore(gameType, isBomb);
						UpdateScore(isServer: false);
						int currentScore = GameData.Instance().GetCurrentScore();
						if (currentScore >= nextScore100)
						{
							int num3 = nextScore100;
							nextScore100 = currentScore / 100 * 100 + 100;
							GameData.Instance().AddMoney((nextScore100 - num3) / 100);
							uiManager.StartAnimRewardIngame(shapeFillPos);
						}
						int itemByScore = eventXmasController.GetItemByScore(currentScore);
						GameData.Instance().AddSnowFlake(itemByScore);
						if (itemByScore > 0)
						{
							uiManager.StartAnimSnow(shapeFillPos);
						}
						IsCanClearLine = triangeTable.IsCanClearLine(shapeEntity.shape.data);
						StartCoroutine(IEPlaceShapeToBoard(shapeEntity));
						return true;
					}
					break;
				}
			}
		}
		if (shapeShadow != null && !shapeShadow.isDestroy)
		{
			shapeShadow.isDestroy = true;
			shapeShadow = null;
		}
		return false;
	}

	private IEnumerator IEPlaceShapeToBoard(Entity shapeEntity)
	{
		MoveCount++;
		bool isCanClear = IsCanClearLine = triangeTable.IsCanClearLine(shapeEntity.shape.data);
		Singleton<SoundManager>.Instance.PlayMoveSuccess();
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		if (curTutStep == TutStep.Tut3)
		{
			curTutStep = TutStep.None;
		}
		if (shapeShadow != null && !shapeShadow.isDestroy)
		{
			shapeShadow.isDestroy = true;
			shapeShadow = null;
		}
		SetupBackUp();
		DestroyCurrentShape(shapeEntity);
		SyncTableToBoard(isNeedCheckClear: true);
		yield return null;
		CheckCreatNewShape(isFirstCheck: false);
	}

	private void SetupBackUp()
	{
		if (!IsRunTutorial())
		{
			Entity[] entities = Pools.pool.GetGroup(Matcher.AllOf(Matcher.Shape, Matcher.Transform)).GetEntities();
			Entity entity = Pools.pool.CreateEntity();
			entity.AddShape(shapeEntity.shape.data);
			entity.AddType(gameType);
			entity.AddColorID(curColorID);
			entity.AddIndex(shapeEntity.index.data);
			backUpShapeEntity = entity;
			Entity[] entities2 = Pools.pool.GetGroup(Matcher.AllOf(Matcher.OldColorID, Matcher.Grid)).GetEntities();
			for (int i = 0; i < entities2.Length; i++)
			{
				entities2[i].RemoveOldColorID();
			}
		}
	}

	private IEnumerator IESyncTableClearBonus()
	{
		Dta.TenTen.Triangle.Table triTable = (Dta.TenTen.Triangle.Table)table;
		ThemeData themeData = Singleton<ThemeManager>.instance.GetThemeData(GameData.Instance().GetCurrentTheme());
		Entity[,] grids = boardEntity.boardEntities.board;
		List<Block> listClear = table.GetBlocksNeedClear();
		Pool pool = Pools.pool;
		int score = listClear.Count;
		if (curTutStep < TutStep.None)
		{
			score = 0;
		}
		GameData.Instance().AddScore(score);
		GameData.Instance().UpdateHighScore(gameType, isBomb);
		UpdateScore(isServer: false);
		int currentScore = GameData.Instance().GetCurrentScore();
		bool isReward100 = false;
		if (currentScore >= nextScore100)
		{
			isReward100 = true;
			int num = nextScore100;
			nextScore100 = currentScore / 100 * 100 + 100;
			GameData.Instance().AddMoney((nextScore100 - num) / 100);
		}
		int snowByScore = eventXmasController.GetItemByScore(currentScore);
		GameData.Instance().AddSnowFlake(snowByScore);
		int numColClear = triTable.GetNumColClear();
		if (curTutStep != TutStep.None)
		{
			numColClear = 2;
		}
		if (m_State == GameState.Playing && !m_IsPlaySoundRecord)
		{
			Singleton<SoundManager>.Instance.PlayClear(numColClear);
		}
		m_IsPlaySoundRecord = false;
		if (numColClear > 1)
		{
			if (curTutStep == TutStep.None)
			{
				for (int i = 2; i <= numColClear; i++)
				{
					int countCombo = GameData.Instance().GetCountCombo(i);
					countCombo++;
					GameData.Instance().SetCountCombo(i, countCombo);
					Singleton<Achievement>.Instance.CheckUnlockCombo(i, countCombo);
				}
			}
			if (themeData.IsSolidColorTheme)
			{
				uiManager.SetColorBGCombo(m_ColorShapeClear);
			}
			int itemByCombo = eventXmasController.GetItemByCombo(numColClear);
			if (itemByCombo > 0)
			{
				uiManager.StartAnimSnow(shapeFillPos);
			}
			GameData.Instance().AddSnowFlake(itemByCombo);
			uiManager.StartAnimRewardIngame(numColClear, shapeFillPos);
		}
		else
		{
			if (isReward100)
			{
				uiManager.StartAnimRewardIngame(shapeFillPos);
			}
			if (snowByScore > 0)
			{
				uiManager.StartAnimSnow(shapeFillPos);
			}
		}
		if (numColClear == 3)
		{
		}
		for (int j = 0; j < listClear.Count; j++)
		{
			if (listClear[j] == null)
			{
				continue;
			}
			int x = listClear[j].GetX();
			int y = listClear[j].GetY();
			Entity entity = grids[x, y];
			if (entity != null)
			{
				Entity entity2 = pool.CreateEntity();
				entity2.AddPrefab(entity.prefab.gameObject);
				entity2.AddParent(entity.parent.data);
				entity2.AddGrid(entity.grid.row, entity.grid.col);
				entity2.AddPosition(entity.position.x, entity.position.y);
				entity2.AddBox(entity.box.width, entity.box.height);
				entity2.AddSprite(entity.sprite.data);
				entity2.AddSortingLayer(entity.spriteRenderer.data.sortingOrder + 1);
				entity2.AddColor(entity.color.data);
				entity2.AddBonus(1);
				if (entity.hasScaleDirect)
				{
					entity2.AddScaleDirect(entity.scaleDirect.x, entity.scaleDirect.y);
				}
			}
		}
		table.AfterSync();
		SyncTableToBoard(isNeedCheckClear: false);
		yield return new WaitForSeconds(0.5f);
		CreateBoardTutorial();
	}

	private void ChangeColorCellsCanClear(List<GridCell> cells, UnityEngine.Color color, UnityEngine.Sprite sprite)
	{
		Entity[,] board = boardEntity.boardEntities.board;
		int count = cells.Count;
		for (int i = 0; i < count; i++)
		{
			Entity entity = board[cells[i].x, cells[i].y];
			if (entity != null && !entity.isEmpty)
			{
				entity.ReplaceColor(color);
				entity.ReplaceSprite(sprite);
			}
		}
	}

	private void RestoreColorAllCells()
	{
		Entity[,] board = boardEntity.boardEntities.board;
		for (int i = 0; i < gridBoardCom.rows; i++)
		{
			for (int j = 0; j < gridBoardCom.cols; j++)
			{
				Entity entity = board[i, j];
				if (entity != null && !entity.isEmpty)
				{
					int data = entity.colorID.data;
					UnityEngine.Color color = Singleton<AssetManagers>.instance.GetColor(data);
					UnityEngine.Sprite sprite = Singleton<AssetManagers>.instance.GetSprite(data);
					entity.ReplaceColor(color);
					entity.ReplaceSprite(sprite);
				}
			}
		}
	}

	public void SyncTableToBoard(bool isNeedCheckClear, bool isSetup = false)
	{
		Block[,] blocks = this.table.GetBlocks();
		bool[,] blackMap = this.table.GetBlackMap();
		Entity[,] board = boardEntity.boardEntities.board;
		bool[,] array = this.table.GetIsBomb();
		int[,] bombTime = this.table.GetBombTime();
		if (IsRunTutorial() && !isNeedCheckClear)
		{
			curColorID = 1;
		}
		int num = curColorID;
		UnityEngine.Color color = Singleton<AssetManagers>.Instance.GetColor(num);
		UnityEngine.Sprite sprite = Singleton<AssetManagers>.instance.GetSprite(num);
		bool flag = false;
		SaveGameData saveGameData = (!isBomb) ? m_SaveDataNormal : m_SaveDataBomb;
		for (int i = 0; i < gridBoardCom.rows; i++)
		{
			for (int j = 0; j < gridBoardCom.cols; j++)
			{
				Entity entity = board[i, j];
				if (entity == null)
				{
					continue;
				}
				if (blocks[i, j] != null && entity.isEmpty)
				{
					if ((!isBomb && m_IsHasDataNormal) || (isBomb && m_IsHasDataBomb))
					{
						num = saveGameData.m_BlockDatas[i, j].colorId;
						color = Singleton<AssetManagers>.instance.GetColor(num);
						sprite = Singleton<AssetManagers>.instance.GetSprite(num);
					}
					entity.ReplaceColor(color);
					entity.ReplaceColorID(num);
					entity.ReplaceSprite(sprite);
					entity.isEmpty = false;
				}
				else if (blocks[i, j] == null && !entity.isEmpty)
				{
					entity.ReplaceColor(Singleton<AssetManagers>.Instance.tileEmpty);
					entity.ReplaceSprite(Singleton<AssetManagers>.instance.spriteEmpty);
					if (entity.hasColorID)
					{
						entity.ReplaceOldColorID(entity.colorID.data);
						entity.RemoveColorID();
					}
					entity.isEmpty = true;
				}
				if (IsRunTutorial() && blocks[i, j] == null)
				{
					if (blackMap[i, j])
					{
						entity.ReplaceColor(Singleton<AssetManagers>.Instance.tileEmptyLock);
					}
					else
					{
						entity.ReplaceColor(Singleton<AssetManagers>.Instance.tileEmpty);
					}
				}
				if (blocks[i, j] != null && isSetup)
				{
					num = blocks[i, j].Color();
					color = Singleton<AssetManagers>.instance.GetColor(num);
					sprite = Singleton<AssetManagers>.instance.GetSprite(num);
					entity.ReplaceColor(color);
					entity.ReplaceColorID(num);
					entity.ReplaceSprite(sprite);
					entity.isEmpty = false;
				}
				if (array[i, j])
				{
					entity.ReplaceBomb(bombTime[i, j]);
					if (!entity.hasText)
					{
						GameObject gameObject = prefabBombText.Spawn();
						gameObject.gameObject.transform.SetParent(bombTextRoot);
						gameObject.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
						entity.AddText(gameObject.gameObject.GetComponent<UnityEngine.UI.Text>());
					}
					if (bombTime[i, j] == 0)
					{
						flag = true;
					}
				}
				else if (entity.hasBomb)
				{
					UnityEngine.UI.Text data = entity.text.data;
					data.gameObject.Recycle();
					entity.RemoveText();
					entity.RemoveBomb();
				}
			}
		}
		if (isBomb)
		{
			m_IsHasDataBomb = false;
		}
		else
		{
			m_IsHasDataNormal = false;
		}
		if (isNeedCheckClear)
		{
			this.table.ClearLines();
			Dta.TenTen.Triangle.Table table = (Dta.TenTen.Triangle.Table)this.table;
			timeDelayAnim = table.GetTimeDelay();
			if (this.table.GetBlocksNeedClear().Count > 0)
			{
				if (IsRunTutorial())
				{
					ServicesManager.DataNormal().SetInt("Tut.Step.Key", (int)curTutStep);
					uiManager.HideTutorial();
					curTutStep++;
				}
				CheckCanPlayMore();
				StartCoroutine(IESyncTableClearBonus());
			}
			else if (flag)
			{
				StartCoroutine(IEGameOverByBomb());
			}
			else
			{
				CheckDeadPiece();
				if (!m_IsAlways3Shapes)
				{
					CheckCanPlayMore();
				}
			}
			return;
		}
		Dta.TenTen.Triangle.Table table2 = (Dta.TenTen.Triangle.Table)this.table;
		timeDelayAnim = table2.GetTimeDelay();
		if (flag)
		{
			StartCoroutine(IEGameOverByBomb());
			return;
		}
		CheckDeadPiece();
		if (!m_IsAlways3Shapes)
		{
			CheckCanPlayMore();
		}
	}

	public void SetBorderShape()
	{
		ShapeBorder[] array = UnityEngine.Object.FindObjectsOfType<ShapeBorder>();
		ShapeBorder[] array2 = array;
		foreach (ShapeBorder shapeBorder in array2)
		{
			shapeBorder.LoadShapeBorder();
		}
	}

	public void CheckDeadPiece()
	{
		if (m_State != GameState.Playing && m_State != 0)
		{
			return;
		}
		Entity[] entities = Pools.pool.GetGroup(Matcher.AllOf(Matcher.Shape, Matcher.Prefab)).GetEntities();
		int num = entities.Length;
		Entity[] array = entities;
		foreach (Entity entity in array)
		{
			if (entity.isDestroy)
			{
				num--;
			}
		}
		if (num < 1)
		{
			return;
		}
		Entity[] array2 = entities;
		foreach (Entity entity2 in array2)
		{
			if (!entity2.isDestroy && entity2.hasChilds)
			{
				bool flag = table.IsPlaceable(entity2.shape.data);
				foreach (Entity datum in entity2.childs.data)
				{
					if (datum.hasColor)
					{
						if (flag)
						{
							UnityEngine.Color data = datum.color.data;
							data.a = 1f;
							datum.ReplaceColor(data);
							ShapeBorder component = datum.spriteRenderer.data.GetComponent<ShapeBorder>();
							component.Show(isShow: true);
						}
						else
						{
							UnityEngine.Color data2 = datum.color.data;
							data2.a = 0.5f;
							datum.ReplaceColor(data2);
							ShapeBorder component2 = datum.spriteRenderer.data.GetComponent<ShapeBorder>();
							component2.Show(isShow: false);
						}
					}
				}
				bool isCanClearLine = triangeTable.IsCanClearLine(entity2.shape.data);
				if (entity2.hasTransform)
				{
					entity2.transform.data.GetComponent<Drag>().IsCanClearLine = isCanClearLine;
				}
			}
		}
	}

	private void CheckCanPlayMore()
	{
		if (m_State != GameState.Playing || IsRunTutorial())
		{
			return;
		}
		Entity[] entities = Pools.pool.GetGroup(Matcher.AllOf(Matcher.Shape, Matcher.Prefab)).GetEntities();
		bool flag = false;
		int num = entities.Length;
		Entity[] array = entities;
		foreach (Entity entity in array)
		{
			if (entity.isDestroy)
			{
				num--;
			}
		}
		if (num < 1)
		{
			return;
		}
		Entity[] array2 = entities;
		foreach (Entity entity2 in array2)
		{
			if (!entity2.isDestroy && table.IsPlaceable(entity2.shape.data))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			StartCoroutine(IEGameOverByNoSlots());
		}
	}

	public void TestGameOver()
	{
		StartCoroutine(IEGameOverByNoSlots());
	}

	public void UpdateAllGridColor()
	{
		Entity[] entities = Pools.pool.GetEntities(Matcher.AllOf(Matcher.Color, Matcher.SpriteRenderer));
		bool[,] array = null;
		if (table != null)
		{
			array = table.GetBlackMap();
		}
		for (int i = 0; i < entities.Length; i++)
		{
			if (entities[i].hasColorID)
			{
				entities[i].ReplaceColor(Singleton<AssetManagers>.Instance.GetColor(entities[i].colorID.data));
				entities[i].ReplaceSprite(Singleton<AssetManagers>.instance.GetSprite(entities[i].colorID.data));
				continue;
			}
			entities[i].ReplaceSprite(Singleton<AssetManagers>.instance.spriteEmpty);
			entities[i].ReplaceColor(Singleton<AssetManagers>.Instance.tileEmpty);
			int row = entities[i].grid.row;
			int col = entities[i].grid.col;
			if (IsRunTutorial() && array != null && array[row, col])
			{
				entities[i].ReplaceColor(Singleton<AssetManagers>.Instance.tileEmptyLock);
			}
		}
	}

	private void DestroyCurrentShape(Entity shapeEntity)
	{
		shapeCount--;
		shapeEntity.isDestroy = true;
	}

	private void CheckCreatNewShape(bool isFirstCheck)
	{
		if (shapeCount <= 0 || m_IsAlways3Shapes)
		{
			if (isFirstCheck)
			{
				StartCoroutine(IE_CreateShapeFromSaveData());
			}
			else
			{
				CreateShape();
			}
			Pool pool = Pools.pool;
			Entity[] entities = pool.GetEntities(Matcher.ShapeShadow);
			Entity[] array = entities;
			foreach (Entity entity in array)
			{
				entity.isDestroy = true;
			}
		}
	}

	private IEnumerator IE_CreateShapeFromSaveData()
	{
		CreateShapeFromSaveData();
		yield return null;
		yield return null;
		CheckCreatNewShape(isFirstCheck: false);
	}

	public float GetScreenHeight()
	{
		return 2f * Camera.main.orthographicSize;
	}

	public float GetScreenWidth()
	{
		return GetScreenHeight() * Camera.main.aspect;
	}

	private void UpdateScore(bool isServer, bool isStartGame = false)
	{
		int currentScore = GameData.Instance().GetCurrentScore();
		int num = 0;
		num = ((!isServer) ? GameData.Instance().GetHighScore(gameType, isBomb) : GameData.Instance().GetServerHighScore(gameType, isBomb));
		uiManager.UpdateScoreInGame(currentScore, num, isStartGame);
		if (!isStartGame && currentScore >= num && !m_IsNewRecord && currentScore > 0)
		{
			m_IsNewRecord = true;
			m_IsPlaySoundRecord = true;
			Singleton<SoundManager>.instance.PlayHighScore();
		}
	}

	private IEnumerator IEGameOverByBomb()
	{
		m_State = GameState.GameOver;
		ClearSaveGame();
		yield return new WaitForSeconds(0.5f);
		Singleton<SoundManager>.Instance.PlayExplosion();
		cameraShake.StartShake();
		yield return new WaitForSeconds(1f);
		GameData.Instance().SubmitHighScore(gameType, isBomb);
		if (shapeShadow != null)
		{
			shapeShadow.isDestroy = true;
			shapeShadow = null;
		}
		yield return null;
		uiManager.ShowGameOver();
	}

	private IEnumerator IEGameOverByNoSlots()
	{
		m_State = GameState.GameOver;
		Analytic.Instance.LogGameOver(GameData.Instance().GetCurrentScore());
		GameData.Instance().GetCurrentScore();
		int playCount = GameData.Instance().GetPlayCount();
		playCount++;
		GameData.Instance().SetPlayCount(playCount);
		Singleton<Achievement>.instance.CheckUnlockPlay(playCount);
		Singleton<SoundManager>.instance.PlayGameOver(m_IsNewRecord);
		if (m_ListLerp == null)
		{
			m_ListLerp = new List<Entity>();
		}
		else
		{
			m_ListLerp.Clear();
		}
		ClearSaveGame();
		uiManager.TakeBoardScreenshot();
		ThemeName theme = GameData.Instance().GetCurrentTheme();
		ThemeData themeData = Singleton<ThemeManager>.instance.GetThemeData(theme);
		Material mat = (!themeData.IsSolidColorTheme) ? m_GameOverMat : m_NormalMat;
		Block[,] blocks = table.GetBlocks();
		Entity[,] grids = boardEntity.boardEntities.board;
		for (int j = 0; j < gridBoardCom.rows; j++)
		{
			for (int k = 0; k < gridBoardCom.cols; k++)
			{
				Entity entity = grids[j, k];
				if (entity != null && blocks[j, k] != null)
				{
					m_ListLerp.Add(entity);
				}
			}
		}
		int gridCount = m_ListLerp.Count;
		float delayGray = 0f / (float)(gridCount - 1);
		WaitForSeconds m_DelayGray = new WaitForSeconds(delayGray);
		m_ListLerp = ShuffleList(m_ListLerp);
		for (int i = 0; i < m_ListLerp.Count; i++)
		{
			m_ListLerp[i].spriteRenderer.data.material = mat;
			m_ListLerp[i].spriteRenderer.data.GetComponent<LerpGrayDead>().StartLerpGray(themeData.IsSolidColorTheme, 1f);
			yield return m_DelayGray;
		}
		yield return new WaitForSeconds(1f);
		GameData.Instance().SubmitHighScore(gameType, isBomb);
		if (shapeShadow != null)
		{
			shapeShadow.isDestroy = true;
			shapeShadow = null;
		}
		yield return null;
		if (m_IsNewMatch)
		{
			m_IsCanContinue = true;
		}
		else
		{
			m_IsCanContinue = ServicesManager.DataNormal().GetBool(m_IsCanContinueKey, defaultValue: true);
		}
		if (m_ContinueMode == ContinueMode.No)
		{
			m_IsCanContinue = false;
		}
		if (m_IsCanContinue && Singleton<ServicesManager>.instance.IsRewardAvailable() && GameData.Instance().GetCurrentScore() >= 100)
		{
			uiManager.ShowContinuePanel();
		}
		else
		{
			uiManager.ShowGameOver();
		}
	}

	public void ResetBoardColor()
	{
		for (int i = 0; i < m_ListLerp.Count; i++)
		{
			m_ListLerp[i].spriteRenderer.data.GetComponent<LerpGrayDead>().ResetColor();
		}
	}

	private List<Entity> ShuffleList(List<Entity> listOrigin)
	{
		List<Entity> list = new List<Entity>();
		while (listOrigin.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, listOrigin.Count);
			list.Add(listOrigin[index]);
			listOrigin.RemoveAt(index);
		}
		return list;
	}

	public void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			DateTime now = DateTime.Now;
			string @string = PlayerPrefs.GetString("FirstDate");
			DateTime d = DateTime.Parse(@string);
			int days = (now - d).Days;
			if (days < 0)
			{
				days = 0;
			}
			int num = UnityEngine.Random.Range(0, notiMsg.Length);
			LocalNotification.SendNotification(m_NotiId, timeSchedule * 1000, "Trigon", notiMsg[num], UnityEngine.Color.white, false, true, true, "smallicon", "bigicon", null, "default");
			GameData.Instance().SaveData();
			ServicesManager.DataNormal().SetBool(m_IsCanContinueKey, IsCanContinue);
			Singleton<ServicesManager>.instance.SaveLastTimePause();
			if (m_State == GameState.Playing)
			{
				Singleton<UIManager>.instance.GetTimePlay();
			}
			if (!IsRunTutorial() && (m_State == GameState.Playing || m_State == GameState.Pause))
			{
				SaveGame();
			}
		}
		else
		{
			if (m_State == GameState.Playing)
			{
				Singleton<UIManager>.instance.StartCountTimePlay();
			}
			Singleton<ServicesManager>.instance.ShowResumeAds();
			LocalNotification.ClearNotifications();
		}
	}

	private void OnApplicationQuit()
	{
		GameData.Instance().SaveData();
		if (!IsRunTutorial() && (m_State == GameState.Playing || m_State == GameState.Pause))
		{
			SaveGame();
		}
	}

	public void PauseGame()
	{
		m_State = GameState.Pause;
	}

	public void ResumeGame()
	{
		m_State = GameState.Playing;
		CheckDeadPiece();
	}

	private void StopUndo()
	{
		if (backUpShapeEntity != null)
		{
			backUpShapeEntity.isDestroy = true;
			backUpShapeEntity = null;
			table.StopUndo();
		}
	}

	public void Undo()
	{
		if (IsRunTutorial() || State != GameState.Playing || !isCanUseSkillUndo)
		{
			return;
		}
		isCanUseSkillUndo = false;
		isCanUseSkillNew3Shapes = false;
		StartCoroutine(IE_WaitAnimSkill());
		if (backUpShapeEntity == null)
		{
			table.StopUndo();
			return;
		}
		Entity[] entities = Pools.pool.GetGroup(Matcher.AllOf(Matcher.Shape, Matcher.Transform)).GetEntities();
		if (entities.Length == 3)
		{
			Entity[] array = entities;
			foreach (Entity entity in array)
			{
				entity.isDestroy = true;
			}
			shapeCount = 0;
		}
		isRecentlyUndo = true;
		table.Undo();
		ShowBackUpShape();
		ShowBackUpColorGrid();
		SyncTableToBoard(isNeedCheckClear: false);
		GameData.Instance().StartScoring(backUpScore);
		GameData.Instance().AddMoney(backUpMoney - GameData.Instance().GetMoney());
		DecreaseMoneyUseSkill(isNew3: false);
		UpdateScore(isServer: false);
	}

	private void ShowBackUpColorGrid()
	{
		Entity[] entities = Pools.pool.GetGroup(Matcher.AllOf(Matcher.OldColorID, Matcher.Grid)).GetEntities();
		for (int i = 0; i < entities.Length; i++)
		{
			int data = entities[i].oldColorID.data;
			UnityEngine.Color color = Singleton<AssetManagers>.Instance.GetColor(data);
			entities[i].ReplaceColor(color);
			entities[i].ReplaceColorID(data);
			entities[i].RemoveOldColorID();
			entities[i].isEmpty = false;
		}
	}

	public void SubmitHighScore()
	{
		GameData.Instance().SubmitHighScore(gameType, isBomb);
	}

	public void ClearSaveGame()
	{
		SaveGameManager.instance.ClearData(isBomb);
	}

	public void SaveGame()
	{
		Entity[,] board = boardEntity.boardEntities.board;
		SaveGameManager.instance.SaveGame(isBomb, table, board);
	}

	public float GetTimeDelay(int x, int y)
	{
		if (timeDelayAnim != null)
		{
			return timeDelayAnim[x, y];
		}
		return 0f;
	}

	public void ContinueAfterAds()
	{
		m_IsCanContinue = false;
		m_IsNewMatch = false;
		ServicesManager.DataNormal().SetBool(m_IsCanContinueKey, m_IsCanContinue);
		m_State = GameState.Playing;
		ResetBoardColor();
		Entity[,] board = boardEntity.boardEntities.board;
		Pool pool = Pools.pool;
		List<MyVector2> listClearFourRow = triangeTable.GetListClearFourRow(isTop: false);
		for (int i = 0; i < listClearFourRow.Count; i++)
		{
			MyVector2 myVector = listClearFourRow[i];
			int x = myVector.x;
			MyVector2 myVector2 = listClearFourRow[i];
			int y = myVector2.y;
			Entity entity = board[x, y];
			if (entity != null)
			{
				Entity entity2 = pool.CreateEntity();
				entity2.AddPrefab(entity.prefab.gameObject);
				entity2.AddParent(entity.parent.data);
				entity2.AddGrid(entity.grid.row, entity.grid.col);
				entity2.AddPosition(entity.position.x, entity.position.y);
				entity2.AddBox(entity.box.width, entity.box.height);
				entity2.AddSprite(entity.sprite.data);
				entity2.AddSortingLayer(entity.spriteRenderer.data.sortingOrder + 1);
				entity2.AddColor(entity.color.data);
				entity2.AddBonus(0);
				if (entity.hasScaleDirect)
				{
					entity2.AddScaleDirect(entity.scaleDirect.x, entity.scaleDirect.y);
				}
			}
		}
		table.AfterSync();
		SyncTableToBoard(isNeedCheckClear: false);
		StartCoroutine(IE_BlockDelay());
	}

	private IEnumerator IE_BlockDelay()
	{
		isCanDrag = false;
		yield return new WaitForSeconds(1f);
		isCanDrag = true;
	}

	private void DecreaseMoneyUseSkill(bool isNew3)
	{
		int num;
		if (isNew3)
		{
			num = GameData.Instance().GetCostNew3Shapes();
			Analytic.Instance.LogUseSkillNew3(num);
			GameData.Instance().SetCostNew3Shapes(num * 2);
		}
		else
		{
			num = GameData.Instance().GetCostUndo();
			Analytic.Instance.LogUseSkillUndo(num);
			GameData.Instance().SetCostUndo(num * 2);
		}
		GameData.Instance().AddMoney(-num);
		Singleton<UIManager>.instance.ReloadCostSkill();
	}

	public void SetOpenEvent(bool isOpen)
	{
		eventXmasController.SetOpenEvent(isOpen);
	}

	private void CreateBoardVideoAd()
	{
		if (SetupType == SetUpType.Legendary)
		{
			UnityEngine.Debug.Log("Setup legend player");
			triangeTable.Reset();
			triangeTable.SetTableLegendPlayer();
			SyncTableToBoard(isNeedCheckClear: false, isSetup: true);
		}
		else if (SetupType == SetUpType.CantDecide)
		{
			UnityEngine.Debug.Log("Setup can't decide");
			triangeTable.Reset();
			triangeTable.SetTableCantDecide();
			SyncTableToBoard(isNeedCheckClear: false, isSetup: true);
		}
	}
}
