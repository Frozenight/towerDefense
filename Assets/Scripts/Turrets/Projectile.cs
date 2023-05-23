using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody body;
    public Transform target;

    private bool hit = false;
    private float damage;
    [SerializeField] private LayerMask whatToHit;
    [SerializeField] private GameObject explosion;
    private GameObject explosionInstance;
    private float lifetime = 0;

    private float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        //target.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        lifetime += Time.deltaTime;
        if (!hit)
        {
            transform.forward = body.velocity;
        }

        if(lifetime > 7f) { Destroy(gameObject); }

        if (hit)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 4f, whatToHit);
        Explode(colliders);
    }

    public void Explode(Collider[] colliders)
    {
        if (!hit)
        {
            foreach (Collider c in colliders)
            {
                if(c.GetComponent<EnemyHealth>() != null)
                {
                    c.GetComponent<EnemyHealth>().GetHit(damage);
                }
            }
            hit = true;
            explosionInstance = (GameObject)Instantiate(explosion, this.transform.position, Quaternion.identity);

            Destroy(body);
            Destroy(GetComponent<SphereCollider>());
        }
    }

    public void Go(Vector3 speed, Vector3 targetPos)
    {
        body.velocity = speed;
        target.position = targetPos;
    }

    public void SetDamage(float _damage)
    {
        damage = _damage;
    }
}