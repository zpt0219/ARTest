using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rokid
{
	public class SingleNewsAgent : MonoBehaviour
	{
		public SingleGoogleArticle m_article = null;

		[System.NonSerialized]
		public RawImage m_image = null;
		[System.NonSerialized]
		public Text m_title, m_description, m_index;
		

		private WebPlayer m_webPlayer = null;

		public static SingleNewsAgent __Create(WebPlayer webPlayer, SingleGoogleArticle article)
		{
			GameObject obj = ResourceManager.Inst.GetPrefab("Prefab/SingleNews");
			obj = Instantiate(obj);
			SingleNewsAgent res = obj.GetComponent<SingleNewsAgent>();
			res.m_article = article;
			res.m_webPlayer = webPlayer;
			return res;
		}

		private void Awake()
		{
			m_image = transform.Find("RawImage").GetComponent<RawImage>();
			m_title = transform.Find("Title").GetComponent<Text>();
			m_description = transform.Find("Description").GetComponent<Text>();
			m_index = transform.Find("Index").GetComponent<Text>();
		}

		// Use this for initialization
		void Start()
		{
			m_title.text = m_article.title;
			m_description.text = m_article.description;
			StartCoroutine(LoadImageFromUrl(m_article.urlToImage));
		}

		private IEnumerator LoadImageFromUrl(string url)
		{
			using (WWW www = new WWW(url))
			{
				yield return www;
				m_image.texture = www.texture;
			}
		}

		// Update is called once per frame
		void Update()
		{

		}

		public void OnClick()
		{
			Debug.Log("Clicked");
			m_webPlayer.OnNewAgentClicked(this);

		}

	}
}

