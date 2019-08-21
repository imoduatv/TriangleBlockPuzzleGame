using Prime31.ZestKit;
using UnityEngine;

public class ZestKitSplineDemo : MonoBehaviour
{
	public Transform quad;

	private float _duration = 2.5f;

	private void OnGUI()
	{
		DemoGUIHelpers.setupGUIButtons();
		_duration = DemoGUIHelpers.durationSlider(_duration, 5f);
		DemoGUIHelpers.easeTypesGUI();
		GUILayout.Label("The splines used in this scene are on the\n*DummySpline GameObjects so you can\nhave a look at them.");
		GUILayout.Label("Just select the GameObject and the gizmos\nwill be drawn in the scene view.");
		GUILayout.Space(20f);
		if (GUILayout.Button("Figure Eight Spline Tween (relative)"))
		{
			Spline spline = new Spline("figureEight");
			spline.closePath();
			new SplineTween(quad, spline, _duration).setIsRelative().start();
		}
		if (GUILayout.Button("Figure Eight Spline Tween (absolute)"))
		{
			Spline spline2 = new Spline("figureEight");
			spline2.closePath();
			new SplineTween(quad, spline2, _duration).start();
		}
		if (GUILayout.Button("Cicle Position Tween (relative with PingPong)"))
		{
			Spline spline3 = new Spline("circle", useBezierIfPossible: true);
			spline3.closePath();
			new SplineTween(quad, spline3, _duration).setIsRelative().setLoops(LoopType.PingPong).start();
		}
		if (GUILayout.Button("DemoRoute Tween (relative with PingPong)"))
		{
			Spline spline4 = new Spline("demoRoute", useBezierIfPossible: true);
			spline4.closePath();
			new SplineTween(quad, spline4, _duration).setIsRelative().setLoops(LoopType.PingPong).start();
		}
		if (GUILayout.Button("Runtime Spline (relative with PingPong)"))
		{
			Vector3[] nodes = new Vector3[6]
			{
				new Vector3(0f, 0f),
				new Vector3(0f, 0f),
				new Vector3(4f, 4f, 4f),
				new Vector3(-4f, 5f, 6f),
				new Vector3(-2f, 2f, 0f),
				new Vector3(0f, 0f)
			};
			Spline spline5 = new Spline(nodes);
			spline5.closePath();
			new SplineTween(quad, spline5, _duration).setIsRelative().setLoops(LoopType.PingPong).start();
		}
	}
}
