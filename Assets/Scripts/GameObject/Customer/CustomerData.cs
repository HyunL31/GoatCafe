using System;
using System.Collections.Generic;

[Serializable]
public class CustomerData
{
    public string Id;
    public string Type;
    public string Race;
    public float MoveSpeed;
    public float DetectionRange;
    public float DetectionAngle;
    public List<string> ItemIds;
}