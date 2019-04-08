using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.Audio;

public class PlayerBallControl : MonoBehaviour
{
    public Rigidbody2D playerRB;
    public Camera mainCam;
    public AudioSource source;
    public AudioSource fastDroppingSource;
    public AudioSource deathSound;
    public GameManager gm;
    public Vector2 checkpointLocation;

    public float moveSpeed = 10f;
    public float bounceForce = 10f;
    public float bouncePower = 10f;
    public bool fastDropping = false;
    public bool controllable = true;

    void Start()
    {
        Vector3 pos1 = mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.9f, 10.0f));
        //Debug.Log(pos1);
        transform.position = pos1;
    }

    void Update()
    {
        //Vector3 pos = mainCam.WorldToViewportPoint(transform.position);
        Vector3 pos2 = mainCam.ScreenToWorldPoint(mainCam.transform.position);

        if (Input.GetKeyDown(KeyCode.R))
        {
            playerRB.velocity = new Vector2(0,0);
            transform.position = new Vector2(0, 4);
            fastDropping = false;
            Time.timeScale = 1f;
        }

        if (transform.position.x < pos2.x)
        {
            transform.position = new Vector2 (-pos2.x, transform.position.y);
        }else if (transform.position.x > -pos2.x)
        {
            transform.position = new Vector2(pos2.x, transform.position.y);
        }
    }

    void FixedUpdate()
    {
        //Maybe take away control when speeding down

        //float move = Input.GetAxis("Horizontal");

        float move = Input.acceleration.x;

        if (Input.GetKeyDown(KeyCode.Space) && controllable)
        {
            playerRB.velocity = new Vector2(0, -bounceForce);
            fastDropping = true;
        }

        if (Input.touchCount > 0)
        {
            Touch screenTouch = Input.GetTouch(0);

            if (screenTouch.phase == TouchPhase.Began)
            {
                playerRB.velocity = new Vector2(0, -bounceForce);
                fastDropping = true;
            }
        }
        //if()
        if (move != 0 && controllable)
        {
            playerRB.velocity = new Vector2(move * moveSpeed, playerRB.velocity.y);
        }
        else
        {
            playerRB.velocity = new Vector2(0, playerRB.velocity.y);
        }

        if (playerRB.velocity.y > 7.7057)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, 7.7057f);
        }
    }

    void OnCollisionStay2D(Collision2D collInfo)
    {
        GameObject blockHit;

        blockHit = collInfo.gameObject;

        if (blockHit.tag == "NormalBlock")
        {
            playerRB.velocity = new Vector2(0, 0);
            playerRB.AddForce(new Vector2(0, bouncePower));
            fastDropping = false;
        }
        else if (blockHit.tag == "MultiHitBlock")
        {
            playerRB.velocity = new Vector2(0, 0);
            playerRB.AddForce(new Vector2(0, bouncePower));
            if (fastDropping)
            {
                blockHit.SendMessage("RegisteredHit");
            }
            fastDropping = false;
        }
        else if (blockHit.tag == "DamageBlock")
        {
            deathSound.Play();
            playerRB.velocity = new Vector2(0, 0);
            playerRB.AddForce(new Vector2(0, bouncePower));
            controllable = false;
            fastDropping = false;
            gm.SendMessage("PlayerDied");
            mainCam.SendMessage("DeathZoom");
            //Add death mechanic
        }
    }

    void OnCollisionEnter2D(Collision2D collInfo)
    {
        if ((collInfo.collider.tag == "MultiHitBlock" && !fastDropping) || collInfo.collider.tag == "NormalBlock")
        {
            source.Play();
        }
        else if (collInfo.collider.tag == "MultiHitBlock" && fastDropping)
        {
            fastDroppingSource.Play();
        }
    }

    void CheckPointRestart()
    {
        mainCam.SendMessage("DeathZoom");
        playerRB.velocity = new Vector2(0,0);
        transform.position = checkpointLocation;
    }
}
