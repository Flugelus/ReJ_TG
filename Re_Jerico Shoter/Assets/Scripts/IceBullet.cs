using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBullet : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;
    public int dmg;
    public GameObject hitVFX;
    private Transform playerTr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerTr = GameObject.Find("Player").transform;

        Vector3 direction = playerTr.transform.position - transform.position;
        direction.y += 1;
        Quaternion rotacion = Quaternion.LookRotation(direction);
        transform.rotation = rotacion;

        rb.velocity = transform.forward * speed;
    }


    public void Atacar(IDamageable isDmg)
    {
        Destroy(gameObject);

        isDmg.RecibirDaño(dmg);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            Atacar(collision.transform.GetComponent<IDamageable>());
        }
        else
        {
            Destroy(gameObject);
        }

        Destroy(Instantiate(hitVFX, transform.position, transform.rotation), 1.5f);
    }
}
