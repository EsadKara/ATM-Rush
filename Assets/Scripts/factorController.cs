using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class factorController : MonoBehaviour
{
    [SerializeField] List<GameObject> factorObjects;
    [SerializeField] GameObject factorPref, factorsParent, moneyPref, moneysParent, canvas;

    void Start()
    {
        InstantiateFactors();
    }

    void InstantiateFactors()
    {
        float factorValue = 1.0f;
        Vector3 newPos = new Vector3(0, 0.5f, 73);
        for (int i = 0; i <= 90; i++)
        {
            GameObject factor = Instantiate(factorPref, newPos, Quaternion.identity);
            factor.transform.SetParent(factorsParent.transform);
            factorObjects.Add(factor);
            factor.GetComponent<factors>().factorTxt.text = System.String.Format("{0:0.0}", factorValue) + "X";
            factor.GetComponent<factors>().value = factorValue;
            factorValue += 0.1001f;
            newPos.y += 1f;
        }
    }
    
    public IEnumerator CalculateFactor()
    {
        gameManager.instance.collecteds[0].transform.parent = null;
        GameObject lastFactor = null;
        for (int i = 0; i < gameManager.instance.moneyValue; i++)
        {
            GameObject money = Instantiate(moneyPref, new Vector3(0, 0.1f + (i * 0.15f), 70), Quaternion.identity);
            money.transform.SetParent(moneysParent.transform);
            money.tag = "Untagged";
            gameManager.instance.collecteds[0].transform.position = new Vector3(0, money.transform.position.y + 0.1f, 70);
            Quaternion newRot = Quaternion.Euler(0, 180, 0);
            Quaternion newRot2 = Quaternion.Euler(0, 0, 0);
            canvas.transform.rotation = newRot2;
            canvas.transform.position = new Vector3(gameManager.instance.collecteds[0].transform.position.x, 
                gameManager.instance.collecteds[0].transform.position.y + 0.15f, gameManager.instance.collecteds[0].transform.position.z);
            gameManager.instance.collecteds[0].transform.rotation = newRot;
            gameManager.instance.playerAnim.SetBool("sitting", true);
            for (int j = 0; j < factorObjects.Count; j++)
            {
                if (gameManager.instance.collecteds[0].transform.position.y >= factorObjects[j].transform.position.y)
                {
                    factorObjects[j].GetComponent<MeshRenderer>().material.color = Color.green;
                    lastFactor = factorObjects[j];
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
        gameManager.instance.moneyValue *= lastFactor.GetComponent<factors>().value;
        gameManager.instance.moneyTxt.text = System.String.Format("{0:0}", gameManager.instance.moneyValue);
        gameManager.instance.moneyTxt.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.2f).OnComplete(() =>
           gameManager.instance.moneyTxt.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.2f));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(gameManager.instance.UpdateTotalMoney());

    }
}
