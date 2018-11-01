using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeGestion : MonoBehaviour {

    private Image[] life_pics;
    void Start () {
        life_pics = new Image[5];
        for(int i=1;i<6;i++)
        {
            life_pics[i - 1] = GameObject.Find("GeneralSettings/LifeDisplay/Panel/Life"+i).GetComponent<Image>();
        }
    }
	
	public void UpdateLifeDisplayDmg(int new_life, int dmg)
    {
        if (new_life > 5)//testing
            return;
        for(int i= new_life + dmg; i >  new_life ;i--)
        {
            if (i > 0)
                life_pics[i - 1].color = new Color(0, 0, 0, 0);
            else
                return;
        }
    }

    public void UpdateLifeDisplayHeal(int new_life, int heal)
    {
        if (new_life > 5)//testing
            return;
        for (int i = new_life; i > new_life - heal; i--)
        {
            if (i > 0)
                life_pics[i - 1].color = new Color(1, 1, 1, 1);
            else
                return;
        }
    }

}
