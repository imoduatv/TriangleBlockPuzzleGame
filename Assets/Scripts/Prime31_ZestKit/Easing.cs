using UnityEngine;

namespace Prime31.ZestKit
{
	public static class Easing
	{
		public static class Linear
		{
			public static float EaseNone(float t, float d)
			{
				return t / d;
			}
		}

		public static class Quadratic
		{
			public static float EaseIn(float t, float d)
			{
				return (t /= d) * t;
			}

			public static float EaseOut(float t, float d)
			{
				return -1f * (t /= d) * (t - 2f);
			}

			public static float EaseInOut(float t, float d)
			{
				if ((t /= d / 2f) < 1f)
				{
					return 0.5f * t * t;
				}
				return -0.5f * ((t -= 1f) * (t - 2f) - 1f);
			}
		}

		public static class Back
		{
			public static float EaseIn(float t, float d)
			{
				return (t /= d) * t * (2.70158f * t - 1.70158f);
			}

			public static float EaseOut(float t, float d)
			{
				return (t = t / d - 1f) * t * (2.70158f * t + 1.70158f) + 1f;
			}

			public static float EaseInOut(float t, float d)
			{
				float num = 1.70158f;
				if ((t /= d / 2f) < 1f)
				{
					return 0.5f * (t * t * (((num *= 1.525f) + 1f) * t - num));
				}
				return 0.5f * ((t -= 2f) * t * (((num *= 1.525f) + 1f) * t + num) + 2f);
			}
		}

		public static class Bounce
		{
			public static float EaseOut(float t, float d)
			{
				if ((double)(t /= d) < 0.36363636363636365)
				{
					return 7.5625f * t * t;
				}
				if ((double)t < 0.72727272727272729)
				{
					return 7.5625f * (t -= 0.545454562f) * t + 0.75f;
				}
				if ((double)t < 0.90909090909090906)
				{
					return 7.5625f * (t -= 0.8181818f) * t + 0.9375f;
				}
				return 7.5625f * (t -= 21f / 22f) * t + 63f / 64f;
			}

			public static float EaseIn(float t, float d)
			{
				return 1f - EaseOut(d - t, d);
			}

			public static float EaseInOut(float t, float d)
			{
				if (t < d / 2f)
				{
					return EaseIn(t * 2f, d) * 0.5f;
				}
				return EaseOut(t * 2f - d, d) * 0.5f + 0.5f;
			}
		}

		public static class Circular
		{
			public static float EaseIn(float t, float d)
			{
				return 0f - (Mathf.Sqrt(1f - (t /= d) * t) - 1f);
			}

			public static float EaseOut(float t, float d)
			{
				return Mathf.Sqrt(1f - (t = t / d - 1f) * t);
			}

			public static float EaseInOut(float t, float d)
			{
				if ((t /= d / 2f) < 1f)
				{
					return -0.5f * (Mathf.Sqrt(1f - t * t) - 1f);
				}
				return 0.5f * (Mathf.Sqrt(1f - (t -= 2f) * t) + 1f);
			}
		}

		public static class Cubic
		{
			public static float EaseIn(float t, float d)
			{
				return (t /= d) * t * t;
			}

			public static float EaseOut(float t, float d)
			{
				return (t = t / d - 1f) * t * t + 1f;
			}

			public static float EaseInOut(float t, float d)
			{
				if ((t /= d / 2f) < 1f)
				{
					return 0.5f * t * t * t;
				}
				return 0.5f * ((t -= 2f) * t * t + 2f);
			}
		}

		public class Elastic
		{
			public static float EaseIn(float t, float d)
			{
				if (t == 0f)
				{
					return 0f;
				}
				if ((t /= d) == 1f)
				{
					return 1f;
				}
				float num = d * 0.3f;
				float num2 = num / 4f;
				return 0f - 1f * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t * d - num2) * 6.28318548f / num);
			}

			public static float EaseOut(float t, float d)
			{
				if (t == 0f)
				{
					return 0f;
				}
				if ((t /= d) == 1f)
				{
					return 1f;
				}
				float num = d * 0.3f;
				float num2 = num / 4f;
				return 1f * Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * d - num2) * 6.28318548f / num) + 1f;
			}

			public static float EaseInOut(float t, float d)
			{
				if (t == 0f)
				{
					return 0f;
				}
				if ((t /= d / 2f) == 2f)
				{
					return 1f;
				}
				float num = d * 0.450000018f;
				float num2 = num / 4f;
				if (t < 1f)
				{
					return -0.5f * (Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t * d - num2) * 6.28318548f / num));
				}
				return Mathf.Pow(2f, -10f * (t -= 1f)) * Mathf.Sin((t * d - num2) * 6.28318548f / num) * 0.5f + 1f;
			}

			public static float Punch(float t, float d)
			{
				if (t == 0f)
				{
					return 0f;
				}
				if ((t /= d) == 1f)
				{
					return 0f;
				}
				return Mathf.Pow(2f, -10f * t) * Mathf.Sin(t * 6.28318548f / 0.3f);
			}
		}

		public static class Exponential
		{
			public static float EaseIn(float t, float d)
			{
				return (t != 0f) ? Mathf.Pow(2f, 10f * (t / d - 1f)) : 0f;
			}

			public static float EaseOut(float t, float d)
			{
				return (t != d) ? (0f - Mathf.Pow(2f, -10f * t / d) + 1f) : 1f;
			}

			public static float EaseInOut(float t, float d)
			{
				if (t == 0f)
				{
					return 0f;
				}
				if (t == d)
				{
					return 1f;
				}
				if ((t /= d / 2f) < 1f)
				{
					return 0.5f * Mathf.Pow(2f, 10f * (t - 1f));
				}
				return 0.5f * (0f - Mathf.Pow(2f, -10f * (t -= 1f)) + 2f);
			}
		}

		public static class Quartic
		{
			public static float EaseIn(float t, float d)
			{
				return (t /= d) * t * t * t;
			}

			public static float EaseOut(float t, float d)
			{
				return -1f * ((t = t / d - 1f) * t * t * t - 1f);
			}

			public static float EaseInOut(float t, float d)
			{
				t /= d / 2f;
				if (t < 1f)
				{
					return 0.5f * t * t * t * t;
				}
				t -= 2f;
				return -0.5f * (t * t * t * t - 2f);
			}
		}

		public static class Quintic
		{
			public static float EaseIn(float t, float d)
			{
				return (t /= d) * t * t * t * t;
			}

			public static float EaseOut(float t, float d)
			{
				return (t = t / d - 1f) * t * t * t * t + 1f;
			}

			public static float EaseInOut(float t, float d)
			{
				if ((t /= d / 2f) < 1f)
				{
					return 0.5f * t * t * t * t * t;
				}
				return 0.5f * ((t -= 2f) * t * t * t * t + 2f);
			}
		}

		public static class Sinusoidal
		{
			public static float EaseIn(float t, float d)
			{
				return -1f * Mathf.Cos(t / d * 1.57079637f) + 1f;
			}

			public static float EaseOut(float t, float d)
			{
				return Mathf.Sin(t / d * 1.57079637f);
			}

			public static float EaseInOut(float t, float d)
			{
				return -0.5f * (Mathf.Cos(3.14159274f * t / d) - 1f);
			}
		}
	}
}
