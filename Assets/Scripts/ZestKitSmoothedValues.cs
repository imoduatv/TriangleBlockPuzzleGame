using Prime31.ZestKit;
using UnityEngine;

public class ZestKitSmoothedValues : MonoBehaviour
{
	public Transform cubeTransform;

	private SmoothedFloat _smoothedFloat;

	private SmoothedVector3 _smoothedVector3;

	private void Awake()
	{
		_smoothedFloat = new SmoothedFloat(0f, 2f);
		_smoothedVector3 = new SmoothedVector3(cubeTransform.position, 0.5f);
	}

	private void Update()
	{
		_smoothedFloat.easeType = ZestKit.defaultEaseType;
		_smoothedVector3.easeType = ZestKit.defaultEaseType;
		if (Input.GetMouseButtonDown(0))
		{
			Camera main = Camera.main;
			Vector3 mousePosition = UnityEngine.Input.mousePosition;
			float x = mousePosition.x;
			Vector3 mousePosition2 = UnityEngine.Input.mousePosition;
			Vector3 toValue = main.ScreenToWorldPoint(new Vector3(x, mousePosition2.y, 10f));
			Vector3 position = cubeTransform.position;
			toValue.z = position.z;
			_smoothedVector3.setToValue(toValue);
		}
		cubeTransform.position = _smoothedVector3.value;
	}

	private void OnGUI()
	{
		DemoGUIHelpers.setupGUIButtons();
		GUILayout.Label("Click anywhere to move the cube via a SmoothedVector3");
		GUILayout.Space(30f);
		GUILayout.Label("Click the buttons below the slider to use\na SmoothedFloat to change the slider value");
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Set To Value to 10"))
		{
			_smoothedFloat.setToValue(10f);
		}
		if (GUILayout.Button("Set To Value to -10"))
		{
			_smoothedFloat.setToValue(-10f);
		}
		GUILayout.EndHorizontal();
		GUILayout.HorizontalSlider(_smoothedFloat.value, -10f, 10f, GUILayout.Width(250f));
		DemoGUIHelpers.easeTypesGUI();
	}
}
