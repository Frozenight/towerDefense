using System;
[System.Serializable]
public class BuildingMainBaseData: BuildingData {
    public int WorkerLevel;
    public BuildingMainBaseData(int tileIndex, int level, int price, int workerLevel, int type)
    : base(tileIndex, level, price, type) {
        TileIndex = tileIndex;
        Level = level;
        Price = price;
        WorkerLevel = workerLevel;
        BuildingType = type;
    }
}