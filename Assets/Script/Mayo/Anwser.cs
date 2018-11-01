using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class Anwser {
    [XmlAttribute("id")]
    public int id;

    [XmlArray("lines")]
    [XmlArrayItem("line")]
    public List<string> lines = new List<string>();

}
