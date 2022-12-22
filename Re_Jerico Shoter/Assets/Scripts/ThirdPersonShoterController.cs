using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class ThirdPersonShoterController : MonoBehaviour, IDamageable
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderMask = new LayerMask();
    [SerializeField] private Transform bullet;
    [SerializeField] private Transform spawnBulletPos;
    [SerializeField] private GameObject bulletFX;

    private StarterAssetsInputs starterInputs;
    private ThirdPersonController thirdController;
    private Animator anim;

    public GameObject pointer;
    private float aimLoader = 0f;

    [Header("Municion")]

    [SerializeField] private TextMeshProUGUI totalAmmoTxt;
    [SerializeField] private TextMeshProUGUI currentAmmoTxt;
    [SerializeField] private TextMeshProUGUI ammoChangeTxt;
    [SerializeField] private GameObject bananaR;

    private int totalAmmo = 40;
    private int currentAmmo = 20;
    private float shotLoad = 0.2f;
    private float shotLoadD;
    private int currentAmmoD;
    private bool isReloading = false;

    [Header("Stats")]

    [SerializeField] private TextMeshProUGUI lifeTxt;
    [SerializeField] private TextMeshProUGUI rageTxt;
    [SerializeField] private TextMeshProUGUI lifeChangeTxt;
    [SerializeField] private Image lifeSld;
    [SerializeField] private Image rageSld;

    [SerializeField] private int vida;
    [SerializeField] private Image profile;
    [SerializeField] List<Sprite> profileImgs;

    private int maxVida;
    private bool isApe = false;

    private IEnumerator courutine1 = null;
    private IEnumerator courutine2 = null;

    private PlayerStructure playerS;

    int IDamageable.vida { get => vida; set => vida = value; }

    void Start()
    {
        shotLoadD = shotLoad;
        currentAmmoD = currentAmmo;

        maxVida = vida;

        ShowLife();

        starterInputs = GetComponent<StarterAssetsInputs>();
        thirdController = GetComponent<ThirdPersonController>();
        anim = GetComponent<Animator>();
        playerS = GetComponent<PlayerStructure>();

        currentAmmoTxt.text = currentAmmo.ToString();
        totalAmmoTxt.text = totalAmmo.ToString();
    }

    void Update()
    {
        Vector3 mouseWPos = Vector3.zero;
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderMask))
        {
            mouseWPos = hit.point;
        }

        if (starterInputs.aim && !isReloading)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdController.SetSensitivity(aimSensitivity);

            thirdController.SetRotateOnMove(false);

            anim.SetLayerWeight(1, Mathf.Lerp(anim.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

            Vector3 wAimTarget = mouseWPos;
            wAimTarget.y = transform.position.y;
            Vector3 aimDir = (wAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * 20f);

            pointer.SetActive(true);

            aimLoader += 1 * Time.fixedDeltaTime;
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdController.SetSensitivity(normalSensitivity);

            thirdController.SetRotateOnMove(true);

            anim.SetLayerWeight(1, Mathf.Lerp(anim.GetLayerWeight(1), 0f, Time.deltaTime * 10f));

            pointer.SetActive(false);

            if(aimLoader != 0f)
            {
                aimLoader = 0f;
            }
        }

        if (starterInputs.shot && starterInputs.aim && aimLoader >= 2.6f && currentAmmo > 0 && !isReloading)
        {
            Ammunition(mouseWPos);
        }
        else
        {
            CurrentReload();
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < currentAmmoD && !isReloading && thirdController.Grounded && totalAmmo > 0)
        {
            Reloading();
        }
    }

    private void Ammunition(Vector3 mp3)
    {
        if (shotLoad == shotLoadD)
        {
            Vector3 aimDir = (mp3 - spawnBulletPos.position).normalized;

            Instantiate(bullet, spawnBulletPos.position, Quaternion.LookRotation(aimDir, Vector3.up));
            Instantiate(bulletFX, spawnBulletPos.position, Quaternion.LookRotation(aimDir, Vector3.up), transform);

            currentAmmo -= 1;
            currentAmmoTxt.text = currentAmmo.ToString();
            totalAmmoTxt.text = totalAmmo.ToString();
        }

        shotLoad -= 1 * Time.deltaTime;

        if(shotLoad <= 0)
        {
            shotLoad = shotLoadD;
        }
    }

    private void CurrentReload()
    {
        if (shotLoad <= 0)
        {
            shotLoad = shotLoadD;
        }
        else if (shotLoad < shotLoadD)
        {
            shotLoad -= 1 * Time.deltaTime;
        }
    }

    private void Reloading()
    {
        isReloading = true;
        StartCoroutine(ReloadingCorrutine());
    }

    private IEnumerator ReloadingCorrutine()
    {
        anim.SetBool("Reloading", true);
        thirdController.enabled = false;
        bananaR.SetActive(true);

        yield return new WaitForSeconds(1.1f);

        anim.SetBool("Reloading", false);
        bananaR.SetActive(false);

        yield return new WaitForSeconds(0.4f);

        thirdController.enabled = true;

        int a = currentAmmo - currentAmmoD;

        int b = a + totalAmmo;

        if (b >= 0)
        {
            currentAmmo = 20;
            totalAmmo += a;

            if (courutine2 == null)
            {
                courutine2 = AmmoChangeValue(a);
                StartCoroutine(courutine2);
            }
            else
            {
                StopCoroutine(courutine2);
                courutine2 = AmmoChangeValue(a);
                StartCoroutine(courutine2);
            }
        }
        else
        {
            if (courutine2 == null)
            {
                courutine2 = AmmoChangeValue(-totalAmmo);
                StartCoroutine(courutine2);
            }
            else
            {
                StopCoroutine(courutine2);
                courutine2 = AmmoChangeValue(-totalAmmo);
                StartCoroutine(courutine2);
            }

            currentAmmo = currentAmmo + totalAmmo;
            totalAmmo = 0;
        }

        currentAmmoTxt.text = currentAmmo.ToString();
        totalAmmoTxt.text = totalAmmo.ToString();

        isReloading = false;
    }

    public void MoreAmmo(int e)
    {
        totalAmmo += e;
        totalAmmoTxt.text = totalAmmo.ToString();

        if (courutine2 == null)
        {
            courutine2 = AmmoChangeValue(e);
            StartCoroutine(courutine2);
        }
        else
        {
            StopCoroutine(courutine2);
            courutine2 = AmmoChangeValue(e);
            StartCoroutine(courutine2);
        }
    }

    public void RecibirDaño(int dmg)
    {
        if (isApe) return;

        vida -= dmg;
        if (courutine1 == null)
        {
            courutine1 = LifeChangeValue(-dmg);
            StartCoroutine(courutine1);
        }
        else
        {
            StopCoroutine(courutine1);
            courutine1 = LifeChangeValue(-dmg);
            StartCoroutine(courutine1);
        }

        UpdateProfile();
        anim.Rebind();
        anim.SetBool("isHit", true);
        isReloading = true;
        StartCoroutine(RecieveDmg());

        ShowLife();
    }

    public void ChangeApe()
    {
        if (!isApe)
        {
            isApe = true;
            profile.sprite = profileImgs[4];
        }
        else
        {
            isApe = false;
            UpdateProfile();
        }
    }

    private void UpdateProfile()
    {
        float f = vida / (float)maxVida;

        if (f >= 0.75)
        {
            profile.sprite = profileImgs[0];
        }
        else if (f >= 0.50)
        {
            profile.sprite = profileImgs[1];
        }
        else if (f >= 0.25)
        {
            profile.sprite = profileImgs[2];
        }
        else
        {
            profile.sprite = profileImgs[3];
        }
    }

    private IEnumerator RecieveDmg()
    {
        yield return new WaitForSeconds(0.4f);
        isReloading = false;
        anim.SetBool("isHit", false);
    }

    private void ShowLife()
    {
        lifeTxt.text = vida + " / " + maxVida;
        lifeSld.fillAmount = (float)vida / (float)maxVida;
    }

    public void Heal(int he)
    {
        vida = vida + he;

        if(courutine1 == null)
        {
            courutine1 = LifeChangeValue(he);
            StartCoroutine(courutine1);
        }
        else
        {
            StopCoroutine(courutine1);
            courutine1 = LifeChangeValue(he);
            StartCoroutine(courutine1);
        }

        if (vida > maxVida) vida = maxVida;

        ShowLife();
        UpdateProfile();
    }

    IEnumerator LifeChangeValue(int e)
    {
        if(e > 0)
        {
            lifeChangeTxt.text = "+" + e;
        }
        else
        {
            lifeChangeTxt.text = e.ToString();
        }

        yield return new WaitForSeconds(2f);

        lifeChangeTxt.text = "";

        courutine1 = null;
    }

    IEnumerator AmmoChangeValue(int e)
    {
        if (e > 0)
        {
            ammoChangeTxt.text = "+" + e;
        }
        else
        {
            ammoChangeTxt.text = e.ToString();
        }

        yield return new WaitForSeconds(2f);

        ammoChangeTxt.text = "";

        courutine2 = null;
    }
}
