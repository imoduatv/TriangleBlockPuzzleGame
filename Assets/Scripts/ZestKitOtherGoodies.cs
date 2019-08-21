using Prime31.ZestKit;
using UnityEngine;

public class ZestKitOtherGoodies : MonoBehaviour
{
	public Transform cube;

	private float _duration = 0.5f;

	private TransformSpringTween _springTween;

	public float wackyDoodleWidth
	{
		get
		{
			Vector3 localScale = cube.localScale;
			return localScale.x;
		}
		set
		{
			Transform transform = cube;
			Vector3 localScale = cube.localScale;
			float y = localScale.y;
			Vector3 localScale2 = cube.localScale;
			transform.localScale = new Vector3(value, y, localScale2.z);
		}
	}

	private void OnGUI()
	{
		DemoGUIHelpers.setupGUIButtons();
		if (_springTween == null)
		{
			_duration = DemoGUIHelpers.durationSlider(_duration);
			if (GUILayout.Button("Custom Property Tween (wackyDoodleWidth)"))
			{
				PropertyTweens.floatPropertyTo(this, "wackyDoodleWidth", 6f, _duration).setFrom(1f).setLoops(LoopType.PingPong)
					.start();
			}
			if (GUILayout.Button("Position via Property Tween"))
			{
				PropertyTweens.vector3PropertyTo(cube, "position", new Vector3(5f, 5f, 5f), _duration).setLoops(LoopType.PingPong).start();
			}
			if (GUILayout.Button("Tween Party (color, position, scale and rotation)"))
			{
				TweenParty tweenParty = new TweenParty(_duration);
				tweenParty.addTween(cube.GetComponent<Renderer>().material.ZKcolorTo(Color.black)).addTween(cube.ZKpositionTo(new Vector3(7f, 4f))).addTween(cube.ZKlocalScaleTo(new Vector3(1f, 4f)))
					.addTween(cube.ZKrotationTo(Quaternion.AngleAxis(180f, Vector3.one)))
					.setLoops(LoopType.PingPong)
					.start();
			}
			if (GUILayout.Button("Tween Chain (same props as the party)"))
			{
				TweenChain tweenChain = new TweenChain();
				tweenChain.appendTween(cube.GetComponent<Renderer>().material.ZKcolorTo(Color.black, _duration).setLoops(LoopType.PingPong)).appendTween(cube.ZKpositionTo(new Vector3(7f, 4f), _duration).setLoops(LoopType.PingPong)).appendTween(cube.ZKlocalScaleTo(new Vector3(1f, 4f), _duration).setLoops(LoopType.PingPong))
					.appendTween(cube.ZKrotationTo(Quaternion.AngleAxis(180f, Vector3.one), _duration).setLoops(LoopType.PingPong))
					.start();
			}
			if (GUILayout.Button("Chaining Tweens Directly (same props as the party)"))
			{
				cube.GetComponent<Renderer>().material.ZKcolorTo(Color.black, _duration).setLoops(LoopType.PingPong).setNextTween(cube.ZKpositionTo(new Vector3(7f, 4f), _duration).setLoops(LoopType.PingPong).setNextTween(cube.ZKlocalScaleTo(new Vector3(1f, 4f), _duration).setLoops(LoopType.PingPong).setNextTween(cube.ZKrotationTo(Quaternion.AngleAxis(180f, Vector3.one), _duration).setLoops(LoopType.PingPong))))
					.start();
			}
			GUILayout.Space(10f);
			if (GUILayout.Button("Start Spring Position"))
			{
				_springTween = new TransformSpringTween(cube, TransformTargetType.Position, cube.position);
			}
			if (GUILayout.Button("Start Spring Position (overdamped)"))
			{
				_springTween = new TransformSpringTween(cube, TransformTargetType.Position, cube.position);
				_springTween.dampingRatio = 1.5f;
				_springTween.angularFrequency = 20f;
			}
			if (GUILayout.Button("Start Spring Scale"))
			{
				_springTween = new TransformSpringTween(cube, TransformTargetType.LocalScale, cube.localScale);
			}
			GUILayout.Space(10f);
			if (GUILayout.Button("Run Action Every 1s After 2s Delay"))
			{
				ActionTask.every(2f, 1f, this, delegate(ActionTask task)
				{
					(task.context as ZestKitOtherGoodies).methodCalledForDemonstrationPurposes();
				});
			}
			if (GUILayout.Button("ActionTask Interoperability"))
			{
				UnityEngine.Debug.Log("The Story: An ActionTask with a 2s delay will be created with a continueWith ActionTask appended to it that will tick every 0.3s for 2s. The original ActionTask will have a waitFor called that is an ActionTask with a 1s delay. Follow?");
				UnityEngine.Debug.Log("--- current time: " + Time.time);
				ActionTask.afterDelay(2f, this, delegate
				{
					UnityEngine.Debug.Log("--- root task ticked: " + Time.time);
				}).continueWith(ActionTask.create(this, delegate(ActionTask task)
				{
					UnityEngine.Debug.Log("+++ continueWith task elapsed time: " + task.elapsedTime);
					if (task.elapsedTime > 2f)
					{
						task.stop(runContinueWithTaskIfPresent: true);
					}
				}).setDelay(1f).setRepeats(0.3f)).waitFor(ActionTask.afterDelay(1f, this, delegate
				{
					UnityEngine.Debug.Log("--- waitFor ticked: " + Time.time);
				}));
			}
			DemoGUIHelpers.easeTypesGUI();
		}
		else
		{
			GUILayout.Label("While the spring tween is active the cube will spring to\nwhichever location you click or scale x/y to that location\nif you chose a scale spring. The sliders below let you tweak\nthe spring contants.\n\nFor the scale tween, try clicking places on a horizontal or vertical\naxis to get a feel for how it works.");
			springSliders();
			string arg = (_springTween.targetType != 0) ? "Spring scale to:" : "Spring position to:";
			Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			string text = $"{arg}\nx: {vector.x:F1}\ny: {vector.y:F1}";
			Vector3 mousePosition = UnityEngine.Input.mousePosition;
			float x = mousePosition.x;
			float num = Screen.height;
			Vector3 mousePosition2 = UnityEngine.Input.mousePosition;
			GUI.Label(new Rect(x, num - mousePosition2.y - 50f, 130f, 50f), text);
			if (GUILayout.Button("Stop Spring"))
			{
				_springTween.stop();
				_springTween = null;
				cube.position = new Vector3(-1f, -2f);
				cube.localScale = Vector3.one;
			}
		}
	}

	private void methodCalledForDemonstrationPurposes()
	{
		UnityEngine.Debug.Log("methodCalledForDemonstrationPurposes was called at " + Time.time);
	}

	private void Update()
	{
		if (_springTween != null && Input.GetMouseButtonDown(0))
		{
			Vector3 targetValue = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			targetValue.z = 1f;
			_springTween.setTargetValue(targetValue);
		}
	}

	private void springSliders()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("Damping Ratio", GUILayout.Width(110f));
		GUI.skin.horizontalSlider.margin = new RectOffset(4, 4, 10, 4);
		_springTween.dampingRatio = GUILayout.HorizontalSlider(_springTween.dampingRatio, 0.1f, 0.75f, GUILayout.ExpandWidth(expand: true));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Angular Frequency", GUILayout.Width(110f));
		GUI.skin.horizontalSlider.margin = new RectOffset(4, 4, 10, 4);
		_springTween.angularFrequency = GUILayout.HorizontalSlider(_springTween.angularFrequency, 1f, 37.6991119f, GUILayout.ExpandWidth(expand: true));
		GUILayout.EndHorizontal();
	}
}
