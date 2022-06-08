using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Conveyor : MonoBehaviour
{
    [SerializeField] GameObject point;
    GameObject GameManagerPref;
    void Start()
    {
        GameManagerPref = GameObject.Find("GameManager");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Money")
        {
            gameManager.instance.collecteds.Remove(other.gameObject);
            other.gameObject.transform.DOMove(point.transform.position, 1f);
        }
        else if (other.gameObject.tag == "Player")
        {
            GameManagerPref.GetComponent<Movement>().enabled=false;
            gameManager.instance.isFinish = true;
        }
    }
}
