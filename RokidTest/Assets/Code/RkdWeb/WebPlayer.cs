using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using UnityEngine.Networking;

namespace Rokid
{
	public class WebPlayer : MonoBehaviour
	{

		private string m_jsonString = "";
		private GoogleNewsProtocol m_googleNewsProtocol = null;

		public List<SingleNewsAgent> m_newsAgentList = new List<SingleNewsAgent>();

		[System.NonSerialized]
		public Transform m_background, m_title, m_scrollView,m_scrollViewContent, m_browserBackground, m_BrowserGUI;

		private void Awake()
		{
			m_background = transform.Find("Background");
			m_title = transform.Find("Title");
			m_scrollView = transform.Find("ScrollView");
			m_scrollViewContent = transform.Find("ScrollView/Viewport/Content");
			m_browserBackground = transform.Find("BrowserBackground");
			m_BrowserGUI = transform.Find("BrowserGUI");
		}


		// Use this for initialization
		void Start()
		{
			var url = "https://newsapi.org/v2/top-headlines?" + "country=us&" + "apiKey=d81dc051b75a47cb8d69e3f226de02c6";
			StartCoroutine(GetNewsList(url));
		}

		// Update is called once per frame
		void Update()
		{

		}

		private IEnumerator GetNewsList(string url)
		{
			UnityWebRequest www = UnityWebRequest.Get(url);
			yield return www.SendWebRequest();
			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log(www.error);
			}
			else
			{
				m_jsonString = www.downloadHandler.text;
				m_googleNewsProtocol = GA.LitJson.JsonMapper.ToObject<GoogleNewsProtocol>(m_jsonString);

				for(int i=0;i<m_newsAgentList.Count;i++)
				{
					Destroy(m_newsAgentList[i].gameObject);
				}
				m_newsAgentList.Clear();

				for(int i=0;i<m_googleNewsProtocol.totalResults;i++)
				{
					if (m_googleNewsProtocol.articles[i].urlToImage == null) continue;
					SingleNewsAgent agent = SingleNewsAgent.__Create(this, m_googleNewsProtocol.articles[i]);
					agent.transform.SetParent(m_scrollViewContent);
					agent.transform.localEulerAngles = Vector3.zero;
					agent.transform.localScale = Vector3.one;
					m_newsAgentList.Add(agent);
				}
				RectTransform rt = m_scrollViewContent.GetComponent<RectTransform>();
				rt.sizeDelta = new Vector2(rt.sizeDelta.x, 20 + 200 * m_newsAgentList.Count);
				for (int i=0;i<m_newsAgentList.Count;i++)
				{
					rt = m_newsAgentList[i].GetComponent<RectTransform>();
					rt.anchoredPosition3D = new Vector3(20, -10 - 200 * i, 0);
					m_newsAgentList[i].m_index.text = (i + 1).ToString();
				}
			}
		}


		public void OnNewAgentClicked(SingleNewsAgent agent)
		{
			Debug.Log(agent.m_index.text);
			m_browserBackground.gameObject.SetActive(true);
			m_BrowserGUI.gameObject.SetActive(true);
			m_scrollView.gameObject.SetActive(false);
			//m_BrowserGUI.GetComponent<ZenFulcrum.EmbeddedBrowser.Browser>().Url = agent.m_article.url;
		}

		public void OnDetailNewsCancel()
		{
			m_BrowserGUI.gameObject.SetActive(false);
			m_browserBackground.gameObject.SetActive(false);
			m_scrollView.gameObject.SetActive(true);
		}

		public void OnScrollUp()
		{
			if(m_BrowserGUI.gameObject.activeSelf)
			{

			}
			else
			{
				var rt = m_scrollViewContent.GetComponent<RectTransform>();
				rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y - Time.deltaTime * 150);
			}
		}

		public void OnScrollDown()
		{
			if (m_BrowserGUI.gameObject.activeSelf)
			{

			}
			else
			{
				var rt = m_scrollViewContent.GetComponent<RectTransform>();
				rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y + Time.deltaTime * 150);
			}
		}

	}
}

