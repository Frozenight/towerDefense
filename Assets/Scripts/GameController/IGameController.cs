using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameController
{
    void LoadData(GameData data);
    void SaveData(ref GameData data);
}
