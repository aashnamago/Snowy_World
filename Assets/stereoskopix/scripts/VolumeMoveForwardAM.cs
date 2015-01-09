using UnityEngine;
using System.Collections;

public class VolumeMoveForwardAM: MonoBehaviour {
	private s3dCameraSBS cameraScript = null; 
	private bool initSuccess = false;
	private int volumePress = 0;

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
	

		#if UNITY_EDITOR || UNITY_STANDALONE
		if (Input.GetKeyDown(KeyCode.Q)) {
			volumePress = 1;
		} else if (Input.GetKeyUp(KeyCode.Q)) {
			volumePress = 0;
		} else if (Input.GetKeyDown(KeyCode.W)) {
			volumePress = 2;
		} else if (Input.GetKeyUp(KeyCode.W)) {
			volumePress = 0;
		} 
		#endif

		#if UNITY_IPHONE && !UNITY_EDITOR
		try {
			volumePress = VolumeUpDown.GetLastVolumePress();
		} catch (System.Exception error) {
			Debug.Log("VOLUME PRESS ERROR");
		}
		#endif

		// Evaluate state of volume button

		if (volumePress == 1) { // volume down button 
			transform.position -= (0.5f * transform.forward);
		}
		if (volumePress == 2) { // volume up button
			transform.position += (0.5f * transform.forward);
		} 

	}
}
