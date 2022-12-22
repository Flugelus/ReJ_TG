using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject hitVFX;
    [SerializeField] private int dmg;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        float speed = 35f;
        rb.velocity = transform.forward * speed;
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime * 33f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(hitVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);

        if (collision.gameObject.GetComponent<Personaje>())
        {
            ImpactoEnemigo(collision.transform.GetComponent<Personaje>(), dmg);
        }
    }

    private void ImpactoEnemigo(IDamageable isDmg, int dmg1)
    {
        isDmg.RecibirDaño(dmg1);
    }
}
