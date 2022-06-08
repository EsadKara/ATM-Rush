using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float runSpeed, swipeSpeed,xValue;

    public float inputHorizontal, touchPos;
    public bool isMove;

    gameManager gM;
    GameObject player;

    Vector2 movePos;

    void Start()
    {
        gM = GetComponent<gameManager>();
        player = gM.collecteds[0];
    }

    private void Update()
    {
        inputHorizontal = Input.GetAxis("Horizontal");
    }
    void FixedUpdate()
    { 
        if (gameManager.instance.isStart)
        {
            transform.position += Vector3.forward * runSpeed * Time.deltaTime;
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    movePos = touch.deltaPosition;
                    isMove = true;
                }
                if (movePos.x > 1) 
                {
                    player.transform.Translate(swipeSpeed * Time.deltaTime, 0, 0);
                }
                else if (movePos.x < -1)
                {
                    player.transform.Translate(-swipeSpeed * Time.deltaTime, 0, 0);
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    isMove = false;
                }
            }
            player.transform.Translate(inputHorizontal * swipeSpeed * Time.deltaTime, 0, 0);
            if (player.transform.position.x <= -xValue)
            {
                player.transform.position = new Vector3(-xValue, player.transform.position.y, player.transform.position.z);
            }
            else if (player.transform.position.x >= xValue)
            {
                player.transform.position = new Vector3(xValue, player.transform.position.y, player.transform.position.z);
            }
        }
    }
}
