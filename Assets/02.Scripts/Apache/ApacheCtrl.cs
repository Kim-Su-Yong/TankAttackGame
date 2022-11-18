using UnityEngine;
using System.Collections;

public class ApacheCtrl : MonoBehaviour
{
	public float rotSpeed;
	public float moveSpeed;
	public float verticalSpeed;
	Vector3 movingVector;
	public float POWER;
	private float powerDelta;
	private Transform tr;
	public bool isGetInapa = false;
	public bool isGetOutapa = true;
	CheckBoxApache boxApache;
	void Start()
	{
		POWER = 50f;
		rotSpeed = 0f;
		moveSpeed = 0f;
		verticalSpeed = 0f;
		powerDelta = 0f;
		tr = GetComponent<Transform>();
		boxApache = GetComponent<CheckBoxApache>();
	}
	void Update()
	{
			#region 아파치 AD 좌우회전
			if (Input.GetKey(KeyCode.A))
			{
				rotSpeed += -0.02f;
			}
			else if (Input.GetKey(KeyCode.D))
				rotSpeed += 0.02f;
			else
			{
				if (rotSpeed > 0) rotSpeed += -0.02f;
				else if (rotSpeed < 0) rotSpeed += 0.02f;
			}
			tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
			#endregion
			#region 아파치 앞뒤이동
			if (Input.GetKey(KeyCode.W))
				moveSpeed += 0.2f;
			else if (Input.GetKey(KeyCode.S))
				moveSpeed += -0.2f;
			tr.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
			#endregion
			#region 아파치 브레이크
			if (Input.GetKey("up") && Input.GetKey("down"))
			{
				if (moveSpeed > 0) moveSpeed = 0f;
				else if (moveSpeed < 0) moveSpeed = 0f;
				if (Mathf.Abs(moveSpeed) < 0.01f)
					moveSpeed = 0f;

				if (Input.GetKey(KeyCode.Z)) verticalSpeed = 0f;
				else if (Input.GetKey(KeyCode.C)) verticalSpeed = 0f;
				if (Mathf.Abs(verticalSpeed) < 0.01f)
					verticalSpeed = 0f;
			}
			else
			{
				if (Input.GetKey("up")) moveSpeed += 0.04f;
				else if (Input.GetKey("down")) moveSpeed += -0.04f;
			}
			#endregion
			#region 위아래 이동
			if (Input.GetKey(KeyCode.Z))
				verticalSpeed += 0.04f;
			else if (Input.GetKey(KeyCode.C))
				verticalSpeed += -0.04f;
			else
			{
				if (Input.GetKey(KeyCode.Z)) verticalSpeed += -0.04f;
				else if (Input.GetKey(KeyCode.C)) verticalSpeed += 0.04f;
				verticalSpeed = 0f;
			}
			tr.Translate(Vector3.up * verticalSpeed * Time.deltaTime);
			#endregion

	}
}