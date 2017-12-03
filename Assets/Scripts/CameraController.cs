using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	
	public float smoothing = 1.0f;
	
	[SerializeField]
	private Transform target;

	void Start () {
		AttemptSetTarget();
	}
	
	void Update () {
		if (target != null) {
			Camera view = GetComponent<Camera>();
			Vector2 offset = Vector2.zero;
			
			if (view != null) {
				offset = (view.ScreenToWorldPoint(Input.mousePosition) - target.position) / 4;
			}
			
			Vector2 newPosition =  Vector2.Lerp(transform.position, new Vector2(target.position.x, target.position.y) + offset, Time.deltaTime * smoothing);
			transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
		} else {
			AttemptSetTarget();
		}
	}
	
	private void AttemptSetTarget() {
		if (target == null) {
			GameObject player = GameObject.FindWithTag("LocalPlayer");
			
			if (player != null) {
				target = player.transform;
			}
		}
	}
}
