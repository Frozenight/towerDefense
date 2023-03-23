using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceHandler : MonoBehaviour
{


    public Text resourceText;
    public Text timeText;

    int resource =0;
    float time = 0; 

    // Start is called before the first frame update
    void Start()
    {
        resourceText.text=resource.ToString()+" RESOURCES";
        timeText.text = time.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        time=Time.time;
    }
}