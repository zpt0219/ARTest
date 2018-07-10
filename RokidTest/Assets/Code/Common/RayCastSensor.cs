using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokid
{
	public class RayCastSensor : MonoBehaviour
	{

		public UnityEngine.Events.UnityEvent m_hoverHandler;
		public UnityEngine.Events.UnityEvent m_outsideHandler;
		public UnityEngine.Events.UnityEvent m_onEnterHandler;
		public UnityEngine.Events.UnityEvent m_onExitHandler;
		public UnityEngine.Events.UnityEvent m_hold2SecondsHandler;

		private float m_holdingTime = 0;

		private void Awake()
		{
			
		}
		// Use this for initialization
		void Start()
		{
			transform.GetComponentInParent<UIRayCaster>().RegisterSensor(this);
		}

		// Update is called once per frame
		void Update()
		{

		}

		public void Tick(bool focused)
		{
			float lastHoldingTime = m_holdingTime;
			if (focused)
			{
				m_holdingTime += Time.deltaTime;
			}
			else
			{
				m_holdingTime = 0;
			}
			
			if (lastHoldingTime < 2 && m_holdingTime >= 2) m_hold2SecondsHandler.Invoke();
			if (lastHoldingTime == 0 && m_holdingTime > 0) m_onEnterHandler.Invoke();
			if (lastHoldingTime > 0 && m_holdingTime == 0) m_onExitHandler.Invoke();
			if (m_holdingTime > 0) m_hoverHandler.Invoke();
			if (m_holdingTime == 0) m_outsideHandler.Invoke();
		}
	}

}
