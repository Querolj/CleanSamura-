using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class Dialogue {
    [XmlAttribute("id")]
    public string name;

    [XmlArray("introductions")]
    [XmlArrayItem("introduction")]
    public List<Introduction> Introductions = new List<Introduction>();

    [XmlArray("questions")]
    [XmlArrayItem("question")]
    public List<Question> Questions = new List<Question>();

}
