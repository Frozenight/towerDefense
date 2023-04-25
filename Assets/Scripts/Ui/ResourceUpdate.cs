using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceUpdate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        text.text = GameController.instance.resources.ToString();
    }
}
