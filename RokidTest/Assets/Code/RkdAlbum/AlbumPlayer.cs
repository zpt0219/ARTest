using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rokid
{
	public class AlbumPlayer : MonoBehaviour
	{
		public List<SingleImageAgent> m_imageAgentList = new List<SingleImageAgent>();

		[System.NonSerialized]
		public Transform m_background, m_title, m_scrollViewContent, m_singleView, m_singleViewImage;

		private Slider m_imageScaleSlider = null;

		public float m_singleImageMaxScale = 4;

		private void Awake()
		{
			m_background = transform.Find("Background");
			m_title = transform.Find("Title");
			m_scrollViewContent = transform.Find("ScrollView/Viewport/Content");
			m_singleView = transform.Find("SingleView");
			m_singleViewImage = m_singleView.Find("Mask/Image");
			m_imageScaleSlider = m_singleView.Find("Slider").GetComponent<Slider>();
		}


		// Use this for initialization
		void Start()
		{
			for(int i=0;i<20;i++)
			{
				var agent = SingleImageAgent.__Create(this, "Image/CUI_photo" + Random.Range(0, 5));
				agent.transform.SetParent(m_scrollViewContent);
				agent.transform.localEulerAngles = Vector3.zero;
				agent.transform.localScale = Vector3.one;
				m_imageAgentList.Add(agent);
			}
			
			RearrangeImages();
		}

		// Update is called once per frame
		void Update()
		{

		}

		public void RearrangeImages()
		{
			RectTransform rt = m_scrollViewContent.GetComponent<RectTransform>();
			rt.sizeDelta = new Vector2((m_imageAgentList.Count + 1) / 2 * 276, rt.sizeDelta.y);

			for (int i=0;i<m_imageAgentList.Count;i++)
			{
				rt = m_imageAgentList[i].GetComponent<RectTransform>();
				int row = i % 2;
				int col = i / 2;
				rt.anchoredPosition3D = new Vector3(10 + 276 * col, -10 - 276 * row, 0);
			}

		}

		public void OnImageAgentClicked(SingleImageAgent agent)
		{
			m_singleView.gameObject.SetActive(true);
			m_singleViewImage.GetComponent<RawImage>().texture = agent.m_texture;
			m_imageScaleSlider.value = 0;
		}

		public void OnSingleViewIamgeClicked()
		{
			m_singleView.gameObject.SetActive(false);
		}

		public void OnSingleViewSliderChanged()
		{
			float ratio = m_imageScaleSlider.value * (m_singleImageMaxScale - 1) + 1;
			RectTransform rt = m_singleViewImage.GetComponent<RectTransform>();
			rt.sizeDelta = new Vector2(256 * ratio, 256 * ratio);
		}
	}
}


