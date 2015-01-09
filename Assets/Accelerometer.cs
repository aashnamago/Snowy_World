using UnityEngine;
using System.Collections;

public class Accelerometer : MonoBehaviour {
	public float speed = 5.0f;


	void Update () {
		Vector3 dir = Vector3.zero;
		float inputx = Input.acceleration.x;
		float inputy = Input.acceleration.y;
		float inputz = Input.acceleration.z;

//		inputy += 1.0f;
//		dir.x = Input.acceleration.x;
//		dir.y = Input.acceleration.z;
		dir.z = -Input.acceleration.z;

		// using negatives of these?
		if (dir.sqrMagnitude > 1)
			dir.Normalize();
		
		dir *= Time.deltaTime;
		transform.Translate(dir * speed);
	}

//	void OnGUI(){
//			GUI.Label( new Rect(0,0,Screen.width,Screen.height),"accelerometer: (" + Input.acceleration.x + ", " + Input.acceleration.y + ", " + Input.acceleration.z + ")");
//
//	}


		
		

}
