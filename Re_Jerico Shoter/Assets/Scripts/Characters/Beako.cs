using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Beako : Personaje
{
    private Rigidbody rb;
    private NavMeshAgent agent;

    [SerializeField]
    private GameObject explodeFX;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {

    }

    private void LateUpdate()
    {
        agent.destination = GetPlayerTr().position;
    }

    public void Movimiento()
    {

    }

    public override void Atacar(IDamageable isDmg)
    {
        Destroy(Instantiate(explodeFX, transform.position, transform.rotation), 1.5f);

        Destroy(gameObject);

        isDmg.RecibirDaño(daño);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            Atacar(GetPlayerTr().GetComponent<IDamageable>());
        }
    }

    public override void Morir()
    {
        if (vida > 0) return;

        Destroy(Instantiate(explodeFX, transform.position, transform.rotation), 1.5f);

        Destroy(gameObject);

        float dis = Vector3.Distance(transform.position, GetPlayerTr().position);
        GetPlayerTr().GetComponent<ThirdPersonShoterController>().Heal(5);

        if (dis <= 3.5f)
        {
            Atacar(GetPlayerTr().GetComponent<IDamageable>());
        }
    }
}
