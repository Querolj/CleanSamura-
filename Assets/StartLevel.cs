using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLevel : MonoBehaviour {
    private string level_name;

	// Use this for initialization
	void Start () {
		
	}
	


    public void SetLevel(string name)
    {
        level_name = name;
    }

    public void Click()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(level_name);
    }
}
