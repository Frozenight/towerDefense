using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public List<TrashObject> trashObjects;
    public GameOverScreen gameOverScreen;

    private void Awake()
    {
        instance = this;
    }

    public void AddTrash(TrashObject trash)
    {
        trashObjects.Add(trash);
    }

    public void GameOver()
    {
        gameOverScreen.Setup();
    }
}
