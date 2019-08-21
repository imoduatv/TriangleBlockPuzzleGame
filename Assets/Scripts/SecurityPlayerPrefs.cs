using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class SecurityPlayerPrefs
{
	private static string _saltForKey;

	private static byte[] _keys;

	private static byte[] _iv;

	private static int keySize;

	private static int blockSize;

	private static int _hashLen;

	static SecurityPlayerPrefs()
	{
		keySize = 256;
		blockSize = 128;
		_hashLen = 32;
		byte[] salt = new byte[8]
		{
			25,
			36,
			77,
			51,
			43,
			14,
			75,
			93
		};
		string password = "5b6fcb4aaa0a42acae649eba45a506ec";
		string password2 = "2e327725789841b5bb5c706d6b2ad897";
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, 1000);
		_saltForKey = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(blockSize / 8));
		Rfc2898DeriveBytes rfc2898DeriveBytes2 = new Rfc2898DeriveBytes(password2, salt, 1000);
		_keys = rfc2898DeriveBytes2.GetBytes(keySize / 8);
		_iv = rfc2898DeriveBytes2.GetBytes(blockSize / 8);
	}

	public static string MakeHash(string original)
	{
		using (MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider())
		{
			byte[] bytes = Encoding.UTF8.GetBytes(original);
			byte[] array = mD5CryptoServiceProvider.ComputeHash(bytes);
			string text = string.Empty;
			for (int i = 0; i < array.Length; i++)
			{
				text += array[i].ToString("x2");
			}
			return text;
		}
	}

	public static byte[] Encrypt(byte[] bytesToBeEncrypted)
	{
		using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
		{
			rijndaelManaged.KeySize = keySize;
			rijndaelManaged.BlockSize = blockSize;
			rijndaelManaged.Key = _keys;
			rijndaelManaged.IV = _iv;
			rijndaelManaged.Mode = CipherMode.CBC;
			rijndaelManaged.Padding = PaddingMode.PKCS7;
			using (ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor())
			{
				return cryptoTransform.TransformFinalBlock(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
			}
		}
	}

	public static byte[] Decrypt(byte[] bytesToBeDecrypted)
	{
		using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
		{
			rijndaelManaged.KeySize = keySize;
			rijndaelManaged.BlockSize = blockSize;
			rijndaelManaged.Key = _keys;
			rijndaelManaged.IV = _iv;
			rijndaelManaged.Mode = CipherMode.CBC;
			rijndaelManaged.Padding = PaddingMode.PKCS7;
			using (ICryptoTransform cryptoTransform = rijndaelManaged.CreateDecryptor())
			{
				return cryptoTransform.TransformFinalBlock(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
			}
		}
	}

	public static string Encrypt(string input)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(input);
		byte[] inArray = Encrypt(bytes);
		return Convert.ToBase64String(inArray);
	}

	public static string Decrypt(string input)
	{
		byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
		byte[] bytes = Decrypt(bytesToBeDecrypted);
		return Encoding.UTF8.GetString(bytes);
	}

	private static void SetSecurityValue(string key, string value)
	{
		string key2 = MakeHash(key + _saltForKey);
		string value2 = Encrypt(value + MakeHash(value));
		PlayerPrefs.SetString(key2, value2);
	}

	private static string GetSecurityValue(string key)
	{
		string key2 = MakeHash(key + _saltForKey);
		string @string = PlayerPrefs.GetString(key2);
		if (string.IsNullOrEmpty(@string))
		{
			return string.Empty;
		}
		string text = Decrypt(@string);
		if (_hashLen > text.Length)
		{
			return string.Empty;
		}
		string text2 = text.Substring(0, text.Length - _hashLen);
		string b = text.Substring(text.Length - _hashLen);
		if (MakeHash(text2) != b)
		{
			return string.Empty;
		}
		return text2;
	}

	public static void DeleteKey(string key)
	{
		PlayerPrefs.DeleteKey(MakeHash(key + _saltForKey));
	}

	public static void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
	}

	public static void Save()
	{
		PlayerPrefs.Save();
	}

	public static void SetInt(string key, int value)
	{
		SetSecurityValue(key, value.ToString());
	}

	public static void SetLong(string key, long value)
	{
		SetSecurityValue(key, value.ToString());
	}

	public static void SetFloat(string key, float value)
	{
		SetSecurityValue(key, value.ToString());
	}

	public static void SetString(string key, string value)
	{
		SetSecurityValue(key, value);
	}

	public static int GetInt(string key, int defaultValue)
	{
		string securityValue = GetSecurityValue(key);
		if (string.IsNullOrEmpty(securityValue))
		{
			return defaultValue;
		}
		int result = defaultValue;
		if (!int.TryParse(securityValue, out result))
		{
			return defaultValue;
		}
		return result;
	}

	public static long GetLong(string key, long defaultValue)
	{
		string securityValue = GetSecurityValue(key);
		if (string.IsNullOrEmpty(securityValue))
		{
			return defaultValue;
		}
		long result = defaultValue;
		if (!long.TryParse(securityValue, out result))
		{
			return defaultValue;
		}
		return result;
	}

	public static float GetFloat(string key, float defaultValue)
	{
		string securityValue = GetSecurityValue(key);
		if (string.IsNullOrEmpty(securityValue))
		{
			return defaultValue;
		}
		float result = defaultValue;
		if (!float.TryParse(securityValue, out result))
		{
			return defaultValue;
		}
		return result;
	}

	public static string GetString(string key, string defaultValue)
	{
		string securityValue = GetSecurityValue(key);
		if (string.IsNullOrEmpty(securityValue))
		{
			return defaultValue;
		}
		return securityValue;
	}

	public static bool HasKey(string key)
	{
		string securityValue = GetSecurityValue(key);
		return !string.IsNullOrEmpty(securityValue);
	}
}
