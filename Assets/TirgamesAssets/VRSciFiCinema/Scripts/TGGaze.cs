using UnityEngine;
using System.Collections;

public class TGGaze : MonoBehaviour {
	public Transform GazeObject;

	Transform cameratransform;
	// Use this for initialization
	void Start () {
		cameratransform=Camera.main.transform;	
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray=new Ray();
		ray.origin=cameratransform.position;
		ray.direction=cameratransform.forward;
		RaycastHit hit;
		if (Physics.Raycast(ray,out hit, 10,Physics.AllLayers)) {
			GazeObject.position=hit.point;
		}
	}
}
