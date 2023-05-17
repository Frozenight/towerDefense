using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SessionData
{
    public List<BuildingData> buildings;

    public SessionData()
    {
        buildings = new List<BuildingData>();
    }
}
