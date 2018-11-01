using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGestion : MonoBehaviour {
    private MayoController mc;
    private Image[] slot_bg = new Image[12];
    private InventoryObject[] slot_info = new InventoryObject[12];
    private Image[] slot_Image = new Image[12];
    private Sprite selected;
    private Sprite not_selected;
    private Inventory inventory;
    private Text item_name;
    private Text item_desc;
    private Text item_weight;
    public bool is_updated = false;
    private int loc = 0;

	// Use this for initialization
	void Start () {
        mc = GameObject.Find("Mayo").GetComponent<MayoController>();
        item_name = GameObject.Find("ItemTitle").GetComponent<Text>();
        item_desc = GameObject.Find("DescriptionText").GetComponent<Text>();
        item_weight = GameObject.Find("WeightText").GetComponent<Text>();
        inventory = GameObject.Find("Mayo").GetComponent<Inventory>();

        slot_bg[0] = GameObject.Find("Slot").GetComponent<Image>();
        
        slot_Image[0] = GameObject.Find("Slot/Item").GetComponent<Image>();
        for (int i = 1;i<12;i++)
        {
            slot_bg[i] = GameObject.Find("Slot ("+i+")").GetComponent<Image>();
            slot_Image[i] = GameObject.Find("Slot (" + i + ")/Item").GetComponent<Image>();
        }
        selected = slot_bg[0].sprite;//Resources.Load<Sprite>("white_inlay");
        not_selected = slot_bg[1].sprite;//Resources.Load<Sprite>("white");        

    }
	
	// Update is called once per frame
	void Update () {
		if(inventory.opened)
        {
            mc.normal_mode = false;
            if (!is_updated)
            {
                is_updated = true;
                DisplayUpdate();
            }
            
            if (Input.GetKeyDown("down"))
                Down();
            if (Input.GetKeyDown("up"))
                Up();
            if (Input.GetKeyDown("left"))
                Left();
            if (Input.GetKeyDown("right"))
                Right();
            //DisplayInfo();
        }
    }

    void Up()
    {
        if(loc > 3)
        {
            DisplayChange(loc);
            loc -= 4;
            slot_bg[loc].sprite = selected;
        }
        DisplayInfo();
    }
    void Down()
    {
        if(loc < 8)
        {
            DisplayChange(loc);
            loc += 4;
            slot_bg[loc].sprite = selected;
        }
        DisplayInfo();
    }
    void Right()
    {
        if (loc != 3 && loc != 7 && loc != 11)
        {
            DisplayChange(loc);
            loc++;
            slot_bg[loc].sprite = selected;
        }
        DisplayInfo();
    }
    void Left()
    {
        if (loc != 0 && loc != 4 && loc != 8)
        {
            DisplayChange(loc);
            loc--;
            slot_bg[loc].sprite = selected;
        }
        DisplayInfo();
    }
    void DisplayChange(int oldloc)
    {
        slot_bg[loc].sprite = not_selected;
    }
    public void CompleteInfo()
    {
        if (inventory.items[0] != null)
            slot_info[0] = inventory.items[0].GetComponent<InventoryObject>();//GameObject.Find("Slot").GetComponent<InventoryObject>();
        for(int i = 1;i<12;i++)
        {
            if(inventory.items[i] != null)
                slot_info[i] = inventory.items[i].GetComponent<InventoryObject>();
        }
    }
    void DisplayUpdate()
    {
        CompleteInfo();
        for (int i = 0;i<12; i++)
        {
            Color c = slot_Image[i].color;
            //Debug.Log("a :" + slot_info[i].item_name);
            if (slot_info[i] != null)
            {
                slot_Image[i].sprite = slot_info[i].inventory_sprite;
                c.a = 255;
                slot_Image[i].color = c;
            }
            else
            {
                c.a = 0;
                slot_Image[i].color = c;
            }
        }
    }
    public void DisplayInfo()
    {
        if(slot_info[loc] != null)
        {
            item_name.text = slot_info[loc].item_name;
            item_desc.text = slot_info[loc].description;
            item_weight.text = slot_info[loc].weight.ToString();
        }
        else
        {
            item_name.text = "";
            item_desc.text = "";
            item_weight.text = "";
        }
    }
}
