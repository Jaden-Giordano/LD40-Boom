using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ZombieSpawner : NetworkBehaviour {
	
	[SerializeField]
	private GameObject zombie;
	
	public int numberOfZombies = 8;
	public float spawnRange = 8.0f;

	public override void OnStartServer() {
		SpawnZombies();
	}
	
	void OnPlayerConnected(NetworkPlayer player) {
		Debug.Log("MORE");
		SpawnZombies();
	}
	
	private void SpawnZombies() {
		for (int i = 0; i < numberOfZombies; i++) {
			Vector3 spawnPosition = new Vector3(Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange), 0);
			
			GameObject enemy = (GameObject)Instantiate(zombie, transform.position + spawnPosition, Quaternion.Euler(0, 0, 0));
			NetworkServer.Spawn(enemy);
		}
	}
}
