using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : NetTransform {
	
	void OnTriggerEnter2D(Collider2D coll) {
		if (!coll.isTrigger) {
			if (coll.gameObject.tag == "Enemy") {
				coll.gameObject.SendMessage("ApplyDamage", 34.0f);
			
				Destroy(gameObject);
			} else if (coll.gameObject.tag == "Environment") {
				Destroy(gameObject);
			}
		}
	}
}
