using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointScript : MonoBehaviour
{

    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D info)
    {
        if (info.gameObject.CompareTag ("Player"))
        {
            PlayerBallControl player = info.GetComponent <PlayerBallControl>();
            player.checkpointLocation = transform.position;
            Debug.Log("Check     POINT");
        }
        gm.SendMessage("CheckPointReached");
    }
}
