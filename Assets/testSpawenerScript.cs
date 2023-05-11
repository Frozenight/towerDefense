using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testSpawenerScript : MonoBehaviour
{
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;
    // Start is called before the first frame update
    void Start()
    {
      var offset = new Vector3(0, 0, Random.Range(-5, 5));
      Instantiate(Enemy1, transform.position + offset, Quaternion.identity);
      Instantiate(Enemy2, transform.position + offset, Quaternion.identity);
      Instantiate(Enemy3, transform.position + offset, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
