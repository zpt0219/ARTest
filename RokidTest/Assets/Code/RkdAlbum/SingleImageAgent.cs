using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rokid
{
	public class SingleImageAgent : MonoBehaviour
	{
		public Texture2D m_texture = null;
		public string m_imagePath = "";

		private AlbumPlayer m_albumPlayer = null;

		public static SingleImageAgent __Create(AlbumPlayer player,string imagePath)
		{
			GameObject obj = ResourceManager.Inst.GetPrefab("Prefab/SingleImage");
			obj = Instantiate(obj);
			SingleImageAgent res = obj.GetComponent<SingleImageAgent>();
			res.m_albumPlayer = player;
			res.m_imagePath = imagePath;
			res.m_texture = ResourceManager.Inst.GetTexture2D(imagePath);
			return res;
		}

		private void Awake()
		{
			
		}

		// Use this for initialization
		void Start()
		{
			GetComponent<RawImage>().texture = m_texture;
		}

		// Update is called once per frame
		void Update()
		{

		}

		public void OnClick()
		{
			Debug.Log("Clicked:" + m_imagePath);
			m_albumPlayer.OnImageAgentClicked(this);

		}
	}
}

