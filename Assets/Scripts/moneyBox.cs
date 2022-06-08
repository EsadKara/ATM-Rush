using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class moneyBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyTxt;
    [SerializeField] bankScript bank;

    public int moneyValue;
    
    private void Start()
    {
        moneyValue = PlayerPrefs.GetInt("moneyValue");
        moneyTxt.text = moneyValue + " / 100";
    }

    private void Update()
    {
        moneyTxt.text = moneyValue + " / " + bank.capacity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Money")
        {
            Destroy(other.gameObject);
            IncreaseMoneyValue();
        }
    }
    void IncreaseMoneyValue()
    {
        moneyValue++;
        PlayerPrefs.SetInt("moneyValue", moneyValue);
    }

    public void CollectMoneys()
    {
        gameManager.instance.totalMoney += moneyValue;
        moneyValue = 0;
        PlayerPrefs.SetInt("moneyValue", moneyValue);
        gameManager.instance.totalMoneyTxt.text = System.String.Format("{0:0}", gameManager.instance.totalMoney);
        PlayerPrefs.SetFloat("TotalMoney", gameManager.instance.totalMoney);
    }
}
