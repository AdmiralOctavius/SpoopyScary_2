using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    RaycastHit2D check;
    float distance;
    public Animator animatorZombie;

    public bool ifMothMan = false;

    int layerMask = 1 << 8;

    bool playerSideRight;
    Vector2 heading;
    // Start is called before the first frame update
    void Start()
    {
        controller.Move(((-1 * moveSpeed) * Time.fixedDeltaTime), false, false, false);
        //Debug.Log(tag);
        //playerMask = LayerMask.GetMask("Player");
        goingRightPath = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        /*
        if (!isStunned)
        {
            animatorZombie.SetBool("Walking", true);

            facingRight = controller.m_FacingRight;

            if(facingRight == true)
            {

                CheckForPlayer(true);
                /*
                if(check.collider != null)
                {
                    Debug.Log("Tag of hit: " + check.collider.gameObject.tag);  
                    DistFromPlayer();
                    if (check.collider.gameObject.tag == "Player")
                    {
                        //Debug.Log("Found player here");
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
              

                if (goingRightPath)
                {
                    chasingPlayer = false;
                    
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

                CheckForPlayer(true);
                
                if (check.collider != null)
                {
                    Debug.Log("Tag of hit: " + check.collider.gameObject.tag);
                    DistFromPlayer();
                    if (check.collider.gameObject.tag == "Player")
                    {
                        //Debug.Log("Found player here");
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
        else
        {
            animatorZombie.SetBool("Walking", false);
        }
    **/

        if (!isStunned)
        {
            animatorZombie.SetBool("Walking", true);
            if (chasingPlayer == false)
            {
                facingRight = controller.m_FacingRight;
                if (facingRight)
                {
                    controller.Move(((1 * moveSpeed) * Time.fixedDeltaTime), false, false, false);
                }
                else
                {
                    controller.Move(((-1 * moveSpeed) * Time.fixedDeltaTime), false, false, false);
                }

            }
            else
            {
                if (playerSideRight)
                {
                    controller.Move(((1 * moveSpeed) * Time.fixedDeltaTime), false, false, false);
                }
                else
                {
                    controller.Move(((-1 * moveSpeed) * Time.fixedDeltaTime), false, false, false);
                }
            }
        }
        else
        {
            animatorZombie.SetBool("Walking", false);
        }
    }

    void OnCollisionEnter2D(Collision2D input)
    {
        Debug.Log(input.gameObject.name);
        if(input.gameObject.tag == "Player")
        {
            Debug.Log("GotHere");
            heading = transform.position - input.transform.position;
            var distance = heading.magnitude;
            var direction = heading / distance; // This is now the normalized direction.
            if (direction.x >= 0)
            {
                playerSideRight = true;
            }
            else
            {
                playerSideRight = false;
            }

            if(distance <= 3)
            {
                if (!attackCooldown)
                {
                    StartCoroutine(Attack(input, true));

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

    public void CheckForPlayer(bool isRight)
    {
        if (isRight)
        {
            check = Physics2D.Raycast(childEyesRight.transform.position, Vector2.right, 50f, layerMask);
            Debug.DrawRay(childEyesRight.transform.position, -Vector2.right, Color.green, 5f, false);
        }
        else
        {
            check = Physics2D.Raycast(childEyesRight.transform.position, -Vector2.right, 50f, layerMask);
            Debug.DrawRay(childEyesRight.transform.position, Vector2.right, Color.green, 5f, false);
        }
    }

    public void DistFromPlayer()
    {
       
        distance = Mathf.Abs(check.point.x - transform.position.x);
        //Debug.Log(distance.ToString() + " " + "Hit: " + check.collider.gameObject.tag + " HitName: " + check.collider.gameObject.name);

    }

    IEnumerator Attack(Collision2D input,bool right)
    {
        
        animatorZombie.SetBool("Attack", true);
        yield return new WaitForSeconds(1);
        GetComponent<AudioSource>().Play();
        if (right)
        {
            input.collider.gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(10, 0), ForceMode2D.Impulse);

        }
        else
        {
            input.collider.gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(-10, 0), ForceMode2D.Impulse);

        }

        Debug.Log("Success at attack");
        attackCooldown = true;
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        animatorZombie.SetBool("Attack", false);
        yield return new WaitForSeconds(5);
        attackCooldown = false;
        if (ifMothMan)
        {
            SceneManager.LoadScene(0);
        }
    }
    IEnumerator StunnedWait()
    {

        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        gameObject.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(5);
        Debug.Log("Success at stunn");
        isStunned = false;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        gameObject.GetComponent<Collider2D>().enabled = true;
        goingRightPath = true;
    }
}
