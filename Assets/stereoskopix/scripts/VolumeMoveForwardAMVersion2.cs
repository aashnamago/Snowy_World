using UnityEngine;
using System.Collections;

public class VolumeMoveForwardAMVersion2: MonoBehaviour {

	private s3dCameraSBS cameraScript = null; 
	private bool initSuccess = false;
	private int prevVolume = 0; 
	private int currVolume = 0;
	private int volumePress = 0;

	private bool moveUp = false;
	private bool moveBackward = false;
//	private int timer = 0;

	
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

	void Update () {

	}

	void FixedUpdate() {
		#if UNITY_EDITOR || UNITY_STANDALONE
		if (Input.GetKeyDown(KeyCode.Q)) {
			volumePress = 1;
			prevVolume = currVolume; //TP
			currVolume = volumePress; //TP
		} else if (Input.GetKeyUp(KeyCode.Q)) {
			volumePress = 0;
			prevVolume = currVolume; //TP
			currVolume = volumePress; //TP
		} else if (Input.GetKeyDown(KeyCode.W)) {
			volumePress = 2;
			prevVolume = currVolume; //TP
			currVolume = volumePress; //TP
		} else if (Input.GetKeyUp(KeyCode.W)) {
			volumePress = 0;
			prevVolume = currVolume; //TP
			currVolume = volumePress; //TP
		}
		#endif

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
		if (currVolume == 1 && prevVolume != 1) { // volume down button 
			moveBackward = true;
			moveUp = false;// move back
		}
		if (currVolume == 2 && prevVolume != 2) { // volume up button
			// move forward
		} 

	}

	IEnumerator moveForward() {
		if (moveUp) {
			// move forward
		}
		yield return null;
	}
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
