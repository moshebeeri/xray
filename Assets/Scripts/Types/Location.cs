using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Location
{
    public String ID { get; set; }
    public String Name { get; set; }
    public String Info { get; set; }
    public float Lat { get; set; }
    public float Long { get; set; }
    public String videoURL { get; set; }
}
