using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LZ : MonoBehaviour {

	void OnTriggerStay2D(Collider2D coll) {
		if (coll.gameObject.tag.Contains("Player")) {
			if (GameObject.FindWithTag("Enemy") == null) {
				Network.Disconnect();
				SceneManager.LoadScene("Empty");
			}
		}
	}
	
}
