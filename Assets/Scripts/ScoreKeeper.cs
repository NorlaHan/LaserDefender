using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {

	public static int playerScore = 0;
	private Text scoreText; 

	void Start(){
		scoreText = GetComponent<Text> ();
		Score (0);
	}

	public void Score(int enemyValue){
		Debug.Log ("player score activated");
		playerScore += enemyValue;
		scoreText.text = "Score = " + playerScore;

	}

	public static void Reset(){
		playerScore = 0;
	}
}
