using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

[Serializable]
public class Tour
{
    public String TourId { get; set; }
    public String CountryId { get; set; }
    public String CityId { get; set; }
    public String Id { get; set; }
    public String Name { get; set; }
    public OrderedDictionary Locations { get; set; } = new OrderedDictionary();
}
