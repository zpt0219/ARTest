using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokid
{
	public class DemoCharacterController : MonoBehaviour
	{
		private float m_moveSpeed = 3f;
		private float m_rotateSpeed = 180f;

		private Vector3 m_moveVec = Vector3.zero;

		private Transform m_cameraPivot = null;

		private Vector3 lastMousePosition = Vector3.zero;

		//Since I use capsule to represent the character, there is no need to use the animator
		private Animator m_animator = null;

		public bool m_lockCamera = false;

		private void Awake()
		{
			m_cameraPivot = Camera.main.transform.parent;
			m_animator = GetComponentInChildren<Animator>();
		}

		enum AnimationEnum
		{
			None=0,
			Idle,
			Run,
			Jump,
		}
		private AnimationEnum currentAnimationEnum = AnimationEnum.None;
		private AnimationEnum nextAnimationEnum = AnimationEnum.None;

		// Use this for initialization
		void Start()
		{
			//GetComponent<CharacterController>().enabled = false;
			//RaycastHit hit;
			//Ray ray = new Ray(transform.position + 1000 * Vector3.up, Vector3.down);
			//if (Physics.Raycast(ray, out hit, 10000, 0x7fffffff))
			//{
			//	transform.position = hit.point;
			//}
			//GetComponent<CharacterController>().enabled = true;
		}

		// Update is called once per frame

		private void OnGUI()
		{
            GUI.Label(new Rect(10, 10, 100, 30), Input.acceleration.x.ToString());
            GUI.Label(new Rect(10, 40, 100, 30), Input.acceleration.y.ToString());
            GUI.Label(new Rect(10, 70, 100, 30), Input.acceleration.z.ToString());
            GUI.Label(new Rect(10, 100, 100, 30), m_cameraPivot.localEulerAngles.x.ToString());
            GUI.Label(new Rect(10, 130, 100, 30), m_cameraPivot.localEulerAngles.y.ToString());


		}

		void Update()
		{
			//nextAnimationEnum = AnimationEnum.Idle;

			//Vector3 horizontalMove = Vector3.zero;
			//if(Input.GetKey(KeyCode.W))
			//{
			//	Vector3 v = Camera.main.transform.forward;
			//	v.y = 0;
			//	v.Normalize();
			//	horizontalMove += v;
			//}
			//if (Input.GetKey(KeyCode.S))
			//{
			//	Vector3 v = -Camera.main.transform.forward;
			//	v.y = 0;
			//	v.Normalize();
			//	horizontalMove += v;
			//}
			//if (Input.GetKey(KeyCode.A))
			//{
			//	Vector3 v = -Camera.main.transform.right;
			//	v.y = 0;
			//	v.Normalize();
			//	horizontalMove += v;
			//}
			//if (Input.GetKey(KeyCode.D))
			//{
			//	Vector3 v = Camera.main.transform.right;
			//	v.y = 0;
			//	v.Normalize();
			//	horizontalMove += v;
			//}
			//horizontalMove.Normalize();

			//if(horizontalMove.magnitude>0)
			//{
			//	RotateToDirection(new Vector2(horizontalMove.x, horizontalMove.z));
			//	nextAnimationEnum = AnimationEnum.Run;
			//}

			//CharacterController cc = GetComponent<CharacterController>();
			//if (cc.isGrounded)
			//{
			//	m_moveVec = new Vector3(0, -0.1f, 0);
			//	if (Input.GetKey(KeyCode.Space))
			//	{
			//		m_moveVec.y = 10f;
			//		nextAnimationEnum = AnimationEnum.Jump;
			//		Debug.Log("Space Pressed");
			//	}
			//}
			//else
			//{
			//	m_moveVec += Physics.gravity * Time.fixedDeltaTime;
			//	nextAnimationEnum = AnimationEnum.None;
			//}

			//m_moveVec = horizontalMove+new Vector3(0,m_moveVec.y,0);
			//cc.Move(new Vector3(m_moveVec.x * m_moveSpeed, m_moveVec.y, m_moveVec.z * m_moveSpeed) * Time.deltaTime);
		}

		private void LateUpdate()
		{
            UpdateCameraByGravity();
			//Vector3 dir = Input.mousePosition - lastMousePosition;
			//lastMousePosition = Input.mousePosition;
            //UpdateCamera(dir.x, -dir.y);
			
			SetAnimation(nextAnimationEnum);
		}

		private bool RotateToDirection(Vector2 direction, bool atOnce = false, float deltaTime = 0.033f)
		{
			if (direction.sqrMagnitude <= 0.00001f) return true;
			Vector3 characterForword = transform.forward;
			Vector2 cf = new Vector2(characterForword.x, characterForword.z);
			float degree = Vector2.Angle(cf, direction);
			direction = new Vector2(direction.y, -direction.x);
			if (Vector2.Dot(cf, direction) > 0) degree = -degree;
			float deltaDegree = m_rotateSpeed * deltaTime;
			if (atOnce) deltaDegree = Mathf.Abs(degree);
			if (Mathf.Abs(degree) > deltaDegree)
			{
				if (degree < 0) deltaDegree = -deltaDegree;
				Vector3 euler = transform.localEulerAngles;
				euler.y += deltaDegree;
				transform.localEulerAngles = euler;
				return false;
			}
			else
			{
				Vector3 euler = transform.localEulerAngles;
				euler.y += degree;
				transform.localEulerAngles = euler;
				return true;
			}
		}

        List<Vector2> m_degreeFilter = new List<Vector2>();

        private void UpdateCameraByGravity()
        {
            if (m_lockCamera) return;
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
			if (m_lockCamera) return;
			//m_cameraPivot.localPosition = this.transform.Find("UpAnchor").position;
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
			//Transform cameraTrans = m_cameraPivot.GetChild(0);
			//float z = Mathf.Clamp(Input.mouseScrollDelta.y * 0.3f + cameraTrans.localPosition.z, -8, -2);
			//cameraTrans.localPosition = new Vector3(cameraTrans.localPosition.x, cameraTrans.localPosition.y, z);
		}

		private void SetAnimation(AnimationEnum anim)
		{
			if (anim == AnimationEnum.None) return;
			if (currentAnimationEnum == AnimationEnum.Jump && m_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump_Down"))
			{

			}
			else if (currentAnimationEnum == anim) return;
			currentAnimationEnum = anim;
			m_animator.CrossFade(anim.ToString(), 0.2f);
		}
	}
}

