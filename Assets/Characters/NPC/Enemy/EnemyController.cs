using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public enum enemyFireType {fix , random};
	public enemyFireType fireType;
	//public GameObject playerProjrctile;
	public GameObject enemyProjectile;
	//public Sprite enemyProjectileSprite;
	public float enemyHealth;
	public float enemyProjectileSpeed = 8f;
	public float enemyFireInterval = 2f;
	public float enemyFireRate = 1f;
	public int scoreVlaue = 150;
	public int enemySupply = 10;

	public AudioClip enemyFireAudio;
	public AudioClip enemyHitAudio;
	public AudioClip enemyDieAudio;
	public GameObject explodeEffect;
	public GameObject smokeEffect;

	private ScoreKeeper scoreKeeper;
	private PlayerController playerController;


	// Use this for initialization
	void Start () {
		scoreKeeper = GameObject.Find ("Score").GetComponent<ScoreKeeper> ();
		enemyHealth = 300;
		playerController = GameObject.FindObjectOfType<PlayerController> ();
		if (fireType == enemyFireType.fix) {
			InvokeRepeating ("EnemyFire", Random.Range (enemyFireInterval * 0.9f, enemyFireInterval * 1.1f), Random.Range (enemyFireRate * 0.9f, enemyFireRate * 1.1f));
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (fireType == enemyFireType.random) {
			float probability = Time.deltaTime * enemyFireRate;
			if (Random.value < probability) {
				EnemyFire ();
			}
		}
		
		
	}

	void OnTriggerEnter2D(Collider2D collider){
		//gameObject.GetComponent<projectile> ();
		Projectile projectile = collider.gameObject.GetComponent<Projectile>();
		if (projectile & projectile.tag == "playerProjectile") {
			Debug.Log ("Hit by player projectile: " + projectile);
			enemyHealth -= projectile.GetDamage();
			projectile.Hit ();
			AudioSource.PlayClipAtPoint (enemyHitAudio, transform.position, 2f);
			if (enemyHealth <= 0) {
				CancelInvoke ("EnemyFire");
				playerController.PlayerKillEnemy(enemySupply);
				Destroy (gameObject);
				scoreKeeper.Score (scoreVlaue);
				AudioSource.PlayClipAtPoint (enemyDieAudio, transform.position, 2f);
				Instantiate (explodeEffect, transform.position, Quaternion.identity);
				Instantiate (smokeEffect, transform.position, Quaternion.identity);
			}
		}
	}

	void EnemyFire(){
		GameObject projectile = Instantiate (enemyProjectile, transform.position + new Vector3(0f, -1f), Quaternion.Inverse(Quaternion.identity));
		projectile.tag = "enemyProjectile";
		// Switch enemy projectile Sprite
		//projectile.gameObject.GetComponent<SpriteRenderer> ().sprite = enemyProjectileSprite;
		//projectile.gameObject.transform.rotation = Quaternion.Inverse(projectile.transform.rotation);
		projectile.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (0f, -enemyProjectileSpeed);
		AudioSource.PlayClipAtPoint (enemyFireAudio, transform.position, 1f);
	}
}
