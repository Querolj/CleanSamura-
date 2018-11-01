using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * Chemin tips : On ne peut pas finir un chemin avec une * !!
 * Prendre en compte l'ordre dans lequel les chemins ont écrits
 * 
 * TODO : Ajouter dans la syntaxe des chemins le char '0', ce chemin doit toujours être en dernier dans l'ordre
 * et il représente le cas ou aucun chemin ne match
 * 
 */ 
public class Character : MonoBehaviour {
    public string char_name;
    private Text[] choices = new Text[8];
    private Text dialogue_display;
    private Dialogue char_dialogue;
    private int intro_id = 0;
    private List<string> current_dialogue = new List<string>();
    public Dictionary<int, string> current_choices = new Dictionary<int, string>();
    private string current_path = "";
    private int choice_selected = 0;
    private bool choice_displayed = false;
    private bool out_dial = false;
    private MayoController mc;
    void Start () {
        mc = GameObject.Find("Mayo").GetComponent<MayoController>();

        if (name != null)
        {
            dialogue_display = GameObject.Find(this.name+ "/DialogueCanvas/DialogueText").GetComponent<Text>();
            GameObject ds = GameObject.Find("Mayo/Dialogue System");
            if(ds.GetComponent<DialogueGestion>().is_loaded)
                char_dialogue = ds.GetComponent<DialogueGestion>().getCharacter(char_name);
            else
            {
                ds.GetComponent<DialogueGestion>().Load();
                char_dialogue = ds.GetComponent<DialogueGestion>().getCharacter(char_name);
            }

            for (int i=1;i<9;i++)
            {
                choices[i-1] = GameObject.Find("Choice" + i).GetComponent<Text>();
            }
        }

	}


