using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class TGAsteroidsSpawner: MonoBehaviour {
	public Vector3 SpeedMin;
	public Vector3 SpeedMax;
	public Vector3 RotSpeedMin;
	public Vector3 RotSpeedMax;
	public GameObject[] AsteroidPrefabs;
	public float SpawnPeriodMin=1f;
	public float SpawnPeriodMax=2f;
	public int	 SpawnOnStart=10;
	public int   SpawnMin=1;
	public int 	 SpawnMax=5;

	float spawntimer;
	BoxCollider boxcol;

	// Use this for initialization
	void Start () {
		boxcol=GetComponent<BoxCollider>();
		for (int i=0;i<SpawnOnStart;i++) {
			SpawnAsteroid();
		}
		spawntimer=Random.Range(SpawnPeriodMin,SpawnPeriodMax);
	}
	
	// Update is called once per frame
	void Update () {
		spawntimer-=Time.deltaTime;
		if (spawntimer<=0) {
			spawntimer=Random.Range(SpawnPeriodMin,SpawnPeriodMax);
			int c=Random.Range(SpawnMin,SpawnMax);
			for (int i=0;i<c;i++) {
				SpawnAsteroid();
			}
		}
	}


	void SpawnAsteroid() {		
		int id=Random.Range(0,AsteroidPrefabs.Length);
		Vector3 pos=new Vector3(Random.Range(boxcol.bounds.min.x,boxcol.bounds.max.x),Random.Range(boxcol.bounds.min.y,boxcol.bounds.max.y),Random.Range(boxcol.bounds.min.z,boxcol.bounds.max.z));
		GameObject obj=(GameObject) Instantiate(AsteroidPrefabs[id],pos,Quaternion.identity);
		TGAsteroid asteroid=obj.GetComponent<TGAsteroid>();
		asteroid.Speed=new Vector3(Random.Range(SpeedMin.x,SpeedMax.x),Random.Range(SpeedMin.y,SpeedMax.y),Random.Range(SpeedMin.z,SpeedMax.z));
		asteroid.RotSpeed=new Vector3(Random.Range(RotSpeedMin.x,RotSpeedMax.x),Random.Range(RotSpeedMin.y,RotSpeedMax.y),Random.Range(RotSpeedMin.z,RotSpeedMax.z));
	}
}
