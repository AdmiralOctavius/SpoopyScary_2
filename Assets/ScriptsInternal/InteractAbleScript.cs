using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractAbleScript : MonoBehaviour
{

    public Collider2D doorCollider;

    public bool isTextObj;
    int textPosition;
    public string[] TextBlockText;
    public GameObject TextUIObj;
    public GameObject TextHolderUIOBJ;

    public bool isAmmo;
    public int ammoCount;

    public bool isCorpse;
    public string CorpsePostExamine;
    public bool finishedReadingCorpse;
   
    public bool isDoor;
    public bool requiresKey;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (isCorpse)
            {
                TextHolderUIOBJ.SetActive(false);
            }
            else
            {
                textPosition = 0;
                TextUIObj.GetComponent<Text>().text = "";
                TextHolderUIOBJ.SetActive(false);
                //TextUIObj.SetActive(false);

            }
        }
    }

    public void InteractObjectActivate()
    {
        Debug.Log("Got here in corpse");
        if (isTextObj){
            if(textPosition < TextBlockText.Length)
            {
                TextHolderUIOBJ.SetActive(true);
                //TextUIObj.SetActive(true);
                TextUIObj.GetComponent<Text>().text = TextBlockText[textPosition];
                textPosition++;
            }

        }

        if (isAmmo)
        {
            if (!isCorpse)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().playerCameraAmmo += ammoCount;
                isAmmo = false;

            }
        }
        
        if ((isCorpse == true))
        {
            Debug.Log("Got here in if call");
            if (textPosition < TextBlockText.Length)
            {
                Debug.Log("Error");
                TextHolderUIOBJ.SetActive(true);
                //TextUIObj.SetActive(true);
                TextUIObj.GetComponent<Text>().text = TextBlockText[textPosition];
                textPosition++;
            }
            else
            {
                if (isAmmo)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().playerCameraAmmo += ammoCount;
                    isAmmo = false;

                }
                finishedReadingCorpse = true;
                isCorpse = false;
            }
        }

        if (finishedReadingCorpse)
        {
            if(TextHolderUIOBJ.activeSelf == true)
            {
                TextHolderUIOBJ.SetActive(false);
            }
            else
            {
                TextHolderUIOBJ.SetActive(true);
                TextUIObj.GetComponent<Text>().text = CorpsePostExamine;

            }
        }

        if (isDoor)
        {
            if(doorCollider.isTrigger == true)
            {
                doorCollider.isTrigger = false;
            }
            else
            {
                doorCollider.isTrigger = true;
            }
        }
    }
}
