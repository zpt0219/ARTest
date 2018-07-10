using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour {

	private Transform m_cameraPivot = null;
	private Camera m_camera = null;

	private Vector3 lastMousePosition = Vector3.zero;

	private void Awake()
	{
		m_cameraPivot = transform;
		m_camera = transform.GetComponentInChildren<Camera>();
	}

	// Use this for initialization
	void Start () {
		
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(10, 10, 100, 30), Input.acceleration.x.ToString());
		GUI.Label(new Rect(10, 40, 100, 30), Input.acceleration.y.ToString());
		GUI.Label(new Rect(10, 70, 100, 30), Input.acceleration.z.ToString());
		GUI.Label(new Rect(10, 100, 100, 30), m_cameraPivot.localEulerAngles.x.ToString());
		GUI.Label(new Rect(10, 130, 100, 30), m_cameraPivot.localEulerAngles.y.ToString());
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void LateUpdate()
	{
#if UNITY_EDITOR
		Vector3 dir = Input.mousePosition - lastMousePosition;
		lastMousePosition = Input.mousePosition;
		UpdateCamera(dir.x, -dir.y);
#else
        UpdateCameraByGravity();
#endif
	}

	List<Vector2> m_degreeFilter = new List<Vector2>();

	private void UpdateCameraByGravity()
	{
		float xAngle = Mathf.Asin(Input.acceleration.y);
		float yAngle = Mathf.Asin(Input.acceleration.x);
		xAngle = xAngle * 180 / Mathf.PI;
		if (xAngle > 180) xAngle -= 360;
		yAngle = yAngle * 180 / Mathf.PI;
		if (yAngle > 180) yAngle -= 360;
		xAngle = Mathf.RoundToInt(xAngle);
		yAngle = Mathf.RoundToInt(yAngle);

		m_degreeFilter.Add(new Vector2(xAngle, yAngle));
		if (m_degreeFilter.Count < 6) return;
		m_degreeFilter.RemoveAt(0);

		/*Vector2 v = 0.0009f * m_degreeFilter[0] + 0.0175f * m_degreeFilter[1] + 0.1295f * m_degreeFilter[2] +
					 0.3521f * m_degreeFilter[3] + 0.3521f * m_degreeFilter[4] + 0.1295f * m_degreeFilter[5] +
					 0.0175f * m_degreeFilter[6] + 0.009f * m_degreeFilter[7];*/

		Vector2 v = m_degreeFilter[0] + 4 * m_degreeFilter[1] + 6 * m_degreeFilter[2] + 4 * m_degreeFilter[3] + m_degreeFilter[4];
		v /= 16;

		m_cameraPivot.localEulerAngles = new Vector3(v.x, v.y, 0);
	}

	private void UpdateCamera(float deltaX, float deltaY)
	{
		if (deltaX != 0 || deltaY != 0)
		{
			if (Input.GetMouseButton(1))
			{
				Vector3 rotate = m_cameraPivot.localEulerAngles + new Vector3(deltaY, deltaX, 0);
				if (rotate.x >= 180f) rotate.x -= 360f;
				if (rotate.x > -20f && rotate.x < 90f)
				{
					m_cameraPivot.localEulerAngles = rotate;
				}
			}
		}
	}

}
