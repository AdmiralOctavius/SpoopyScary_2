using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public GameObject cameraFlash;
    public GameObject interactColliderBox;
    
    float runSpeed = 40f;
    float horizontalMove = 0f;
    bool isJumping = false;
    public bool isCrouching = false;
    bool isRunning = false;
     
    Collider2D[] results = new Collider2D[5];
    ContactFilter2D baseFilter;

    public float stunPower = 5f;


    public int playerCameraAmmo = 12;
    // Start is called before the first frame update
    void Start()
    {
        baseFilter.NoFilter();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //controller.Move();

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
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
            cameraFlash.SetActive(true);
            cameraFlash.GetComponent<Collider2D>().OverlapCollider(baseFilter, results);

            for(int i = 0; i < results.Length; i++)
            {
                if(results.GetValue(i) != null)
                {
                    if(results[i].tag == "Enemy")
                    {
                        //Debug.Log(results[i].name);
                        results[i].gameObject.GetComponent<EnemyScript>().Stunned(stunPower);
                    }

                }
            }
            cameraFlash.SetActive(false);

        }

        if (Input.GetButtonDown("Interact"))
        {
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
        
    }
}
