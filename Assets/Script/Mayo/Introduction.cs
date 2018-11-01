using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class Introduction {
    [XmlAttribute("id")]
    public int id;

    [XmlArray("lines")]
    [XmlArrayItem("line")]
    public List<string> lines = new List<string>();

}
