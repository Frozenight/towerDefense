using System;
[System.Serializable]
public class BuildingMainBaseData: BuildingData {
    public int WorkerLevel;
    public BuildingMainBaseData(int tileIndex, int level, int price, int workerLevel, int type, int healthC, int healthM)
    : base(tileIndex, level, price, type, healthC, healthM) {
        TileIndex = tileIndex;
        Level = level;
        Price = price;
        WorkerLevel = workerLevel;
        BuildingType = type;
        HealthMax = healthM;
        HealthCurrent = healthC;
    }
}