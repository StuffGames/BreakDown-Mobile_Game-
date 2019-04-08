using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlatformBehaviourScript : MonoBehaviour
{
    public List<Transform> platformList = new List<Transform>();

    public Camera mainCam;
    //public AudioSource source;
    //public AudioClip fastDroppingSound;

    //public int numberOfPlatforms;
    public int platformIndex;
    public int hits;
    private Vector3 platPos2;
    LineRenderer thisLine;
    Color colorShift;


    void Start()
    {
        Transform parent = transform.parent;

        foreach (Transform child in parent)
        {
            if (transform.tag != "PlatformRow")
            {
                platformList.Add(child);
            }
        }
        
        //Vector3 pos1 = mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.6f, 10.0f));
        //transform.position = pos1;
        thisLine = GetComponent<LineRenderer>();
        colorShift.a = 1;
        colorShift.b = 0;
        colorShift.g = 1;
        if (gameObject.tag == "MultiHitBlock")
        {
            colorShift.r = (hits / 255f) / 255f;
        }
        else if (gameObject.tag == "DamageBlock")
        {
            colorShift.r = 1;
            colorShift.b = 0;
            colorShift.g = 0;
            thisLine.material.color = colorShift;
        }
        else if (gameObject.tag == "NormalBlock")
        {
            colorShift.b = 1;
            colorShift.r = 1;
            thisLine.material.color = colorShift;
        }

        int index = 0;
        foreach (Transform platform in platformList)
        {
            index++;
            PlatformBehaviourScript script = platform.GetComponent<PlatformBehaviourScript>();
            script.platformIndex = index;
            //Debug.Log(platformList.IndexOf(platform));
        }
    }

    void Update()
    {
        //BIG BIG THING TO FIX. the bug where the crazy high bounce occurs, happens from the intersecting colliders of blocks - FIX IT

        Transform platformInFront;

        if (platformIndex == platformList.Count)
        {
            platformInFront = platformList[0];
        }
        else
        {
            platformInFront = platformList[platformIndex];
        }

        //platPos2 = mainCam.WorldToViewportPoint(transform.position);
        Vector2 Platpos = transform.position;
        //float Xmax = 1.5f * numberOfPlatforms; //1.5f is the norm if you want to go 

        /*if (platPos2.y > 1.25f)//if its too short use something between 1.25 and 1.5 i guess
        {
            gameObject.SetActive(false);
        }*/

        if (Platpos.x >= 4.49f)
        {
            /*float spacing;
            if (platformIndex == platformList.Count)
            {
                spacing = 2.75f;
            }
            else
            {
                spacing = 3f;
            }*/

            transform.position = new Vector2(-31.25f, Platpos.y); //-Xmax, Platpos.y); //idk fix this or something -28.5
        }
        else
        {
            transform.position = new Vector2(Platpos.x + 1f / 4f, Platpos.y); //(1f / (numberOfPlatforms * 1.5f)), Platpos.y);
            //the speed could also just be a set speed for any number of platforms: Platpos.x + (1f/6)
        }

        if (gameObject.tag == "MultiHitBlock")
        {
            if (hits > 0)
            {
                colorShift.r = (255f / hits) / 255f;
            }
            thisLine.material.color = colorShift;
        }
    }

    public void RegisteredHit()
    {
        if (gameObject.tag == "MultiHitBlock")
        {
            //source.Play();
            --hits;
            if (hits < 1)
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                thisLine.enabled = false;
                //GameObject.Destroy(this.gameObject);
            }
        }

    }

    /*void OnCollisionEnter2D(Collision2D colInfo)
    {
        if (colInfo.collider.tag == "Player" && gameObject.tag == "NormalBlock")
        {
            source.Play();
        }
    }*/
}
