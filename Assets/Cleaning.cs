using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaning : MonoBehaviour {

    private Dictionary<float, Dictionary<float, List<SpriteRenderer>>> splash_locations = new Dictionary<float, Dictionary<float, List<SpriteRenderer>>>(); //1er float : y, 2eme float = x, avec le sprite du sang sur cette location
    private const float blood_range = 0.05f;

	void Update () {
        int c = 0;
        if (Input.GetKeyUp(KeyCode.P)) //Stop Cleaning
        {
            foreach (float key in splash_locations.Keys)
            {
                foreach (float key_x in splash_locations[key].Keys)
                {
                    c++;
                    print(key.ToString() + " : " + key_x.ToString());
                }

            }
            print("val de c : " + c);
        }
    }

    public void Add(float x, float y, SpriteRenderer sprite) //TODO : SI on trouve déjà une clé Y assez proche, on ajoute
    {
        bool in_list = false;
        foreach (float key in splash_locations.Keys)
        {
            //print(x + " : " + y+ ", et key_y : "+y);
            if (key > y - blood_range && key < y + blood_range) //Intervalle supposé de location d'un sol 
            {
                in_list = true;
                if (splash_locations[key].ContainsKey(x))
                    splash_locations[key][x].Add(sprite); //Pas besoin de check les x, on remplace direct
                else
                {
                    //print(x.ToString() + " : " + y.ToString());
                    Dictionary<float, List<SpriteRenderer>> tmp = new Dictionary<float, List<SpriteRenderer>>();
                    List<SpriteRenderer> list_tmp = new List<SpriteRenderer>();
                    list_tmp.Add(sprite);
                    splash_locations[key].Add(x, list_tmp);
                }
            }
        }
        if(!in_list && !splash_locations.ContainsKey(y))
        {
            Dictionary<float, List<SpriteRenderer>> tmp = new Dictionary<float, List<SpriteRenderer>>();
            List<SpriteRenderer> list_tmp = new List<SpriteRenderer>();
            list_tmp.Add(sprite);
            tmp.Add(x, list_tmp);
            splash_locations.Add(y, tmp);

        }
        else if(!in_list)
        {
            print("fait rien..");
        }

        /*
        int c = 0;
        foreach (float key in splash_locations.Keys)
        {
            foreach (float key_x in splash_locations[key].Keys)
            {
                c++;
                print(key.ToString() + " : " + key_x.ToString());
            }
                
        }
        print("val de c : " + c);*/
    }
    public Dictionary<float, List<SpriteRenderer>> GetAllSplashOfY(float y) //Retourne la liste de sprite de sang au niveau y, si pas de sang, retourne null. le y de la liste doit être < sc.y & > sc.y -0.4
    {
        foreach(float key in splash_locations.Keys)
        {
            if(key>y - blood_range && key < y + blood_range) //Intervalle supposé de location d'un sol 
            {
                return splash_locations[key];
            }
        }
        return null;
    }

    public bool Remove(float x_to_remove, float y) //y = le niveau
    {
        foreach (float key_y in splash_locations.Keys)
        {
            if (key_y  > y - blood_range && key_y < y + blood_range) //Intervalle supposé de location d'un sol 
            {
                if (splash_locations[key_y].ContainsKey(x_to_remove))
                {
                    splash_locations[key_y].Remove(x_to_remove);
                    return true;
                }
            }
        }
        return false;

    }


}
