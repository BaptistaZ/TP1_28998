using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    private float currentTime;

    private bool smash, invincible;
   
    private int currentBrokenStacks, totalStacks;

    public GameObject invincibleObj;
    public Image invincibleFill;
    public GameObject fireEffect, winEffect, splashEffect;

    public enum BallState
    {
        Prepare,
        Playing,
        Died,
        Finish
    }

    [HideInInspector]
    public BallState ballState = BallState.Prepare;

    public AudioClip bounceOffClip, deadClip, winClip, destroyClip, iDestroyClip;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentBrokenStacks = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
         totalStacks = FindObjectsOfType<StackController>().Length;
         
    }


    // Update is called once per frame
    void Update()
{
    if(ballState == BallState.Playing)
    {
        if (Input.GetMouseButtonDown(0))
            smash = true;

        if (Input.GetMouseButtonUp(0))
            smash = false;

        if (invincible)
        {
            currentTime -= Time.deltaTime * .35f;

            // Apenas ativa o efeito de fogo quando invencível.
            if (!fireEffect.activeInHierarchy)
                fireEffect.SetActive(true);

            if (currentTime <= 0)
            {
                currentTime = 0;
                invincible = false;
                // Desativa o efeito de fogo quando deixa de ser invencível.
                fireEffect.SetActive(false);
                invincibleFill.color = Color.white;
            }
        }
        else
        {
            // Desativa o efeito de fogo quando não está invencível.
            if (fireEffect.activeInHierarchy)
                fireEffect.SetActive(false);

            if (smash)
                currentTime += Time.deltaTime * .8f;
            else
                currentTime -= Time.deltaTime * .5f;
        }

        // Ativa o objeto de invencibilidade com base na quantidade de tempo.
        invincibleObj.SetActive(currentTime >= 0.3f);

        if (currentTime >= 1)
        {
            currentTime = 1;
            invincible = true;
            invincibleFill.color = Color.red;
        }

        // Atualiza a barra de invencibilidade.
        invincibleFill.fillAmount = currentTime;
    }

    if(ballState == BallState.Finish)
    {
        if (Input.GetMouseButtonDown(0))
            FindObjectOfType<GeradorNiveis>().NextLevel();
    }
}

    void FixedUpdate()
    {
       if (ballState == BallState.Playing)
       {
            if (Input.GetMouseButton(0))
            {
                smash = true;
                rb.velocity = new Vector3(0, -100 * Time.fixedDeltaTime * 7, 0);
            }
       }

        if (rb.velocity.y > 5)
            rb.velocity = new Vector3(rb.velocity.x, 5, rb.velocity.z);
    }

    public void IncreaseBrockenStacks()
    {
        currentBrokenStacks++;

        if (!invincible)
        {
            GerirPontuacao.instance.AddScore(1);
            GerirSom.instance.PlaySoundFX(iDestroyClip, 0.5f);
        }
        else
        {
            GerirPontuacao.instance.AddScore(2);
            GerirSom.instance.PlaySoundFX(iDestroyClip, 0.5f);
        }
    }

    void OnCollisionEnter(Collision target)
    {
        if (!smash)
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);

            if (target.gameObject.tag != "Finish")
            {
                GameObject splash = Instantiate(splashEffect);
                splash.transform.SetParent(target.transform);
                splash.transform.localEulerAngles = new Vector3(90, Random.Range(0, 359), 0);
                float randomScale = Random.Range(0.18f, 0.25f);
                splash.transform.localScale = new Vector3(randomScale, randomScale, 1);
                splash.transform.position = new Vector3(transform.position.x, transform.position.y - 0.22f, transform.position.z);
                splash.GetComponent<SpriteRenderer>().color = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
            }

            GerirSom.instance.PlaySoundFX(bounceOffClip, 0.5f);
        }
        else
        {
            if (invincible)
            {
                if (target.gameObject.tag == "enemy" || target.gameObject.tag == "plane")
                {
                    target.transform.parent.GetComponent<StackController>().ShatterAllParts();
                }
            }

            else
            {
                if (target.gameObject.tag == "enemy")
                {
                    target.transform.parent.GetComponent<StackController>().ShatterAllParts();
                }

               if (target.gameObject.tag == "plane")
                {
                    rb.isKinematic = true;
                    transform.GetChild(0).gameObject.SetActive(false);
                    ballState = BallState.Died;
                    GerirSom.instance.PlaySoundFX(deadClip, 0.5f);
                }
            }
        }

         foreach (GameUI gui in FindObjectsOfType<GameUI>())
         {
              gui.LevelSliderFill(currentBrokenStacks / (float)totalStacks);
         }

        if (target.gameObject.tag == "Finish" && ballState == BallState.Playing)
        {
            ballState = BallState.Finish;
            GerirSom.instance.PlaySoundFX(winClip, 0.7f);
            GameObject win = Instantiate(winEffect);
            win.transform.SetParent(Camera.main.transform);
            win.transform.localPosition = Vector3.up * 1.5f;
            win.transform.eulerAngles = Vector3.zero;
        }
    }

    void OnCollisionStay(Collision target)
    {
        if (!smash || target.gameObject.tag == "Finish")
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
        }
    }
}

