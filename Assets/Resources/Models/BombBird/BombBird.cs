using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBird : Actionable
{

    Rigidbody rb;

    public GameObject PoofSound;
    public GameObject Poof;
    public float explosionForce = 300.0f;
    public float explosionRadius = 10.0f;
    public float upForce = 1.0f;
    public bool hasCollided = false;
    private bool hasExploded = false;
    private Vector3 explosionPosition;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = Mathf.Infinity;
    }

    void detonate()
    {
        Debug.Log("Detonation!");
        explosionPosition = transform.position; //center of the explosion originates from transform.position
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Debug.Log("rigidbody " + hit.transform + " registered");
            Rigidbody r = hit.GetComponentInParent<Rigidbody>(); //rigidbody contained in parent of collider
            if (hit.name == "PigGraphics" || hit.name == "WoodPlankGraphics")
            {
                //only add explosion force to the pig and plank
                if (r != null)
                {
                    Debug.Log("applied explosion to " + r.transform);
                    r.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, upForce, ForceMode.Impulse);
                }
            }
        }
        hasExploded = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!hasExploded)
        {
            detonate();
        }
    }

    public override IEnumerable HandleCollision(Collision collision)
    {
        if (!hasCollided)
        {
            GameObject newPoofSound = GameObject.Instantiate(PoofSound);
            Vector3 direction = (collision.gameObject.transform.position - newPoofSound.transform.position).normalized;
            newPoofSound.transform.position += direction;

            //GameObject.Instantiate(Poof);
            //Vector3 direction1 = (collision.gameObject.transform.position - Poof.transform.position).normalized;
            //Poof.transform.position += direction1;

            GameStatus.instance.SpawnNextBird();
            yield return new WaitForSeconds(1);

            GameObject.Destroy(newPoofSound);
            //GameObject.Destroy(Poof);
            //GameObject.Destroy(collision.gameObject);
            
            
            hasCollided = true;
        }

        yield return null;
    }
}

