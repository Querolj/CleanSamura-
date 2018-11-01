using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("Dialogues")]
public class DialogueDatabase{

    [XmlArray("characters")]
    [XmlArrayItem("character")]
    public List<Dialogue> dialogues = new List<Dialogue>();

}
