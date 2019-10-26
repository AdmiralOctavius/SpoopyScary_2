using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;

    float runSpeed = 40f;
    float horizontalMove = 0f;
    bool isJumping = false;
    public bool isCrouching = false;

    public GameObject cameraFlash;
     
    Collider2D[] results = new Collider2D[5];
    ContactFilter2D baseFilter;
    // Start is called before the first frame update
    void Start()
    {
        baseFilter.NoFilter();
        baseFilter.
        
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
            cameraFlash.GetComponent<Collider2D>().OverlapCollider(baseFilter, results);

            for(int i = 0; i < results.Length; i++)
            {
                if(results.GetValue(i) != null)
                {
                    if(results[i].tag == "Enemy")
                    {
                        //Debug.Log(results[i].name);
                        results[i].GetComponent<EnemyScript>().Stunned();
                    }

                }
            }
        }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, isCrouching, isJumping);
        isJumping = false;
        
    }
}
