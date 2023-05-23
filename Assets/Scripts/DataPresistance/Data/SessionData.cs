using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SessionData
{
    public float workerSpeed;
    public int completedWaves;
    public int resources;
    public List<BuildingData> buildings;

    public SessionData()
    {
        buildings = new List<BuildingData>();
    }
}
