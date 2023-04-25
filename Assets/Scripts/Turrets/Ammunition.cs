using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class Ammunition : MonoBehaviour
{

    private Transform target;
    public float speed = 70f;


    public void Seek(Transform _target){
        target = _target;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target==null){
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position-transform.position;
        float distancePerFrame = speed*Time.deltaTime;

        if(dir.magnitude<=distancePerFrame){
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized*distancePerFrame, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }


    public bool HitTarget(){
        
        return true;
    }
}
