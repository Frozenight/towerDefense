using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Show_BuildingSelection : MonoBehaviour
{
    [SerializeField] private RectTransform panel;

    private void Start()
    {
        TileOnWhichToPlace.SetBoundaries(panel.GetComponent<RectTransform>());
    } 
}
