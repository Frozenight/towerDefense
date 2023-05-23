using System;

[System.Serializable]
public class BuildingData {
    public int TileIndex;
    public int Level;
    public int Price;
    public int BuildingType;
    public int HealthMax;
    public int HealthCurrent;
    public BuildingData(int tileIndex, int level, int price, int type, int healthC, int healthM) {
        TileIndex = tileIndex;
        Level = level;
        Price = price;
        BuildingType = type;
        HealthMax = healthM;
        HealthCurrent = healthC;
    }
}