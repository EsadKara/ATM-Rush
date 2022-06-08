using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class bankScript : MonoBehaviour
{
    public int capacity;
    public float speed;

    int capacityValue, capacityLv, speedValue, speedLv;

    [SerializeField] TextMeshProUGUI capacityLvTxt, capacityValueTxt, speedTxt, speedLvTxt, speedValueTxt;
    [SerializeField] GameObject bankMoney, spawnPoint, bank, capacityBtnObj, speedBtnObj;
    [SerializeField] Button capacityBtn, speedBtn;
    [SerializeField] moneyBox MoneyBox;
    void Start()
    {
        StartCoroutine(SpawnMoney());
        LoadPlayerPrefs();

        capacityBtnObj.SetActive(true);
        speedBtnObj.SetActive(true);

        capacityLvTxt.text = "Lv " + capacityLv;
        speedLvTxt.text = "Lv " + speedLv;
        capacityValueTxt.text = capacityValue.ToString();
        speedValueTxt.text = speedValue.ToString();
        speedTxt.text = System.String.Format("{0:0.0}", speed);
    }

    private void Update()
    {
        if (MoneyBox.moneyValue >= capacity)
            MoneyBox.moneyValue = capacity;

        if (capacityValue > gameManager.instance.totalMoney)
            capacityBtn.interactable = false;
        else
            capacityBtn.interactable = true;

        if (speedValue > gameManager.instance.totalMoney) 
            speedBtn.interactable = false;
        else
            speedBtn.interactable = true;

        if (capacityLv >= 30)
        {
            capacityBtn.interactable = false;
            capacityLvTxt.text = "Max Lv";
        }
        if (speedLv >= 30)
        {
            speedBtn.interactable = false;
            speedLvTxt.text = "Max Lv";
        }

        if (gameManager.instance.isStart)
        {
            capacityBtnObj.SetActive(false);
            speedBtnObj.SetActive(false);
        }
    }

    IEnumerator SpawnMoney()
    {
        for(int i = 0; i < Mathf.Infinity; i++)
        {
            if (MoneyBox.moneyValue < capacity)
            {
                GameObject money = Instantiate(bankMoney, spawnPoint.transform.position, Quaternion.identity);
                money.transform.SetParent(bank.transform);
                yield return new WaitForSeconds(1 / speed);
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    void LoadPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("CapacityLv"))
        {
            capacityLv = PlayerPrefs.GetInt("CapacityLv");
        }

        else
        {
            capacityLv = 1;
            PlayerPrefs.SetInt("CapacityLv", capacityLv);
        }
        if (PlayerPrefs.HasKey("SpeedLv"))
        {
            speedLv = PlayerPrefs.GetInt("SpeedLv");
        }

        else
        {
            speedLv = 1;
            PlayerPrefs.SetInt("SpeedLv", speedLv);
        }
        if (PlayerPrefs.HasKey("CapacityValue"))
        {
            capacityValue = PlayerPrefs.GetInt("CapacityValue");
        }

        else
        {
            capacityValue = 200;
            PlayerPrefs.SetInt("CapacityValue", capacityValue);
        }
        if (PlayerPrefs.HasKey("SpeedValue"))
        {
            speedValue = PlayerPrefs.GetInt("SpeedValue");
        }

        else
        {
            speedValue = 200;
            PlayerPrefs.SetInt("SpeedValue", speedValue);
        }

        if (PlayerPrefs.HasKey("Capacity"))
            capacity = PlayerPrefs.GetInt("Capacity");
        else
        {
            capacity = 100;
            PlayerPrefs.SetInt("Capacity", capacity);
        }

        if (PlayerPrefs.HasKey("Speed"))
            speed = PlayerPrefs.GetFloat("Speed");
        else
        {
            speed = 1.1f;
            PlayerPrefs.SetFloat("Speed", speed);
        }
    }

    public void IncreaseCapacityLv()
    {
        capacityLv++;
        PlayerPrefs.SetInt("CapacityLv", capacityLv);
        capacityLvTxt.text = "Lv " + capacityLv;
        gameManager.instance.totalMoney -= capacityValue;
        IncreaseCapacity();
    }
    public void IncreaseSpeedLv()
    {
        speedLv++;
        PlayerPrefs.SetInt("SpeedLv", speedLv);
        speedLvTxt.text = "Lv " + speedLv;
        gameManager.instance.totalMoney -= speedValue;
        IncreaseSpeed();
    }

    void IncreaseCapacity()
    {
        capacity += 50;
        PlayerPrefs.SetInt("Capacity", capacity);
        capacityValue += (int)(200 * capacityLv);
        PlayerPrefs.SetInt("CapacityValue", capacityValue);
        capacityValueTxt.text = capacityValue.ToString();
    }
    void IncreaseSpeed()
    {
        speed += 0.1001f;
        PlayerPrefs.SetFloat("Speed", speed);
        speedTxt.text = System.String.Format("{0:0.0}", speed);
        speedValue += (int)(200 * speedLv);
        PlayerPrefs.SetInt("SpeedValue", speedValue);
        speedValueTxt.text = speedValue.ToString();
    }
}
