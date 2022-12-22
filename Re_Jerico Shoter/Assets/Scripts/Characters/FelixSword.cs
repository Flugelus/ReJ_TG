using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FelixSword : MonoBehaviour
{
    private int dmg;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Atacar(other.GetComponent<IDamageable>());
        }
    }

    public void Atacar(IDamageable isDmg)
    {
        isDmg.RecibirDa�o(dmg);
        GetComponent<BoxCollider>().enabled = false;
    }

    public void SetDmg(int e)
    {
        dmg = e;
    }
}
