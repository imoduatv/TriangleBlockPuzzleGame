using Prime31.ZestKit;
using System;
using UnityEngine;

public class ZestKitStressTest : MonoBehaviour
{
	public class Perlin
	{
		private const int B = 256;

		private const int BM = 255;

		private const int N = 4096;

		private int[] p = new int[514];

		private float[,] g3 = new float[514, 3];

		private float[,] g2 = new float[514, 2];

		private float[] g1 = new float[514];

		public Perlin()
		{
			System.Random random = new System.Random();
			int i;
			for (i = 0; i < 256; i++)
			{
				p[i] = i;
				g1[i] = (float)(random.Next(512) - 256) / 256f;
				for (int j = 0; j < 2; j++)
				{
					g2[i, j] = (float)(random.Next(512) - 256) / 256f;
				}
				normalize2(ref g2[i, 0], ref g2[i, 1]);
				for (int j = 0; j < 3; j++)
				{
					g3[i, j] = (float)(random.Next(512) - 256) / 256f;
				}
				normalize3(ref g3[i, 0], ref g3[i, 1], ref g3[i, 2]);
			}
			while (--i != 0)
			{
				int num = p[i];
				int j;
				p[i] = p[j = random.Next(256)];
				p[j] = num;
			}
			for (i = 0; i < 258; i++)
			{
				p[256 + i] = p[i];
				g1[256 + i] = g1[i];
				for (int j = 0; j < 2; j++)
				{
					g2[256 + i, j] = g2[i, j];
				}
				for (int j = 0; j < 3; j++)
				{
					g3[256 + i, j] = g3[i, j];
				}
			}
		}

		private float s_curve(float t)
		{
			return t * t * (3f - 2f * t);
		}

		private void setup(float value, out int b0, out int b1, out float r0, out float r1)
		{
			float num = value + 4096f;
			b0 = ((int)num & 0xFF);
			b1 = ((b0 + 1) & 0xFF);
			r0 = num - (float)(int)num;
			r1 = r0 - 1f;
		}

		private float at2(float rx, float ry, float x, float y)
		{
			return rx * x + ry * y;
		}

		private float at3(float rx, float ry, float rz, float x, float y, float z)
		{
			return rx * x + ry * y + rz * z;
		}

		public float Noise(float arg)
		{
			setup(arg, out int b, out int b2, out float r, out float r2);
			float a = s_curve(r);
			float b3 = r * g1[p[b]];
			float t = r2 * g1[p[b2]];
			return Mathf.Lerp(a, b3, t);
		}

		private void normalize2(ref float x, ref float y)
		{
			float num = Mathf.Sqrt(x * x + y * y);
			x = y / num;
			y /= num;
		}

		private void normalize3(ref float x, ref float y, ref float z)
		{
			float num = Mathf.Sqrt(x * x + y * y + z * z);
			x = y / num;
			y /= num;
			z /= num;
		}
	}

	private const int _totalCubes = 2500;

	private Transform[] _cubes = new Transform[2500];

	private Perlin _perlinNoiseGenerator = new Perlin();

	private void Awake()
	{
		Application.targetFrameRate = 60;
		for (int i = 0; i < _cubes.Length; i++)
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
			UnityEngine.Object.Destroy(gameObject.GetComponent<BoxCollider>());
			Transform transform = gameObject.transform;
			float x = (float)i * 0.1f - 40f;
			Vector3 position = gameObject.transform.position;
			transform.position = new Vector3(x, position.y - 10f, i % 10);
			_cubes[i] = gameObject.transform;
		}
	}

	private void Start()
	{
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < _cubes.Length; i++)
		{
			float arg = 4f;
			float arg2 = UnityEngine.Random.Range(-2f, 2f) * 2f;
			float arg3 = UnityEngine.Random.Range(-2f, 2f) * 3f;
			float num = _perlinNoiseGenerator.Noise(arg) * 100f;
			Vector3 position = _cubes[i].position;
			zero.x = num + position.x;
			float num2 = _perlinNoiseGenerator.Noise(arg2) * 100f;
			Vector3 position2 = _cubes[i].position;
			zero.y = num2 + position2.y;
			float num3 = _perlinNoiseGenerator.Noise(arg3) * 100f;
			Vector3 position3 = _cubes[i].position;
			zero.z = num3 + position3.z;
			_cubes[i].ZKpositionTo(zero, 1f).setLoops(LoopType.PingPong, 99999, 0.1f).start();
		}
	}
}
