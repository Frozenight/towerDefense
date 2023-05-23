using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Show_BuildingSelection : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private GameObject tower2Img;

    private void Start()
    {
        TileOnWhichToPlace.SetBoundaries(panel.GetComponent<RectTransform>());
    }

    public void showTwr2()
    {
        tower2Img.SetActive(true);
    }
}
