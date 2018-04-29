using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateKeeper : MonoBehaviour {

	Text playerHealth;
	PlayerController player;


	// Use this for initialization
	void Start () {
		player = GameObject.FindObjectOfType<PlayerController> ();
		PlayerHealth ();
	}

	public void PlayerHealth(){
		GetComponent<Text>().text = ("Hit Point " + player.playerHealth);
	}
}
