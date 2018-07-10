using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Rokid
{
	public class UIRayCaster : MonoBehaviour
	{
		GraphicRaycaster m_rayCaster = null;
		EventSystem m_eventSystem = null;

		List<RayCastSensor> m_registeredSensor = new List<RayCastSensor>();

		void Awake()
		{
			m_rayCaster = GetComponent<GraphicRaycaster>();
			m_eventSystem = transform.parent.Find("EventSystem").GetComponent<EventSystem>();
		}

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			PointerEventData ped = new PointerEventData(m_eventSystem);
			ped.position = new Vector2(Screen.width / 2, Screen.height / 2);
			List<RaycastResult> results = new List<RaycastResult>();
			m_rayCaster.Raycast(ped, results);
			List<RayCastSensor> targetSensorList = new List<RayCastSensor>();
			for(int i=0;i<results.Count;i++)
			{
				var s = results[i].gameObject.GetComponent<RayCastSensor>();
				if (s != null) { targetSensorList.Add(s); }
			}

			for(int i=0;i<m_registeredSensor.Count;i++)
			{
				if (targetSensorList.Contains(m_registeredSensor[i])) m_registeredSensor[i].Tick(true);
				else m_registeredSensor[i].Tick(false);
			}

		}

		public void RegisterSensor(RayCastSensor sensor)
		{
			if (m_registeredSensor.Contains(sensor)) return;
			m_registeredSensor.Add(sensor);
		}

		public void UnRegisterSensor(RayCastSensor sensor)
		{
			m_registeredSensor.Remove(sensor);
		}
	}

}
