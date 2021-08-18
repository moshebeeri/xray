using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

[Serializable]
public class Tourist
{
    public Tourist( String UserId ,
                    String CurrentTourId,
                    String CurrentLocationId)
    {
        this.UserId = UserId;
        this.CurrentTourId = CurrentTourId;
        this.CurrentLocationId = CurrentLocationId;
    }
    public String UserId { get; set; }
    public String CurrentTourId { get; set; }
    public String CurrentLocationId { get; set; }
}
