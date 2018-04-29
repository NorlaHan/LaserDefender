using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Text playerScore = GetComponent<Text> ();
		playerScore.text = ScoreKeeper.playerScore.ToString();
		ScoreKeeper.Reset ();
	}

}
