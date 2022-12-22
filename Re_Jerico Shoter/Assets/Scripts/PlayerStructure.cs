using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using TMPro;
using UnityEngine.UI;

public class PlayerStructure : MonoBehaviour
{
    public GameObject jerico;
    public GameObject gorilla;
    public GameObject aimCam;

    private Animator anim;
    public RuntimeAnimatorController jericoAnim;
    public RuntimeAnimatorController gorilaAnim;

    public Avatar jericoAvatar;
    public Avatar gorillaAvatar;
    public GameObject vfxGorila;
    public GameObject vfxJerico;

    private bool isTransform;
    private bool isAB;

    private ThirdPersonController thirdController;
    private ThirdPersonShoterController controller;

    [Header ("Rage")]
    [SerializeField] private TextMeshProUGUI rageTxt;
    [SerializeField] private Image rageSld;
    [SerializeField] private int rageValueMax;
    private int rageValue;
    private float rageRef;

    void Start()
    {
        anim = GetComponent<Animator>();
        thirdController = GetComponent<ThirdPersonController>();
        controller = GetComponent<ThirdPersonShoterController>();

        UpdateRage();
    }

    void Update()
    {
        ChangeBody();
        Punch();
    }

    private void ChangeBody()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Vector3 newPos = transform.position;
            newPos.y += 0.5f;

            if (!isTransform && rageValue == 50)
            {
                Instantiate(vfxGorila, newPos, Quaternion.Euler(90, 0, 0));

                jerico.SetActive(false);
                anim.runtimeAnimatorController = gorilaAnim;
                anim.avatar = gorillaAvatar;
                aimCam.SetActive(false);

                thirdController.SetSensitivity(1);

                thirdController.SetRotateOnMove(true);

                anim.SetLayerWeight(1, Mathf.Lerp(anim.GetLayerWeight(1), 0f, Time.deltaTime * 10f));

                GetComponent<ThirdPersonShoterController>().enabled = false;
                gorilla.SetActive(true);

                isTransform = true;
                controller.ChangeApe();
                rageRef = rageValueMax;
            }
            else if (isTransform)
            {
                gorilla.SetActive(false);
                anim.runtimeAnimatorController = jericoAnim;
                anim.avatar = jericoAvatar;
                GetComponent<ThirdPersonShoterController>().enabled = true;
                jerico.SetActive(true);

                Instantiate(vfxJerico, newPos, Quaternion.Euler(90, 0, 0));

                isTransform = false;

                controller.ChangeApe();
                rageRef = 0;
                rageValue = 0;
            }

            UpdateRage();
        }

        if (isTransform)
        {
            rageRef -= 2 * Time.deltaTime;
            rageValue = (int)rageRef;

            if(rageValue == 0)
            {
                Vector3 newPos = transform.position;
                newPos.y += 0.5f;

                gorilla.SetActive(false);
                anim.runtimeAnimatorController = jericoAnim;
                anim.avatar = jericoAvatar;
                GetComponent<ThirdPersonShoterController>().enabled = true;
                jerico.SetActive(true);

                Instantiate(vfxJerico, newPos, Quaternion.Euler(90, 0, 0));

                isTransform = false;
            }

            UpdateRage();
        }
    }

    private void Punch()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && isTransform && !isAB && thirdController.Grounded)
        {
            isAB = true;

            anim.SetInteger("Ability", 1);

            thirdController.enabled = false;

            StartCoroutine(StartPunch());
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && isTransform && !isAB && thirdController.Grounded)
        {
            isAB = true;

            anim.SetInteger("Ability", 2);

            thirdController.enabled = false;

            StartCoroutine(StartAB01());
        }
    }

    IEnumerator StartPunch()
    {
        yield return new WaitForSeconds(0.3f);
        anim.SetInteger("Ability", 0);

        yield return new WaitForSeconds(0.3f);

        isAB = false;
        thirdController.enabled = true;
    }

    IEnumerator StartAB01()
    {
        yield return new WaitForSeconds(0.3f);
        anim.SetInteger("Ability", 0);

        yield return new WaitForSeconds(3.5f);

        isAB = false;
        thirdController.enabled = true;
    }

    private void UpdateRage()
    {
        rageTxt.text = rageValue + " / " + rageValueMax;

        rageSld.fillAmount = rageValue / (float)rageValueMax;
    }

    public void IncreaseRage(int e)
    {
        if (isTransform) return;

        rageValue += e;

        if (rageValue > rageValueMax) rageValue = rageValueMax;

        UpdateRage();
    }
}
