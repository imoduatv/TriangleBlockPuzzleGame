using UnityEngine;

public class FirebaseRemoteData : ScriptableObject
{
	[Header("Android")]
	public FirebaseConfigFloat TimeFullAd_Android;

	public FirebaseConfigArrayInt Difficulty_Android;

	[Header("IOS")]
	public FirebaseConfigFloat TimeFullAd_IOS;

	public FirebaseConfigArrayInt Difficulty_IOS;
}
