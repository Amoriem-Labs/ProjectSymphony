using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {

	public Animator animator;

	public int levelMarker;

	private int levelToLoad;
	
	// Update is called once per frame
	// void Update () {
	// 	if (Input.GetMouseButtonDown(0))
	// 	{
	// 		FadeToNextLevel();
	// 	}
	// }

	// public void FadeToNextLevel ()
	// {
	// 	FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
	// }

	public void FadeToLevel (int level) // int levelIndex)
	{
		//levelToLoad = levelIndex;

		levelMarker = level;
		
		animator.SetTrigger("FadeOut");
	}

	public void OnFadeComplete ()
	{
		Scene scene = SceneManager.GetActiveScene();
        Debug.Log("Active Scene is '" + scene.name + "'.");
		if (levelMarker == 1)
		{
			SceneManager.LoadScene(1);
		}
		else if (levelMarker== 2)
		{
			SceneManager.LoadScene("Ending Screen");
		}
		
		
		
	}
}
