using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{

    AudioSource soundSource;
    public Camera mainCam;

    void OnEnable()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector3 somethingPosition = rectTransform.position;
        rectTransform.localPosition = new Vector2(0, 200);
        rb.velocity = new Vector2(0,0);
    }

    void Start()
    {
        //Debug.Log(transform.position);
        soundSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "NormalBlock")
        {
            soundSource.Play();
        }
    }

}
