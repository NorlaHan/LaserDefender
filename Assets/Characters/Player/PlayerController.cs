using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameObject projectilePrefab;
	public GameObject playerPrefab;


	public float playerSpeed = 6f;
	public float playerHealth = 300f;
	public float padding = 0.5f;
	public float projectileSpeed = 12f;
	public float projectileFiringRate = 0.2f;
	public AudioClip playerFireAudio;
	public AudioClip playerHitAudio;
	public AudioClip playerDieAudio;

	private Vector3 playerPos;
	float xmin;
	float xMax;
	float ymin;
	float yMax;
	private ScoreKeeper scoreKeeper;
	private LevelManager levelmanager;
	private PlayerStateKeeper playerState;

	// Use this for initialization
	void Start () {
		// Setting up the boundary of the play zone.
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftDown = Camera.main.ViewportToWorldPoint (new Vector3 (0 ,0 ,distance));
		Vector3 rightTop = Camera.main.ViewportToWorldPoint (new Vector3 (1 ,1 ,distance));
		xmin = leftDown.x + padding;
		xMax = rightTop.x - padding;
		ymin = leftDown.y + padding*2.5f;
		yMax = rightTop.y - padding;
		// Setting up the boundary of the play zone.
		scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
		levelmanager = GameObject.Find ("LevelManager").GetComponent<LevelManager> ();
		playerState = GameObject.FindObjectOfType<PlayerStateKeeper> ();
		playerPos = transform.position;

	}
	
	// Update is called once per frame
	void Update () {
		// Player controlled by the key pressed.
		// Left and right
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
			playerPos.x = Mathf.Clamp (playerPos.x - playerSpeed * Time.deltaTime, xmin, xMax);
			transform.position = playerPos;
		} else if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
			playerPos.x = Mathf.Clamp (playerPos.x + playerSpeed * Time.deltaTime, xmin, xMax);
			transform.position = playerPos;
		}
		// Up and down
		if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S)) {
			playerPos.y = Mathf.Clamp (playerPos.y - playerSpeed * Time.deltaTime, ymin, yMax);
			transform.position = playerPos;
		} else if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) {
			playerPos.y = Mathf.Clamp (playerPos.y + playerSpeed * Time.deltaTime, ymin, yMax);
			transform.position = playerPos;
		}
		// Player PlayerFire
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Keypad0 )) {
			InvokeRepeating ("PlayerFire", 0.0001f, projectileFiringRate);
		}
		if (Input.GetKeyUp (KeyCode.Space) || Input.GetKeyUp (KeyCode.Keypad0)) {
			CancelInvoke ("PlayerFire");
		}
	}

	void PlayerFire(){
		MakeProjectile (new Vector3 (0, 1, 0));
		MakeProjectile (new Vector3 (-1, 3, 0));
		MakeProjectile (new Vector3 (1, 3, 0));
		AudioSource.PlayClipAtPoint (playerFireAudio, transform.position, 1f);
	}

	void MakeProjectile(Vector3 projection){
		GameObject projectile = Instantiate (projectilePrefab, transform.position + new Vector3 (0f, 1f), Quaternion.identity);
		projectile.tag = "playerProjectile";
		projectile.GetComponent<Rigidbody2D>().velocity = projection.normalized*projectileSpeed;
	}

	void OnTriggerEnter2D(Collider2D collider){
		Projectile projectile = collider.gameObject.GetComponent<Projectile> ();
		if (projectile & projectile.tag == "enemyProjectile") {
			Debug.Log ("Player gets hitted");
			projectile.Hit ();
			playerHealth -= projectile.GetDamage ();
			AudioSource.PlayClipAtPoint (playerHitAudio, transform.position, 2f);
			// Update Player health with UI
			playerState.PlayerHealth ();
			if (playerHealth <= 0) {
				//Instantiate (playerPrefab, transform.position, Quaternion.identity);
				PlayerDie();

			}

		}
	}
	void PlayerDie (){
		AudioSource.PlayClipAtPoint (playerDieAudio, transform.position, 2f);
		levelmanager.LoadLevel("Win Screen");
		Destroy (gameObject);
	}

	public void PlayerKillEnemy(int enemySupply){
		playerHealth += enemySupply;
		playerState.PlayerHealth ();
	}

}
