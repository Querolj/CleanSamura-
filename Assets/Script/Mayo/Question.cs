using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class Question {
    [XmlAttribute("id")]
    public int id;

    public int displayed;

    public string content;

    [XmlArray("anwsers")]
    [XmlArrayItem("anwser")]
    public List<Anwser> Anwsers = new List<Anwser>();

    public string path;

}
