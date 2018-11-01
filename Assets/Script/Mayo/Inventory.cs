using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour{
    private MayoController mc;
    public GameObject[] items = new GameObject[12];
    private GameObject inv_canvas;
    private InventoryGestion inv_gestion;
    private bool open_close = false;
    private QAIGestion QAI; // QUICK ACTION ITEM.
    [HideInInspector]
    public bool opened = false;
    void Start()
    {
        mc = GameObject.Find("Mayo").GetComponent<MayoController>();
        QAI = GameObject.Find("Quick Action Item").GetComponent<QAIGestion>();
        inv_canvas = GameObject.Find("InventoryCanvas");
        inv_gestion = inv_canvas.GetComponent<InventoryGestion>();
        inv_canvas.SetActive(false);
    }
    void Update() //Maintient l'affichage
    {
        if(mc.normal_mode)
        {
            if (Input.GetKeyUp(KeyCode.I))
                open_close = true;
            if (opened && open_close)
            {
                Close();
            }
            else if (open_close)
            {
                open_close = false;
                Display();
            }
        }
    }


    public bool addItem(GameObject item)
    {
        bool itemAdded = false;
        InventoryObject item_info = item.GetComponent<InventoryObject>();
        if (!item_info.can_add)
            return false;
        for(int i = 0; i< items.Length; i++)
        {
            if(items[i] == null)
            {
                item_info.Interaction();
                items[i] = item;
                itemAdded = true;
                item_info.place = i;
                if(item_info.usable)
                    QAI.FullUpdate();
                break;
            }
            else
            {

            }
        }
        return itemAdded;
    }
    public void removeItem(int place)
    {
        Debug.Log("item : " + items[place].name);
        items[place] = null;
    }

    public void Display()
    {
        if(!inv_canvas.activeInHierarchy)
        {
            inv_canvas.SetActive(true);
            inv_gestion.is_updated = false;
            inv_gestion.CompleteInfo();
            inv_gestion.DisplayInfo();
            opened = true;
        }
    }

    public void Close()
    {
        open_close = false;
        opened = false;
        mc.normal_mode = true;
        inv_canvas.SetActive(false);
    }

}
