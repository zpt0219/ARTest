using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokid
{
	public class ResourceManager
	{
		private static ResourceManager _inst = null;

		public static ResourceManager Inst
		{
			get
			{
				if (_inst == null)
				{
					_inst = new ResourceManager();
				}
				return _inst;
			}
		}

		public string MapToolObjPath = "MapTool/";

		Dictionary<string, Object> m_ResourceDic = new Dictionary<string, Object>();

		public TextAsset GetText(string txtPath)
		{
			return GetObject<TextAsset>(txtPath);
		}

		public Material GetMaterial(string materialPath)
		{
			return GetObject<Material>(materialPath);
		}

		public Material GetMTMaterial(string materialPath)
		{
			return GetObject<Material>(MapToolObjPath + materialPath, true);
		}

		public GameObject GetMTPrefab(string prefabPath)
		{
			return GetObject<GameObject>(MapToolObjPath + prefabPath);
		}

		public GameObject GetPrefab(string prefabPath)
		{
			return GetObject<GameObject>(prefabPath);
		}

		public AudioClip GetAudioClip(string path)
		{
			return GetObject<AudioClip>(path);
		}

		public Sprite GetUISprite(string spritePath)
		{
			return GetObject<Sprite>(spritePath);
		}

		public Texture2D GetTexture2D(string path)
		{
			return GetObject<Texture2D>(path);
		}

		public T GetObject<T>(string objectPath, bool copy = false)
		{
			Object obj = GetObject(objectPath, copy);
			if (obj is T)
			{
				return (T)(object)obj;
			}
			else
			{
				Debug.LogError("Function ResourceManager.GetObject<T>(string path), T can not be converted!");
				Debug.LogError(objectPath);
				Debug.LogError(typeof(T));
				return default(T);
			}
		}

		public Object GetObject(string objectPath, bool copy = false)
		{
			if (m_ResourceDic.ContainsKey(objectPath))
			{
				return m_ResourceDic[objectPath];
			}
			else
			{
				Object g = Resources.Load(objectPath);
				if (g == null) return null;
				if (copy) g = Object.Instantiate(g);
				Debug.Log("Object Loadded At:" + objectPath);
				m_ResourceDic.Add(objectPath, g);
				return g;
			}
		}
	}
}
