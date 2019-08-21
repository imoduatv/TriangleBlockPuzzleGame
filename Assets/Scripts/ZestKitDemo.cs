using Prime31.ZestKit;
using UnityEngine;

public class ZestKitDemo : MonoBehaviour
{
	public Transform cube;

	public AnimationCurve curve;

	public RectTransform panel;

	private float _duration = 0.5f;

	private void OnGUI()
	{
		DemoGUIHelpers.setupGUIButtons();
		_duration = DemoGUIHelpers.durationSlider(_duration);
		if (GUILayout.Button("Position Tween with 2 PingPong Loops"))
		{
			cube.ZKpositionTo(new Vector3(9f, 5f), _duration).setLoops(LoopType.PingPong, 2).setLoopCompletionHandler(delegate
			{
				UnityEngine.Debug.Log("Loop complete");
			})
				.setCompletionHandler(delegate
				{
					UnityEngine.Debug.Log("Tween complete");
				})
				.start();
		}
		if (GUILayout.Button("Relative Position Tween"))
		{
			cube.ZKpositionTo(new Vector3(1f, 0f), _duration).setIsRelative().start();
		}
		if (GUILayout.Button("AnimationCurve for Easing Scale"))
		{
			cube.ZKlocalScaleTo(new Vector3(3f, 3f, 3f), _duration).setAnimationCurve(curve).start();
		}
		if (GUILayout.Button("Scale back to 1"))
		{
			cube.ZKlocalScaleTo(new Vector3(1f, 1f, 1f), _duration).start();
		}
		if (GUILayout.Button("Punch Scale to 3"))
		{
			cube.ZKlocalScaleTo(new Vector3(3f, 3f, 3f), _duration).setEaseType(EaseType.Punch).start();
		}
		if (GUILayout.Button("Rotation to 270, 0, 0"))
		{
			cube.ZKlocalEulersTo(new Vector3(270f, 0f), _duration).start();
		}
		if (GUILayout.Button("Rotation to 310, 90 -> localScale"))
		{
			cube.ZKlocalEulersTo(new Vector3(310f, 90f), _duration).setNextTween(cube.ZKlocalScaleTo(new Vector3(2f, 4f, 2f)).setLoops(LoopType.PingPong)).start();
		}
		if (GUILayout.Button("Rotation by 0, 720, 360 (relative tween)"))
		{
			cube.ZKlocalEulersTo(new Vector3(0f, 720f, 360f), _duration).setIsRelative().start();
		}
		if (GUILayout.Button("Material Color to Yellow with PingPong Loop"))
		{
			cube.GetComponent<Renderer>().material.ZKcolorTo(Color.yellow, _duration).setLoops(LoopType.PingPong).start();
		}
		if (GUILayout.Button("RectTransform Panel Position Tween"))
		{
			panel.ZKanchoredPositionTo(new Vector2((float)(-Screen.width) * 0.5f, (float)Screen.height * 0.5f), _duration).setLoops(LoopType.PingPong).start();
		}
		if (GUILayout.Button("RectTransform Button Position (relative tweens)"))
		{
			RectTransform self = panel.GetChild(0) as RectTransform;
			RectTransform self2 = panel.GetChild(1) as RectTransform;
			self.ZKanchoredPositionTo(new Vector2(0f, panel.rect.height * 0.6f), _duration).setIsRelative().setLoops(LoopType.PingPong)
				.start();
			self2.ZKanchoredPositionTo(new Vector2(0f, panel.rect.height * 0.6f), _duration).setIsRelative().setLoops(LoopType.PingPong)
				.setDelay(_duration * 0.5f)
				.start();
		}
		if (GUILayout.Button("Camera Shake"))
		{
			new CameraShakeTween(Camera.main).start();
		}
		DemoGUIHelpers.easeTypesGUI();
	}
}
