using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationController : MonoBehaviour {

	public GameObject enemyPrefab;
	public float frameWidth = 10f;
	public float frameHeight = 5f;
	public float frameAdjustY = 0f;
	public float enemySpeed = 5f;
	public float enemyMoveRange = 0.33f;
	public float spawnDelay = 2f;

	private Vector3 enemyPos;
	private bool movingRight = true;
	private float xMax;
	private float xmin;
	private float yMax;
	private float ymin;

	// Use this for initialization
	void Start () {
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftDown = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera));
		Vector3 rightUp = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, distanceToCamera));
		xMax = rightUp.x;
		xmin = leftDown.x;
		yMax = rightUp.y;
		ymin = leftDown.y;

		SpawnUntillFull ();
	}

	public void OnDrawGizmos(){
		
		Gizmos.DrawWireCube((transform.position + new Vector3(0f, frameAdjustY)), new Vector3(frameWidth, frameHeight));
	}

	// Update is called once per frame
	void Update () {
		if (movingRight) {
			transform.position += new Vector3 (enemySpeed * Time.deltaTime, 0f);
		} else {
				transform.position += new Vector3 (-enemySpeed * Time.deltaTime, 0f);
		}
		enemyPos = transform.position;
		if (enemyPos.x >= xMax * enemyMoveRange){
			movingRight = false;
		}else if (enemyPos.x <= xmin * enemyMoveRange){
			movingRight = true;
		}

		if (AllMemberDead ()) {
			//Debug.Log ("Empty formation");
			SpawnUntillFull ();
		}
	}

	Transform NextFreePosition(){
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount == 0) {return childPositionGameObject;}
		}		
		return null;
	}

	bool AllMemberDead(){
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount > 0) {return false;}
		}
		return true;
	}

	// Obsolete
	void SpawnEnemies(){
		foreach (Transform child in transform) {
			GameObject enemy = Instantiate (enemyPrefab, child.transform.position, Quaternion.identity);
			enemy.transform.parent = child;
		}
	}

	void SpawnUntillFull(){
		Transform freePosition = NextFreePosition ();
		if (freePosition) {
			GameObject enemy = Instantiate (enemyPrefab, freePosition.position, Quaternion.identity);
			enemy.transform.parent = freePosition;
		}
		if (NextFreePosition ()) {
			Invoke ("SpawnUntillFull", spawnDelay);
		}
	}
}