    void Update () {
        
		if(current_dialogue.Count>0)
        {
            Debug.Log("du dial");
            if (Input.GetKeyUp(KeyCode.E))
            {
                dialogue_display.text = current_dialogue[0];
                current_dialogue.RemoveAt(0);
            }
        }
        else if(out_dial)
        {
            //Stop le dialogue
            out_dial = false;
            mc.StopDialogue();
        }
        else if(choice_displayed) //Pour avoir le temps que ca display
        {
            //alpha 255 pour choices
            //choices[choice_selected].color = Color.red;
            MakingChoice();
            if (Input.GetKeyUp(KeyCode.E))
                ChoiceMade();
        }
	}
    //------------------------------------------------------------------------------------------------------------------------------------
    public void startDialogue()
    {
        //Ranger tous les choix actuels dans current_choices
        for (int i = 0; i < char_dialogue.Questions.Count; i++)
        {
            if (char_dialogue.Questions[i].displayed == 1)
            {
                current_choices.Add(char_dialogue.Questions[i].id, char_dialogue.Questions[i].content);
            }
        }
        //Range l'intro choisit dans current_dialogue
        for (int i = 0; i < char_dialogue.Introductions[intro_id].lines.Count; i++)
        {
            current_dialogue.Add(char_dialogue.Introductions[intro_id].lines[i]);
        }
        dialogue_display.text = current_dialogue[0];
        current_dialogue.RemoveAt(0);

        DisplayChoices();
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    private void DisplayChoices()
    {
        choice_selected = 0;
        Debug.Log("count choice : " + current_choices.Count);
        //current_choices.Clear();
        int j = 0;
        for (int i = 0;i< char_dialogue.Questions.Count;i++)
        {
            if(current_choices.ContainsKey(i+1))
            {
                choices[j].text = current_choices[i + 1]; //i+1 car l'index représente la clé qui est l'id §§§§§§§§§§§ BUG LA
                choices[j].color = Color.white;
                choices[j].GraphicUpdateComplete();
                j++;
            }
        }
        for(int i = j; i < char_dialogue.Questions.Count; i++)
        {
            choices[i].text = "";
            choices[i].GraphicUpdateComplete();
        }
        choices[0].color = Color.red;
        choice_displayed = true;

    }
    //------------------------------------------------------------------------------------------------------------------------------------
    public void MakingChoice()
    {
        if (Input.GetKeyUp("up"))
        {
            if (choice_selected - 1 >= 0)
            {
                choice_selected -= 1;
                choices[choice_selected].color = Color.red;
                choices[choice_selected + 1].color = Color.white;
            }
        }
        else if (Input.GetKeyUp("down"))
        {
            if (choice_selected + 1 < current_choices.Count)
            {
                choice_selected += 1;
                choices[choice_selected].color = Color.red;
                choices[choice_selected - 1].color = Color.white;
            }
        }

    }
    //------------------------------------------------------------------------------------------------------------------------------------
    private void ChoiceMade()
    {
        //AddToPath(choice_selected);
        DetermineAnwserProcess();
        choice_selected = 0;
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    private void AddToPath(int question)
    {
        if (current_path.Equals(""))
            current_path += question.ToString();
        else
            current_path += "-" + question.ToString();
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    private void DetermineAnwserProcess()
    {
        int id = -1;
        string result;
        for (int i = 0; i < char_dialogue.Questions.Count; i++)
        {
            if (char_dialogue.Questions[i].content.Equals(choices[choice_selected].text))
            {
                id = char_dialogue.Questions[i].id;
                break;
            }
        }
        char[] sep = { ' ' };
        if(id != -1)
        {
            AddToPath(id);
            string[] paths = char_dialogue.Questions[id-1].path.Split(sep);
            for (int i = 0; i < paths.Length; i++)
            {
                if((result = CheckPath(paths[i])) != null)
                {
                    Debug.Log("correct path : "+id);
                    ExcecuteChange(result, id);
                    break;
                }
            }
        }
        else
        {
            Debug.Log("Error Dialogue : "+current_path);
        }
        
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    private string CheckPath(string path)
    {
        char[] sep = { ':' };
        string pre_path = path.Split(sep)[0];
        Debug.Log(pre_path+" ,our path : "+current_path);
        string result = path.Split(sep)[1];

        //Vérifie si le chemin correspond
        char[] rep = { '-' };
        string[] check_quest_path = pre_path.Split(rep);
        if (check_quest_path[0].Equals("0")) //Le chemin est forcément bon quand il est 0
            return result;
        string[] check_cur_path = current_path.Split(rep);
        int j = 0;
        //if (check_cur_path.Length < check_quest_path.Length)
        //    return null;
        for(int i = 0;i< check_cur_path.Length;i++)
        {
            if (check_cur_path[i].Equals(check_quest_path[j]))
            {
                j++;
            }
            else if (check_quest_path[j].Equals("*"))
            {
                if (check_quest_path[j + 1].Equals(check_cur_path[i])) //att out
                    j += 2;
                else if (i == check_cur_path.Length - 1)
                    return null;
            }
            else
                return null;
            if(i == check_cur_path.Length - 1 && j == check_quest_path.Length - 1)
            {
                return null;
            }
        }
        return result;
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    private void ExcecuteChange(string result, int id)
    {
        char[] sep = { '|' };
        char[] rep = { ',' };
        string[] results = result.Split(sep);
        Debug.Log("following path : " + result);
        AddingChoice(results[0].Split(rep));
        DeleteChoice(results[1].Split(rep));
        IntroChange(results[2]);
        AnwserChoosed(results[3], id);
        if (results[4].Equals("o"))
            out_dial = true;
        choice_displayed = false;
        DisplayChoices();
        
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    private void AddingChoice(string[] results)
    {
        int k;
        for (int i = 0; i < results.Length; i++)
        {
            k = int.Parse(results[i]);
            if (k != 0 && !current_choices.ContainsKey(k))
                current_choices.Add(k, char_dialogue.Questions[k - 1].content);
            else
                break;
        }
            
    }
    private void DeleteChoice(string[] results)
    {
        int k;
        for (int i = 0; i < results.Length; i++)
        {
            k = int.Parse(results[i]);
            if (k != 0)
            {
                current_choices.Remove(k);
                //Debug.Log("remove " + k);
            }
                
            else
                break;
        }
    }
    private void IntroChange(string results)
    {
        intro_id = int.Parse(results);
    }
    private void AnwserChoosed(string results, int id)
    {
        int anwser_selected = int.Parse(results) - 1;
        current_dialogue.Clear();
        for (int i = 0; i < char_dialogue.Questions[id-1].Anwsers[anwser_selected].lines.Count;i++)
            current_dialogue.Add(char_dialogue.Questions[id - 1].Anwsers[anwser_selected].lines[i]);
        //Debug.Log("choose res : " + results + ", id " + id + " " + current_dialogue.Count);
    }
}
