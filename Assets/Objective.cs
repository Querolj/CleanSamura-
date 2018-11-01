using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Objective : MonoBehaviour {
    public bool enable_objective;

    private SamuraiController sc;
    private GameObject restart;
    private Text restart_text;
    private bool objectif_competed = false;
    // Use this for initialization
    void Start () {
        sc = GameObject.Find("Samuraï").GetComponent<SamuraiController>();
        restart = GameObject.Find("GeneralSettings/LifeDisplay/Restart");
        restart_text = GameObject.Find("GeneralSettings/LifeDisplay/Restart/Text").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
		if(enable_objective )
        {
            if (objectif_competed)
            {
                restart_text.text = "Super, il ne reste plus d'ennemi donc tu as gagné. Tu veux un biscuit ? Y pour recommencer";
                restart.SetActive(true);
                enable_objective = false;
            }
        }
        else if(objectif_competed && Input.GetKeyDown(KeyCode.Y))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
	}

    public void ObjectifCompleted()
    {
        objectif_competed = true;
    }
}
