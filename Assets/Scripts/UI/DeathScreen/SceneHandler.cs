using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneHandler : MonoBehaviour {

	public Animator animator;
	private int sceneToLoad;

	public void DeathScene()
	{	
		sceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
		animator.SetTrigger("fadeOut");

	}

	public void OnFadeComplete ()
	{
		SceneManager.LoadScene(sceneToLoad);
	}


}