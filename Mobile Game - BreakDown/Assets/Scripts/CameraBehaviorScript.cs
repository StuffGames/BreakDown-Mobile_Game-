using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviorScript : MonoBehaviour
{
    public Transform player;
    private Camera mainCam;

    public float smoothTime;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;
    private bool playerIsDead = false;

    private float spinDirection;

    void Start()
    {
        mainCam = GetComponent<Camera>();
        //Debug.Log(mainCam.WorldToViewportPoint(new Vector3 (0, -0.5f,0)));

        int random = Random.Range(0, 2);
        //Debug.Log(random);
        if (random == 1)
        {
            spinDirection = 0.25f;
        }
        else if (random == 0)
        {
            spinDirection = -0.25f;
        }
    }

    void LateUpdate()
    {
        //Vector3 viewPortPosition = mainCam.WorldToViewportPoint(player.position);

        Vector3 destination;

        if (playerIsDead)
        {
            destination = new Vector3(player.position.x, player.position.y, player.position.z - 1);
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, 3, 0.125f);

            transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0.0f, 0.0f, spinDirection, 1.0f), 0.125f);// * Time.deltaTime);
        }
        else
        {
            destination = new Vector3(0, player.position.y + offset.y, player.position.z + offset.z);
            mainCam.orthographicSize = 5;
            transform.rotation = new Quaternion(0, 0, 0, 1);
        }

        //if (viewPortPosition.y < 0.6f)
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, smoothTime);
    }

    public void DeathZoom()
    {
        //Debug.Log("Check!");
        if (!playerIsDead)
        {
            playerIsDead = true;
        }else if (playerIsDead)
        {
            playerIsDead = false;
        }
    }
}
