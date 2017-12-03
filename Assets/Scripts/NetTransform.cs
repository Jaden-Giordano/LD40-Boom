using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetTransform : NetworkBehaviour {
	
	private Vector3 newPosition;
	private Quaternion newRotation;
	
	public int updatesPerSecond = 8;
	public float interpSmoothing = 4.0f;
	public float interpRotationSmoothing = 12.0f;
	
	protected virtual void Start() {
		if (isLocalPlayer || (isServer && !gameObject.tag.Contains("Player")))
			StartCoroutine(UpdateTransform());
	}

	protected virtual void Update() {
		if (isLocalPlayer || (isServer && !gameObject.tag.Contains("Player")))
			return;
		
		transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * interpSmoothing);
		transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * interpRotationSmoothing);
	}
	
	protected IEnumerator UpdateTransform() {
		while (enabled) {
			CmdSendPosition(transform.position);
			CmdSendRotation(transform.rotation);
			
			yield return new WaitForSeconds(1.0f / updatesPerSecond);
		}
	}
	
	[Command]
	void CmdSendPosition(Vector3 position) {
		newPosition = position;
		RpcReceivePosition(position);
	}
	
	[ClientRpc]
	void RpcReceivePosition(Vector3 position) {
		newPosition = position;
	}
	
	[Command]
	void CmdSendRotation(Quaternion rotation) {
		newRotation = rotation;
		RpcReceiveRotation(rotation);
	}
	
	[ClientRpc]
	void RpcReceiveRotation(Quaternion rotation) {
		newRotation = rotation;
	}
}
