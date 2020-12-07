using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour {

	public void startGame()
	{
        SceneManager.LoadScene("World");
	}

	public void quitGame(){
         UnityEditor.EditorApplication.isPlaying = false;
		 //Application.Quit();
	}


}

