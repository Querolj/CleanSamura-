using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

public class DialogueGestion : MonoBehaviour {
    DialogueDatabase dialogue_db;
    [HideInInspector]
    public bool is_loaded = false;
    void Start () {
	}
	
	void Update () {
		
	}

    public void Load()
    {
        var serializer = new XmlSerializer(typeof(DialogueDatabase));
        using (var stream = new FileStream("Assets/xml/dialogues/Dialogue.xml", FileMode.Open))
        {
            dialogue_db = serializer.Deserialize(stream) as DialogueDatabase;
        }
        is_loaded = true;
    }
    
    //------------------------------------------------------------------------------------------------------------------------------------
    public Dialogue getCharacter(string name)
    {
        if (dialogue_db == null)
            Debug.Log("wat");
        for(int i = 0;i< dialogue_db.dialogues.Count;i++)
        {
            if (dialogue_db.dialogues[i].name.Equals(name))
                return dialogue_db.dialogues[i];
        }
        Debug.Log("Le dialogue pour le personnage " + name + " n'existe pas");
        return null;
    }
    /*private void Display()
    {
        for (int i = 0; i < dialogues.characters[0].Questions.Count; i++)
        {
            if (dialogues.characters[0].Questions[i].displayed == 1)
            {
                //Affiche la question dans la bulle
            }
        }
    }
    */
}
