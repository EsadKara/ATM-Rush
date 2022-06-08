using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CollectibleMoney")
        {
            if (!gameManager.instance.collecteds.Contains(other.gameObject))
            {
                GameObject money = other.gameObject;
                gameManager.instance.StackMoney(money, gameManager.instance.collecteds.Count - 1);
                money.tag = "Money";
                money.AddComponent<collect>();
                money.AddComponent<Rigidbody>();
                money.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
}
