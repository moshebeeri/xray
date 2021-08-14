using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

[Serializable]
public class Tourist
{
    public String UserId { get; set; }
    public String CurrentTour { get; set; }
    public String CurrentLocation { get; set; }
}
