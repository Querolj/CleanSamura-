using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QAIGestion : MonoBehaviour {
    private MayoController mc;
    private Inventory inventory;
    private InventoryObject[] slot_info_us = new InventoryObject[12];
    private int current_place = 1;
    private Image[] places = new Image[3]; //0 = left, 1 = center, 2 = right
    private int[] free_spaces = new int[3];
    private int used = -1; //Pour supprimer les objets de slot_infos_us
    void Start () {
        //inv_gestion = GameObject.Find("InventoryCanvas").GetComponent<InventoryGestion>();
        /*item_selected = GameObject.Find("ItemSelectedSprite").GetComponent<Image>();
        item_right = GameObject.Find("ItemRight").GetComponent<Image>();
        item_left = GameObject.Find("ItemLeft").GetComponent<Image>();*/
        GameObject mayo = GameObject.Find("Mayo");
        mc = mayo.GetComponent<MayoController>();
        inventory = mayo.GetComponent<Inventory>();
        places[0] = GameObject.Find("ItemLeft").GetComponent<Image>();
        places[1] = GameObject.Find("ItemSelectedSprite").GetComponent<Image>();
        places[2] = GameObject.Find("ItemRight").GetComponent<Image>();
        for (int j = 0; j < 3; j++)
            free_spaces[j] = -1;
        CompleteInfo();
        Display();
    }
	
	// Update is called once per frame
	public void MayoUpdate() {
        if(mc.normal_mode)
            if (Input.GetKeyUp(KeyCode.Q))
                Left();
            if (Input.GetKeyUp(KeyCode.S))
                Right();
            if (Input.GetKeyUp(KeyCode.X) && !inventory.opened)
                Item_action();
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    //Cette fonction permet d'initier le mode lancer et d'autres choses varié en fonction de l'objet sélectionné
    private void Item_action()
    {
        if (places[1].sprite != null && slot_info_us[current_place].throwable)
        {
            used = current_place;
            mc.LaunchThrowingMode(slot_info_us[current_place], slot_info_us[current_place].gameObject);
        }
            
    }

    //------------------------------------------------------------------------------------------------------------------------------------
    public void FullUpdate()
    {
        CompleteInfo();
        Display();
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    void Left()
    {
        if(current_place < 11 && Is_a_usefull_change(false))
        {
            current_place++;
            FullUpdate();
        }
        else
        {
            current_place = 0;
            FullUpdate();
        }
    }
    void Right()
    {
        if(current_place > 0 && Is_a_usefull_change(true))
        {
            current_place--;
            FullUpdate();
        }
        else
        {
            current_place = 11;
            FullUpdate();
        }
    }

    //------------------------------------------------------------------------------------------------------------------------------------
    private bool Is_a_usefull_change(bool d)
    {
        return true;

    }

    //------------------------------------------------------------------------------------------------------------------------------------
    public void CompleteInfo()
    {
        if (inventory.items[0] != null && inventory.items[0].GetComponent<InventoryObject>().usable)
            slot_info_us[0] = inventory.items[0].GetComponent<InventoryObject>();//GameObject.Find("Slot").GetComponent<InventoryObject>();
        for (int i = 1; i < 12; i++)
        {
            if (inventory.items[i] != null && inventory.items[i].GetComponent<InventoryObject>().usable)
                slot_info_us[i] = inventory.items[i].GetComponent<InventoryObject>();
        }
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    void Display()
    {
        //Milieu
        if (slot_info_us[current_place] != null)
            places[1].sprite = slot_info_us[current_place].inventory_sprite;
        else
            places[1].sprite = null;
        //Gauche
        if(current_place != 0 )
        {
            if(slot_info_us[current_place - 1] != null)
                places[0].sprite = slot_info_us[current_place - 1].inventory_sprite;
            else
                places[0].sprite = null;
        }
        else if(current_place == 0)
        {
            if(slot_info_us[11] != null)
                places[0].sprite = slot_info_us[11].inventory_sprite;
            else
                places[0].sprite = null;
        }
        //Droite
        if (current_place != 11)
        {
            if (slot_info_us[current_place + 1] != null)
                places[2].sprite = slot_info_us[current_place + 1].inventory_sprite;
            else
                places[2].sprite = null;
        }
        else if (current_place == 11)
        {
            if(slot_info_us[0] != null)
                places[2].sprite = slot_info_us[0].inventory_sprite;
            else
                places[2].sprite = null;
        }
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    bool IsIn(int i)
    {
        for (int j = 0; j < 3; j++)
        {
            if (free_spaces[j] == i)
                return true;
        }
        return false;
    }
    bool Space()
    {
        for (int j = 0; j < 3; j++)
        {
            if (free_spaces[j] == -1)
                return true;
        }
        return false;
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    public void removeUsedItem()
    {
        places[1].sprite = null;
        if (used != -1)
            slot_info_us[used] = null;
        used = -1;
    }
    //------------------------------------------------------------------------------------------------------------------------------------
}
