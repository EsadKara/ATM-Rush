using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle : MonoBehaviour
{
    [SerializeField] GameObject particleEffect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Money")
        {
            for(int i = 0; i < gameManager.instance.collecteds.Count; i++)
            {
                if (gameManager.instance.collecteds[i] == other.gameObject)
                {
                    if(i == gameManager.instance.collecteds.Count - 1)
                    {
                        Instantiate(particleEffect, other.gameObject.transform.position, Quaternion.identity);
                        gameManager.instance.collecteds.Remove(other.gameObject);
                        Destroy(other.gameObject);
                        gameManager.instance.UpdateMoneyValue();
                    }
                    else
                    {
                        gameManager.instance.DeployMoneys(i, this.gameObject);
                        break;
                    }
                }
            }
        }
    }
}
