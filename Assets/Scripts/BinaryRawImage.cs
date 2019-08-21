using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BinaryRawImage : MonoBehaviour
{
	[Tooltip("imageABC.png -> imageABC.bytes")]
	public TextAsset m_TextureBytes;

	private RawImage m_RawImage;

	private Image m_Image;

	private static Dictionary<int, Texture2D> m_Textures = new Dictionary<int, Texture2D>();

	private static Dictionary<int, Sprite> m_Sprites = new Dictionary<int, Sprite>();

	private void Start()
	{
		m_RawImage = GetComponent<RawImage>();
		m_Image = GetComponent<Image>();
		if (m_RawImage != null)
		{
			Texture2D texture2D = GetTexture2D(m_TextureBytes);
			if (texture2D != null)
			{
				m_RawImage.texture = texture2D;
			}
		}
		else
		{
			Sprite sprite = GetSprite(m_TextureBytes);
			if (sprite != null)
			{
				m_Image.sprite = sprite;
			}
		}
	}

	public static Texture2D GetTexture2D(TextAsset textAsset)
	{
		if (textAsset == null)
		{
			return null;
		}
		int instanceID = textAsset.GetInstanceID();
		if (m_Textures.ContainsKey(instanceID))
		{
			return m_Textures[instanceID];
		}
		Texture2D texture2D = new Texture2D(1, 1, TextureFormat.RGB24,  false);
		texture2D.LoadImage(textAsset.bytes, markNonReadable: false);
		int width = Screen.width;
		int height = Screen.height;
		if (texture2D.width > width || texture2D.height > height)
		{
			float num = (float)width / (float)texture2D.width;
			float num2 = (float)height / (float)texture2D.height;
			texture2D = ((!(num < num2)) ? Resize(texture2D, (int)((float)texture2D.width * num2), (int)((float)texture2D.height * num2)) : Resize(texture2D, (int)((float)texture2D.width * num), (int)((float)texture2D.height * num)));
		}
		m_Textures[instanceID] = texture2D;
		return texture2D;
	}

	public static Sprite GetSprite(TextAsset textAsset)
	{
		if (textAsset == null)
		{
			return null;
		}
		int instanceID = textAsset.GetInstanceID();
		if (m_Sprites.ContainsKey(instanceID))
		{
			return m_Sprites[instanceID];
		}
		Texture2D texture2D = GetTexture2D(textAsset);
		Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100f);
		m_Sprites[instanceID] = sprite;
		return sprite;
	}

	public static void ClearCache(TextAsset textAsset)
	{
		if (!(textAsset == null))
		{
			int instanceID = textAsset.GetInstanceID();
			if (m_Sprites != null && m_Sprites.ContainsKey(instanceID))
			{
				UnityEngine.Object.Destroy(m_Sprites[instanceID]);
				m_Sprites.Remove(instanceID);
			}
			if (m_Textures != null && m_Textures.ContainsKey(instanceID))
			{
				UnityEngine.Object.Destroy(m_Textures[instanceID]);
				m_Textures.Remove(instanceID);
			}
		}
	}

	public static Texture2D Resize(Texture2D source, int width, int height)
	{
		RenderTexture renderTexture2 = RenderTexture.active = new RenderTexture(width, height, 32);
		GL.PushMatrix();
		GL.LoadPixelMatrix(0f, width, height, 0f);
		Graphics.DrawTexture(new Rect(0f, 0f, width, height), source);
		GL.PopMatrix();
		Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGB24,  false);
		texture2D.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
		texture2D.Apply();
		RenderTexture.active = null;
		UnityEngine.Object.DestroyImmediate(source, allowDestroyingAssets: true);
		return texture2D;
	}
}
