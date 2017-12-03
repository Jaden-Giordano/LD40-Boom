using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour, IEntity {
	
	public float health = 100.0f;

	public void ApplyDamage(float damage) {
		if (health > 0) {
			health -= damage;
			
			if (health <= 0) {
				Network.Disconnect();
				SceneManager.LoadScene("Empty");
			}
		}
	}
	
}
