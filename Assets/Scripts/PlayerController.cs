using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetTransform {
	
	public float speed = 1.0f;
	public float attackSpeed = 0.75f;
	public float bulletSpeed = 90.0f;
	public float bulletDecay = 1.25f;
	
	[SerializeField]
	private GameObject bullet;
	[SerializeField]
	private Transform bulletSpawnPoint;
	
	private float attackCooldown = 0.0f;

	protected override void Start () {
		base.Start();
		
		if (isLocalPlayer) {
			tag = "LocalPlayer";
		}
	}
	
	protected override void Update() {
		base.Update();
		
		if (isLocalPlayer) {
			if (attackCooldown > 0) {
				attackCooldown -= Time.deltaTime;
				
				if (attackCooldown < 0)
					attackCooldown = 0;
			}

			Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			movement *= speed;
			
			GetComponent<Rigidbody2D>().velocity = movement;
			
			Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			
			float angle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x);
			float deg = ((180 / Mathf.PI) * angle) - 90;
			transform.rotation = Quaternion.Euler(0, 0, deg);
			
			if (Input.GetButtonDown("Fire1") && bulletSpawnPoint != null && attackCooldown == 0) {
				CmdFire(bulletSpawnPoint.position, bulletSpawnPoint.rotation);
				attackCooldown = 1.0f / attackSpeed;
			}
		}
	}
	
	[Command]
	void CmdFire(Vector3 position, Quaternion rotation) {
		Quaternion rotationOne = rotation * Quaternion.Euler(0, 0, Random.Range(-16.0f, 16.0f));
		Quaternion rotationTwo = rotation * Quaternion.Euler(0, 0, Random.Range(-16.0f, 16.0f));
		Quaternion rotationThree = rotation * Quaternion.Euler(0, 0, Random.Range(-16.0f, 16.0f));
		
		GameObject newBulletOne = (GameObject)Instantiate(bullet, position, rotationOne);
		GameObject newBulletTwo = (GameObject)Instantiate(bullet, position, rotationTwo);
		GameObject newBulletThree = (GameObject)Instantiate(bullet, position, rotationThree);
		
		newBulletOne.GetComponent<Rigidbody2D>().velocity = newBulletOne.transform.up * bulletSpeed;
		newBulletTwo.GetComponent<Rigidbody2D>().velocity = newBulletTwo.transform.up * bulletSpeed;
		newBulletThree.GetComponent<Rigidbody2D>().velocity = newBulletThree.transform.up * bulletSpeed;
		
		NetworkServer.Spawn(newBulletOne);
		NetworkServer.Spawn(newBulletTwo);
		NetworkServer.Spawn(newBulletThree);
		
		Destroy(newBulletOne, bulletDecay);
		Destroy(newBulletTwo, bulletDecay);
		Destroy(newBulletThree, bulletDecay);
	}
}
