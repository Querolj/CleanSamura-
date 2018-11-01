using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LifePic : MonoBehaviour {
    public int life_num;
    private SamuraiController sc;
    private Image image;
    private Color tr;
	// Use this for initialization
	void Start () {
        tr = new Color(0, 0, 0, 0);
        image = this.GetComponent<Image>();
        sc = GameObject.Find("Samuraï").GetComponent<SamuraiController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (sc.life == life_num -1)
            image.color = tr;
	}
}
