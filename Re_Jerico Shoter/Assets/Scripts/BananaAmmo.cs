using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaAmmo : MonoBehaviour
{
    [SerializeField] private int ammo;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<ThirdPersonShoterController>().MoreAmmo(ammo);
            Destroy(gameObject);
        }
    }
}
