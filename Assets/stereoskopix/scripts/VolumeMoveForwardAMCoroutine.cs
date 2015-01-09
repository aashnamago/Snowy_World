using UnityEngine;
using System.Collections;

public class VolumeMoveForwardAMCoroutine: MonoBehaviour {
	private s3dCameraSBS cameraScript = null; 
	private bool initSuccess = false;
	private int volumePress = 0;
	private int prevVolume = 0;
	private int currVolume = 0;

	private float counter = 0f;

	private bool moveUp = false;
	private bool moveBack = false;


	void Start () {
		cameraScript = GetComponent<s3dCameraSBS>();

		#if UNITY_IPHONE && !UNITY_EDITOR 
		try {
			VolumeUpDown.Init();
			initSuccess = true;
		} catch (System.Exception error) {
			Debug.Log("INIT ERROR");
			initSuccess = false;
		}
		#endif
	}

	void OnGUI(){
//		GUI.Label( new Rect(0,0,Screen.width,Screen.height),"prevVolume " + prevVolume);
//		GUI.Label( new Rect(Screen.width / 2.0f, 0, Screen.width,Screen.height),"currVolume " + currVolume);
	}

	void Update () {

	}

	void FixedUpdate() {
	

//		#if UNITY_EDITOR || UNITY_STANDALONE
//		if (Input.GetKeyDown(KeyCode.Q)) {
//			volumePress = 1;
//			prevVolume = currVolume; //TP
//			currVolume = volumePress; //TP
//		} else if (Input.GetKeyUp(KeyCode.Q)) {
//			volumePress = 0;
//			prevVolume = currVolume; //TP
//			currVolume = volumePress; //TP
//		} else if (Input.GetKeyDown(KeyCode.W)) {
//			volumePress = 2;
//			prevVolume = currVolume; //TP
//			currVolume = volumePress; //TP
//		} else if (Input.GetKeyUp(KeyCode.W)) {
//			volumePress = 0;
//			prevVolume = currVolume; //TP
//			currVolume = volumePress; //TP
//		} 
//		#endif

		#if UNITY_IPHONE && !UNITY_EDITOR
		try {
			volumePress = VolumeUpDown.GetLastVolumePress();
			prevVolume = currVolume;
			currVolume = volumePress;
		} catch (System.Exception error) {
			Debug.Log("VOLUME PRESS ERROR");
		}
		#endif

		// Evaluate state of volume button



		if (currVolume == 1 || prevVolume == 1) { // volume down button 
			if (moveUp != true) {
				moveUp = true;
				counter = 0f;
				StartCoroutine("MovingUp");
			} 

			else if (counter >= 1.0f) {
				StopCoroutine("MovingUp");

			}
//
//			if (moveBack) {
//				StopCoroutine("MovingBack");
//				moveBack = false;
//			}


		}
//		if (currVolume == 2 || prevVolume == 2) { // volume up button
//			if (moveBack != true) {
//				moveBack = true;
//				counter = 0f;
//				StartCoroutine("MovingBack");
//			} else if (counter >= 1.0f) {
//				StopCoroutine("MovingBack");
//			}
//
//			if (moveUp) {
//				StopCoroutine("MovingUp");
//				moveUp = false;
//			}
//
//
//		} 

//		if (currVolume == 0 && prevVolume == 0) { // no press
//			if (moveUp == true) {
//				moveUp = false;
//				StopCoroutine("MovingUp");
//			}
//			if (moveBack == true) {
//				moveBack = false;
//				StopCoroutine("MovingBack");
//			}
//		}
	}

	IEnumerator MovingUp() {
		Vector3 currentPos = transform.position;
		Vector3 futurePos = transform.position + this.transform.forward * 1000;
		transform.position = Vector3.Lerp(currentPos, futurePos, counter);
		counter += (0.1f * Time.deltaTime);
		yield return null;
	}

//	IEnumerator MovingBack() {
//		yield return null;
//	}
}

// if volume press = 0?



// 

// may need some of these :
// 

//private var lastVolumePress = 0;  //0 = No press, 1 = Up, 2 = Down


//private var lastTime : float = 0; //TP
//


// FOR TESTING:
//		if (timer >= 10) {
//			GameObject cube =  GameObject.CreatePrimitive(PrimitiveType.Cube);
//			cube.transform.localScale = new Vector3 (10, 10, 10);
//			cube.transform.position = new Vector3 (0, 3, 2);
//			cube.renderer.material.color = Color.blue;
//		}
//		timer ++;
