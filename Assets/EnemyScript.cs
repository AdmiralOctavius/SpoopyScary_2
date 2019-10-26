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

    public CharacterController2D controller;

    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(tag);
//playerMask = LayerMask.GetMask("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(facingRight == true)
        {
            RaycastHit2D check = Physics2D.Raycast(childEyes.transform.position, Vector2.right);
            if(check.collider != null)
            {
                float distance = Mathf.Abs(check.point.x - transform.position.x);
                Debug.Log(distance.ToString() + " " + "Hit: " + check.collider.gameObject.tag + " HitName: " + check.collider.gameObject.name);

                if(check.collider.gameObject.tag == "Player")
                {
                    controller.Move((1 * moveSpeed * Time.fixedDeltaTime), false, false, false);
                }
            }

        }
        else
        {
            RaycastHit2D check = Physics2D.Raycast(childEyes.transform.position, -Vector2.right);
            if (check.collider != null)
            {
                float distance = Mathf.Abs(check.point.x - transform.position.x);
                Debug.Log(distance.ToString() + " " + "Hit: " + check.collider.gameObject.tag + " HitName: " + check.collider.gameObject.name);
                if (check.collider.gameObject.tag == "Player")
                {
                    Debug.Log("Got to here");
                    controller.Move((-1 * moveSpeed * Time.fixedDeltaTime), false, false, false);
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

    IEnumerator StunnedWait()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("Success at stunn");
    }
}
