namespace SA.IOSNative.Gestures
{
	public class ForceInfo
	{
		private float _Force;

		private float _MaxForce;

		public float Force => _Force;

		public float MaxForce => _MaxForce;

		public ForceInfo(float force, float maxForce)
		{
			_Force = force;
			_MaxForce = maxForce;
		}
	}
}
