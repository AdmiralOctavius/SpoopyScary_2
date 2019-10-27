using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public GameObject cameraFlash;
    public GameObject interactColliderBox;
    public GameObject lightCamera;
    public Animator animatorPlayer;

    public bool lightFade = true;
    public float currentLightIntensity;
    float runSpeed = 40f;
    float horizontalMove = 0f;
    bool isJumping = false;
    public bool isCrouching = false;
    bool isRunning = false;
    bool isTalking = false;
    Collider2D[] results = new Collider2D[5];
    ContactFilter2D baseFilter;

    public float stunPower = 5f;
    public bool flashed;

    public int playerCameraAmmo = 12;

    public bool CameraUsed = false;
    // Start is called before the first frame update
    void Start()
    {
        baseFilter.NoFilter();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //controller.Move();
        if (!CameraUsed && !isTalking)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            if(horizontalMove != 0)
            {
                animatorPlayer.SetBool("Walking", true);
            }
            else
            {
                animatorPlayer.SetBool("Walking", false);
            }

        }

        if (Input.GetButtonDown("Jump"))
        {
            //isJumping = true;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            isCrouching = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            isCrouching = false;
        }

        if (Input.GetButtonDown("CameraClick"))
        {
            if (!CameraUsed) {
                CameraUsed = true;
                animatorPlayer.SetBool("Shooting", true);
                cameraFlash.SetActive(true);
                cameraFlash.GetComponent<Collider2D>().OverlapCollider(baseFilter, results);
                //lightCamera.SetActive(true);
                //animatorPlayer.GetBool("Flashed");
                for(int i = 0; i < results.Length; i++)
                {
                    if(results.GetValue(i) != null)
                    {
                        if(results[i].tag == "Enemy")
                        {
                            Debug.Log(results[i].name);
                            results[i].gameObject.GetComponent<EnemyScript>().Stunned(stunPower);

                        }

                    }
                }
                cameraFlash.SetActive(false);
                //StartCoroutine(CameraFlashWait());
                lightFade = true;
            }
        }

        if (Input.GetButtonDown("Interact"))
        {
            isTalking = true;
            interactColliderBox.SetActive(true);
            interactColliderBox.GetComponent<Collider2D>().OverlapCollider(baseFilter, results);

            for (int i = 0; i < results.Length; i++)
            {
                if (results.GetValue(i) != null)
                {
                    if (results[i].tag == "Interactable")
                    {
                        Debug.Log(results[i].name);
                        //results[i].gameObject.GetComponent<EnemyScript>().Stunned(stunPower);
                        results[i].gameObject.GetComponent<InteractAbleScript>().InteractObjectActivate();
                        
                    }

                }
            }
            StartCoroutine(TalkingWait());
            interactColliderBox.SetActive(false);
        }


        if (Input.GetButton("Running"))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, isCrouching, isJumping, isRunning);
        isJumping = false;

        if (flashed)
        {
            lightCamera.SetActive(true);
        }

        if(lightFade == true)
        {
            //Debug.Log("Got to light fade function");
            if (lightCamera.GetComponent<UnityEngine.Experimental.Rendering.LWRP.Light2D>().intensity >= 0)
            {
                Debug.Log(lightCamera.GetComponent<UnityEngine.Experimental.Rendering.LWRP.Light2D>().intensity);
                //Debug.Log("Got here in fixed update");
                lightCamera.GetComponent<UnityEngine.Experimental.Rendering.LWRP.Light2D>().intensity -= 2.5f * Time.deltaTime;
                currentLightIntensity = lightCamera.GetComponent<UnityEngine.Experimental.Rendering.LWRP.Light2D>().intensity;
            }
            else
            {
                lightCamera.SetActive(false);
                lightCamera.GetComponent<UnityEngine.Experimental.Rendering.LWRP.Light2D>().intensity = 5;
                currentLightIntensity = lightCamera.GetComponent<UnityEngine.Experimental.Rendering.LWRP.Light2D>().intensity;
                lightFade = false;
                flashed = false;
            }

        }
    }

    IEnumerator CameraFlashWait()
    {
        while (true)
        {
            while(lightCamera.GetComponent<UnityEngine.Experimental.Rendering.LWRP.Light2D>().intensity >= 0)
            {
                Debug.Log("Got here in coroutine");
                lightCamera.GetComponent<UnityEngine.Experimental.Rendering.LWRP.Light2D>().intensity -= 0.000000001f * Time.deltaTime;
                currentLightIntensity = lightCamera.GetComponent<UnityEngine.Experimental.Rendering.LWRP.Light2D>().intensity;
            }
            yield return null;
            lightCamera.SetActive(false);
        }
        //yield return new WaitForSeconds(1);
        
    }

    IEnumerator TalkingWait()
    {
        yield return new WaitForSeconds(1);
        isTalking = false;
        animatorPlayer.SetBool("Talking", false);
    }

    public void CameraHasFlashed()
    {
        GetComponent<AudioSource>().Play();
        Debug.Log("Got flashed again");
        flashed = true;
        animatorPlayer.SetBool("Shooting", false);
        CameraUsed = false;
        lightFade = true;
    }
}
