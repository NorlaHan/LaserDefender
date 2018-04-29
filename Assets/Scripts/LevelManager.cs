using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	//private ScoreKeeper scorekeeper;
	//private MusicPlayer musicPlayer;
	public static int nowPlayingLevel;

	void GetLevel(){
		//nowPlayingLevel = Application.loadedLevel;
		nowPlayingLevel = SceneManager.GetActiveScene().buildIndex;
	}

	void Start(){
		GetLevel ();
		if (GameObject.Find ("Music Player").GetComponent<MusicPlayer> ()) {
			GameObject.Find ("Music Player").GetComponent<MusicPlayer> ().OnLevelWasLoaded (nowPlayingLevel);
		} else {
			Debug.LogError ("No music player was found");
		}
	}

	public void LoadLevel(string name){
		Debug.Log ("New Level load: " + name);
		//Application.LoadLevel (name);
		SceneManager.LoadScene (name, LoadSceneMode.Single);

		}

	public void QuitRequest(){
		Debug.Log ("Quit requested");
		ScoreKeeper.Reset ();
		Application.Quit ();
	}

}
