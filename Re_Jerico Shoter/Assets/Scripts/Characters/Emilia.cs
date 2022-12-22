using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Emilia : Personaje
{

    [SerializeField] List<Transform> iceGenerator;
    [SerializeField] GameObject iceBullet;
    private NavMeshAgent agent;
    private Animator anim;
    [SerializeField]private Animator anim2;
    private bool isHurt = false;

    void Start()
    {
        StartCoroutine(IceAtk());
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        agent.Move(Vector3.forward);
    }

    private void LateUpdate()
    {
        if (!isHurt)
        {
            agent.destination = GetPlayerTr().position;
            anim.SetFloat("Speed", agent.velocity.magnitude);
            anim2.SetFloat("Speed", agent.velocity.magnitude);

            float dis = Vector3.Distance(transform.position, GetPlayerTr().position);

            if(dis <= 6)
            {
                Vector3 direction = GetPlayerTr().transform.position - transform.position;
                direction.y += 1;

                Quaternion rotacion = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, 3 * Time.deltaTime);
            }
            
        }
        else
        {
            agent.destination = transform.position;
        }
    }

    private IEnumerator IceAtk()
    {
        int rnd = Random.Range(0, 5);

        Instantiate(iceBullet, iceGenerator[rnd].position, transform.rotation);

        yield return new WaitForSeconds(0.8f);

        if(!isDeath) StartCoroutine(IceAtk());
    }

    public override void RecibirDaño(int dmg)
    {
        base.RecibirDaño(dmg);

        if (vida <= 0) return;

        anim.Rebind();
        anim2.Rebind();
        anim.SetBool("isHit", true);
        anim2.SetBool("isHit", true);
        isHurt = true;

        StartCoroutine(RecieveDmg());
    }

    IEnumerator RecieveDmg()
    {

        yield return new WaitForSeconds(0.6f);

        anim.SetBool("isHit", false);
        anim2.SetBool("isHit", false);
        isHurt = false;
    }

    public override void Morir()
    {
        if (vida > 0) return;

        GetComponent<CapsuleCollider>().enabled = false;
        isDeath = true;
        agent.enabled = false;
        GetComponent<Emilia>().enabled = false;
        anim.applyRootMotion = true;
        anim.SetBool("isDeath", true);
        anim2.SetBool("isDeath", true);

        Destroy(gameObject, 6f);
    }
}
