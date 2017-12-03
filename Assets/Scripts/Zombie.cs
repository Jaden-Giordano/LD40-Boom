using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Zombie : NetTransform, IEntity {
	
	public float health = 100.0f;
	public float speed = 5.0f;
	
	public float attackRange = 0.2f;
	public float attackSpeed = 1.0f; // Attacks per second
	
	private GameObject target;
	
	private float attackCooldown = 0.0f;

	protected override void Update() {
		base.Update();
		
		if (isServer) {
			if (attackCooldown > 0) {
				attackCooldown -= Time.deltaTime;
				
				if (attackCooldown < 0)
					attackCooldown = 0;
			}
			
			if (target != null) {
				Vector3 movement = Vector3.Normalize(target.transform.position - transform.position);
				movement *= speed;
				
				GetComponent<Rigidbody2D>().velocity = movement;
				
				Vector2 targetPosition = target.transform.position;
				
				float angle = Mathf.Atan2(targetPosition.y - transform.position.y, targetPosition.x - transform.position.x);
				float deg = ((180 / Mathf.PI) * angle) - 90;
				transform.rotation = Quaternion.Euler(0, 0, deg);
				
				if (Vector3.Distance(targetPosition, transform.position) <= attackRange && attackCooldown == 0) {
					target.SendMessage("ApplyDamage", 10);
					attackCooldown = 1.0f / attackSpeed;
				}
			}
		}
	}
	
	public void ApplyDamage(float damage) {
		if (health > 0) {
			health -= damage;
			
			if (health <= 0) {
				Destroy(gameObject);
			}
		}
	}
	
	void OnTriggerEnter2D(Collider2D coll) {
		if (isServer && (coll.gameObject.tag == "Player" || coll.gameObject.tag == "LocalPlayer")) {
			target = coll.gameObject;
		}
	}
	
	void OnTriggerExit2D(Collider2D coll) {
		if (isServer && (coll.gameObject.tag == "Player" || coll.gameObject.tag == "LocalPlayer")) {
			target = null;
		}
	}
}
