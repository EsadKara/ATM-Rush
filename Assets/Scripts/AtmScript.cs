using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AtmScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyTxt;
    public static int moneyValue;
    public int moneyValue1;

    private void Start()
    {
        moneyValue = 0;
        moneyTxt.text = moneyValue.ToString();
    }

    private void Update()
    {
        moneyTxt.text = moneyValue.ToString();
        moneyValue1 = moneyValue;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Money")
        {
            for(int i =1; i < gameManager.instance.collecteds.Count; i++)
            {
                if (other.gameObject == gameManager.instance.collecteds[i])
                {
                    moneyValue += other.gameObject.GetComponent<Money>().value;
                    moneyTxt.text = moneyValue.ToString();
                    gameManager.instance.collecteds.Remove(other.gameObject);
                    Destroy(other.gameObject);
                    gameManager.instance.DeployMoneys(i, this.gameObject);
                }
            }
        }
    }
}
