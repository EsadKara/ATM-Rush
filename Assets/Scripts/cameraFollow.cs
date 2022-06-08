using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class cameraFollow : MonoBehaviour
{
    public Vector3 offset;
    Quaternion newRot;
    [SerializeField] GameObject player;
    bool isFollow;

    private void Start()
    {
        newRot = Quaternion.Euler(14, 0, 0);
        isFollow = false;
    }

    private void Update()
    {
        if (gameManager.instance.isStart)
            isFollow = true;
    }

    void LateUpdate()
    {
        if (isFollow)
        {
            transform.rotation = newRot;
            transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, 5f);
        }
    }

    public void UpdateCam()
    {
        Quaternion newRot = Quaternion.Euler(0, 0, 0);
        transform.rotation = newRot;
        offset.y = 0.5f;
        offset.z = -2.8f;
    }
}
