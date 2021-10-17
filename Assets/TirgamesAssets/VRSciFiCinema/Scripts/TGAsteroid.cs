using UnityEngine;
using System.Collections;

// Single asterod movement script

public class TGAsteroid : MonoBehaviour {	
	// This parameters controlled over TGAsteroids script
	[HideInInspector]
	public Vector3 Speed;
	[HideInInspector]
	public Vector3 RotSpeed;

	Transform tr;
	// Use this for initialization
	void Start () {
		tr=transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		tr.position+=Speed*Time.fixedDeltaTime;
		tr.Rotate(RotSpeed*Time.fixedDeltaTime);
	}

	void OnTriggerEnter(Collider other) {
		Destroy(this.gameObject);
	}
}
