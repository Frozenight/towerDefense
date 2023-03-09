using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashManager : MonoBehaviour
{
    public static TrashManager instance;
    public List<TrashObject> trashObjects;

    private void Awake()
    {
        instance = this;
    }

    public void AddTrash(TrashObject trash)
    {
        trashObjects.Add(trash);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
