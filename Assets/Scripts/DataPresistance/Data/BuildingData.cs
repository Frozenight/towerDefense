using System;

[System.Serializable]
public class BuildingData {
    public int TileIndex;
    public int Level;
    public int Price;
    public int BuildingType;
    public BuildingData(int tileIndex, int level, int price, int type) {
        TileIndex = tileIndex;
        Level = level;
        Price = price;
        BuildingType = type;
    }
}