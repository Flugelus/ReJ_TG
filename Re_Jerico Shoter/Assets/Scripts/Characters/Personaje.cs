using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public abstract class Personaje : MonoBehaviour, IDamageable
{
    [SerializeField]
    protected int vida;
    private float maxLife;
    [SerializeField]
    protected int velocidad;
    [SerializeField]
    protected int daño;
    protected bool isDeath;

    [Header("Vida")]
    [SerializeField] private TextMeshProUGUI lifeTxt;
    [SerializeField] private Image lifeSld;

    private Transform playerTr;
    [SerializeField] private bool isLoli;

    int IDamageable.vida { get => vida; set => vida = value; }

    private void Awake()
    {
        maxLife = vida;
        playerTr = playerTr = GameObject.Find("Player").transform;

        ShowLife();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Atacar(IDamageable isdmg)
    {

    }

    public virtual void RecibirDaño(int dmg)
    {
        vida = vida - dmg;

        if (isLoli) playerTr.GetComponent<PlayerStructure>().IncreaseRage(3);
        else playerTr.GetComponent<PlayerStructure>().IncreaseRage(10);

        if (vida < 0) vida = 0;

        ShowLife();

        Morir();
    }

    public virtual void Morir()
    {
        if(vida <= 0)
        {
            Destroy(gameObject);
            isDeath = true;
        }
    }

    public Transform GetPlayerTr()
    {
        return playerTr;
    }

    private void ShowLife()
    {
        lifeTxt.text = vida + " / " + maxLife;
        lifeSld.fillAmount = vida / maxLife;
    }
}
