using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public bool isStunned = false;
    public float stunTime;

    public bool facingRight = false;

    LayerMask playerMask;


    public GameObject childEyes;
    public GameObject childEyesRight;

    public CharacterController2D controller;

    public float moveSpeed;
    public bool attackCooldown;

    public GameObject target1;
    public GameObject target2;
    public bool goingRightPath = true;

    public bool chasingPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(tag);
        //playerMask = LayerMask.GetMask("Player");
        goingRightPath = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        if (!isStunned)
        {

            facingRight = controller.m_FacingRight;

            if(facingRight == true)
            {
                RaycastHit2D check = Physics2D.Raycast(childEyesRight.transform.position, Vector2.right);
                if(check.collider != null)
                {
                    float distance = Mathf.Abs(check.point.x - transform.position.x);
                    //Debug.Log(distance.ToString() + " " + "Hit: " + check.collider.gameObject.tag + " HitName: " + check.collider.gameObject.name);

                    if(check.collider.gameObject.tag == "Player")
                    {
                        controller.Move(((1 * moveSpeed) * Time.fixedDeltaTime), false, false, false);
                        chasingPlayer = true;
                        if (distance <= 5f)
                        {
                            if (!attackCooldown)
                            {
                                StartCoroutine(Attack(check,true));

                            }
                        }
                    }
                    else
                    {
                        if (gameObject.transform.position.x >= target2.transform.position.x)
                        {
                            goingRightPath = !goingRightPath;
                            controller.Move(((-1 * moveSpeed) * Time.fixedDeltaTime), false, false, false);
                            facingRight = !facingRight;
                        }
                        else
                        {
                            
                            //Debug.Log("got to the move func");
                            controller.Move(((1 * moveSpeed) * Time.fixedDeltaTime), false, false, false);
                        }
                    }
                }
                else if (goingRightPath)
                {
                    chasingPlayer = true;
                    
                    if (gameObject.transform.position.x >= target2.transform.position.x)
                    {
                        goingRightPath = !goingRightPath;
                        controller.Move(((-1 * moveSpeed) * Time.fixedDeltaTime), false, false, false);
                        facingRight = !facingRight;
                    }
                    else
                    {
                        
                        controller.Move(((1 * moveSpeed) * Time.fixedDeltaTime), false, false, false);
                    }
                }


            }
            else
            {
                RaycastHit2D check = Physics2D.Raycast(childEyes.transform.position, -Vector2.right);
                if (check.collider != null)
                {
                    float distance = Mathf.Abs(check.point.x - transform.position.x);
                    //Debug.Log(distance.ToString() + " " + "Hit: " + check.collider.gameObject.tag + " HitName: " + check.collider.gameObject.name);
                    if (check.collider.gameObject.tag == "Player")
                    {
                        //Debug.Log("Got to here");
                        controller.Move(((-1 * moveSpeed )* Time.fixedDeltaTime), false, false, false);
                        chasingPlayer = true;
                        if (distance <= 5f)
                        {
                            if (!attackCooldown)
                            {
                                StartCoroutine(Attack(check,true));

                            }
                        }
                    }

                    else
                    {
                        chasingPlayer = false;
                        if (gameObject.transform.position.x <= target1.transform.position.x)
                        {
                            goingRightPath = !goingRightPath;
                            controller.Move(((1 * moveSpeed) * Time.fixedDeltaTime), false, false, false);
                            facingRight = !facingRight;
                        }
                        else
                        {
                            controller.Move(((-1 * moveSpeed) * Time.fixedDeltaTime), false, false, false);
                        }
                    }

                }
                else if (!goingRightPath)
                {
                    
                    if (gameObject.transform.position.x >= target2.transform.position.x)
                    {
                        goingRightPath = !goingRightPath;
                        controller.Move(((1 * moveSpeed) * Time.fixedDeltaTime), false, false, false);
                        facingRight = !facingRight;
                    }
                    else
                    {
                        controller.Move(((-1 * moveSpeed) * Time.fixedDeltaTime), false, false, false);
                    }
                }
            }
        }
    }

    public void Stunned(float newStunTime)
    {
        if(isStunned == true)
        {
            Debug.Log("can't stun multiple times");
        }
        else
        {
            Debug.Log("Enemy Hit");
            isStunned = true;
            stunTime = newStunTime;
            StartCoroutine(StunnedWait());
        }
    }

    IEnumerator Attack(RaycastHit2D input,bool right)
    {
        yield return new WaitForSeconds(1);
        if (right)
        {
            input.collider.gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(-10, 0), ForceMode2D.Impulse);

        }
        else
        {
            input.collider.gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(10, 0), ForceMode2D.Impulse);

        }

        Debug.Log("Success at attack");
        attackCooldown = true;
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(5);
        attackCooldown = false;
    }
    IEnumerator StunnedWait()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("Success at stunn");
        isStunned = false;
    }
}
