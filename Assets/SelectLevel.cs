using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SelectLevel : MonoBehaviour {
    public Sprite levelImage;
    public string scene_name = null;
    [TextArea]
    public string description;
    [TextArea]
    public string objectif;

    private Image display;
    private TextMeshProUGUI desc_display;
    private TextMeshProUGUI obj_display;
    private StartLevel sl;
    private void Start()
    {
        display = GameObject.Find("Menu/Title/LevelImage").GetComponent<Image>();
        desc_display = GameObject.Find("Menu/Description/Desc").GetComponent<TextMeshProUGUI>();
        obj_display = GameObject.Find("Menu/Description/Obj").GetComponent<TextMeshProUGUI>();
        sl = GameObject.Find("Menu/StartLevel").GetComponent<StartLevel>();
    }
    
    public void ChooseArena()
    {
        display.sprite = levelImage;
        desc_display.text = description;
        obj_display.text = objectif;
        if (scene_name != null)
            sl.SetLevel(scene_name);
        else
            Debug.LogError("Pas de nom de scene");

    }
}
