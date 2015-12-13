using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour {

	public int diceResult;
	float timer = 2;
	Vector3 torque;

	// Use this for initialization
	void Start () 
	{
		torque.x = Random.Range (800, 1200);
		torque.y = Random.Range (800, 1200);
		torque.z = Random.Range (800, 1200);
		GetComponent<Rigidbody> ().AddTorque (torque);
	}
	
	// Update is called once per frame
	void Update () {

		timer -= Time.deltaTime;
		if (timer <= 1)
		{
			Vector3 r = transform.eulerAngles;

			if (timer <= -1)
			{
				switch(diceResult)
				{
					case 1:
					transform.rotation = Quaternion.Euler(Mathf.LerpAngle(r.x, 90, Time.deltaTime * 10), r.y, Mathf.LerpAngle(r.z, 0, Time.deltaTime * 10));
					break;

					case 2:
					transform.rotation = Quaternion.Euler(Mathf.LerpAngle(r.x, 0, Time.deltaTime * 10), r.y, Mathf.LerpAngle(r.z, 270, Time.deltaTime * 10));
					break;

					case 3:
					transform.rotation = Quaternion.Euler(Mathf.LerpAngle(r.x, 0, Time.deltaTime * 10), r.y, Mathf.LerpAngle(r.z, 0, Time.deltaTime * 10));
					break;

					case 4:
					transform.rotation = Quaternion.Euler(Mathf.LerpAngle(r.x, 0, Time.deltaTime * 10), r.y, Mathf.LerpAngle(r.z, 180, Time.deltaTime * 10));
					break;

					case 5:
					transform.rotation = Quaternion.Euler(Mathf.LerpAngle(r.x, 0, Time.deltaTime * 10), r.y, Mathf.LerpAngle(r.z, 90, Time.deltaTime * 10));
					break;
				
					case 6:
					transform.rotation = Quaternion.Euler(Mathf.LerpAngle(r.x, -90, Time.deltaTime * 10), r.y, r.z);
					break;
				 }
			}

			if (timer <= -3)
			{
				GameObject.Find("Game Manager").GetComponent<GameManager>().canMove = true;
				GameObject.Find("Game Manager").GetComponent<GameManager>().diceRolling = false;
				Destroy(this.gameObject);
			}
		}
	}
}
