using UnityEngine;
using System.Collections;

public class moveWithCamera : MonoBehaviour {

	public Transform player; 

	// Update is called once per frame
	void Update () {
		this.transform.position = player.position;
//		this.transform.position = new Vector3 (player.position.x, 0, player.position.z);
	}
}
