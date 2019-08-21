using Root;
using UnityEngine;

public class ServicesConfig : ScriptableObject
{
	[Header("Bundle Version")]
	public int bundleVersion;

	[Header("Analytic AB Test")]
	public ABTestConfig[] abTestConfigs;
}
