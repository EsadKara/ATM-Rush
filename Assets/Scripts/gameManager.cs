using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    [SerializeField] GameObject moneyPref, moneyPoint, nextBtn, playBtn;
    [SerializeField] List<GameObject> Loads;

    public static gameManager instance;
    public Animator playerAnim;
    public List<GameObject> collecteds;
    public bool isStart, isFinish;
    public  float moneyValue, totalMoney;
    public TextMeshProUGUI moneyTxt, totalMoneyTxt, levelTxt, complatedTxt;

    int level;

    Movement move;
    factorController FactorController;
    cameraFollow camFollow;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        RandomLoad();
        LoadLevel();
    }

    private void Start()
    {
        camFollow = Camera.main.GetComponent<cameraFollow>();
        FactorController = GetComponent<factorController>();
        moneyValue = 0;
        totalMoney = PlayerPrefs.GetFloat("TotalMoney");
        moneyTxt.text = moneyValue.ToString();
        totalMoneyTxt.text = System.String.Format("{0:0}", totalMoney);
        isStart = false;
        isFinish = false;
        move = GetComponent<Movement>();
        nextBtn.SetActive(false);
        playBtn.SetActive(true);
        complatedTxt.enabled = false;
        playerAnim.SetBool("isFinish", false);
    }

    private void Update()
    {
        if (move.inputHorizontal != 0 || move.isMove)
        {
            if(!isFinish)
               MoveCollectedElements();
        }
        else
        {
            if(!isFinish)
               MoveToCenter();
        }
        if (isFinish)
        {
            StartCoroutine(IsFinished());
            camFollow.UpdateCam();
            isFinish = false;
        }
        totalMoneyTxt.text = System.String.Format("{0:0}", totalMoney);
    }

    public void DeployMoneys(int index, GameObject obstacle)
    {
        if (index == 0)
        {
            index = 1;
        }
        for (int i = collecteds.Count - 1; i > index; i--)
        {
            int a = i;
            GameObject money = instance.collecteds[a];
            collecteds.Remove(money);
            Destroy(money.GetComponent<collect>());
            Destroy(money.GetComponent<Rigidbody>());
            money.transform.parent = null;
            Vector3 newPos = new Vector3(Random.Range(-0.7f, 0.7f), 0.1f, obstacle.transform.position.z + Random.Range(3.5f, 6.5f));
            money.transform.DOMove(newPos, 0.3f);
            StartCoroutine(AddTagToMoney(money));
        }
        UpdateMoneyValue();
    }

    public void StackMoney(GameObject money, int index)
    {
        money.transform.parent = this.transform;
        instance.collecteds.Add(money);
        Vector3 newPos = collecteds[index].transform.localPosition;
        newPos.y = -0.9f;
        newPos.z += 0.2f;
        money.transform.localPosition = newPos;
        moneyValue += money.GetComponent<Money>().value;
        moneyTxt.text = moneyValue.ToString();
        StartCoroutine(MakeBiggerElements());
        
    }

    void MoveCollectedElements()
    {
        for(int i = 1; i < instance.collecteds.Count; i++)
        {
            GameObject money = instance.collecteds[i];
            if (instance.collecteds.Contains(money))
            {
                Vector3 newPos = money.transform.localPosition;
                newPos.x = instance.collecteds[i-1].transform.localPosition.x;
                money.transform.DOLocalMove(newPos, 0.1f);
            }
        }
    }

    void MoveToCenter()
    {
        for (int i = 1; i < instance.collecteds.Count; i++)
        {
            GameObject money = instance.collecteds[i];
            Vector3 newPos = money.transform.localPosition;
            newPos.x = instance.collecteds[0].transform.localPosition.x;
            money.transform.DOLocalMove(newPos, 0.3f);
        }
    }

    IEnumerator MakeBiggerElements()
    {
        for (int i = instance.collecteds.Count - 1; i > 0; i--)
        {
            Vector3 normalScale = new Vector3(0.2f, 0.2f, 0.2f); ;
            Vector3 biggerScale = new Vector3(0.3f, 0.3f, 0.3f); ;
            int index = i;
            if(index>= instance.collecteds.Count)
            {
                break;
            }
            GameObject money = instance.collecteds[index];
            if (instance.collecteds.Contains(money))
            {
                Money moneyCs = money.GetComponent<Money>();
                if (moneyCs.meshcount == 1)
                {
                    normalScale = new Vector3(0.2f, 0.2f, 0.2f);
                    biggerScale = new Vector3(0.3f, 0.3f, 0.3f);
                }
                else if (moneyCs.meshcount == 2)
                {
                    normalScale = new Vector3(0.45f, 1, 0.7f);
                    biggerScale = new Vector3(0.67f, 1.5f, 1f);
                }
                else if (moneyCs.meshcount == 3)
                {
                    normalScale = new Vector3(0.45f, 0.45f, 0.45f);
                    biggerScale = new Vector3(0.67f, 0.67f, 0.67f);
                }
                money.transform.DOScale(biggerScale, 0.1f).OnComplete(() =>
                 money.transform.DOScale(normalScale, 0.1f));
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    public void UpdateMoneyValue()
    {
        moneyValue = AtmScript.moneyValue;
        if (collecteds.Count > 0)
        {
            for (int i = 1; i < collecteds.Count; i++)
            {
                moneyValue += collecteds[i].GetComponent<Money>().value;
            }
            moneyTxt.text = moneyValue.ToString();
        }
        
    }

    IEnumerator IsFinished()
    {
        isStart = false;
        playerAnim.SetBool("isFinish", true);
        playerAnim.SetBool("isStart", isStart);
        Vector3 newPos = new Vector3(0, -0.2f, collecteds[0].transform.position.z);
        collecteds[0].transform.DOMove(newPos, 1f);
        yield return new WaitForSeconds(1f);
        StartCoroutine(FactorController.CalculateFactor());
    }

    public IEnumerator UpdateTotalMoney()
    {
        Quaternion newRot = Quaternion.Euler(90, 0, 0);
        Vector3 newScale = new Vector3(0.05f, 0.05f, 0.05f);
        List<GameObject> moneys = new List<GameObject>();
        for (int i = 0; i < 20; i++)
        {
            Vector3 newPos = new Vector3(Random.Range(-0.3f, 0.3f), collecteds[0].transform.position.y + Random.Range(-0.3f, 1f),
                collecteds[0].transform.position.z - 1.35f);
            GameObject money = Instantiate(moneyPref, collecteds[0].transform.position, newRot);
            money.transform.localScale = newScale;
            money.transform.DOMove(newPos, 0.2f);
            moneys.Add(money);
            money.AddComponent<destroyObject>();
            moneyTxt.text = 0.ToString();
        }
        yield return new WaitForSeconds(0.5f);
        for (int index = 0; index < moneys.Count; index++) 
        {
            moneys[index].transform.DOMove(moneyPoint.transform.position, 1f).SetEase(Ease.OutBounce);
            totalMoney += moneyValue / moneys.Count;
            totalMoneyTxt.text = System.String.Format("{0:0}", totalMoney);
            totalMoneyTxt.transform.DOScale(new Vector3(1.3f,1.3f,1.3f),0.1f).OnComplete(() =>
             totalMoneyTxt.transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f));
            yield return new WaitForSeconds(0.05f);
        }
        PlayerPrefs.SetFloat("TotalMoney", totalMoney);
        Invoke("NextLevel", 0.5f);
    }

    void LoadLevel()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            level = PlayerPrefs.GetInt("Level");
        }
        else
        {
            level = 1;
        }
        levelTxt.text = "LEVEL " + level;
    }

    void RandomLoad()
    {
        for(int i = 0; i < Loads.Count; i++)
        {
            Loads[i].SetActive(false);
        }
        int random = Random.Range(0, 5);
        switch (random)
        {
            case 0: 
                Loads[0].SetActive(true);
                break;
            case 1:
                Loads[1].SetActive(true);
                break;
            case 2:
                Loads[2].SetActive(true);
                break;
            case 3:
                Loads[3].SetActive(true);
                break;
            case 4:
                Loads[4].SetActive(true);
                break;
        }
    }

    void NextLevel()
    {
        level += 1;
        PlayerPrefs.SetInt("Level", level);
        nextBtn.SetActive(true);
        complatedTxt.enabled = true;
    }

    public void NextLevelBtn()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayBtn()
    {
        isStart = true;
        playerAnim.SetBool("isStart", isStart);
        playBtn.SetActive(false);
    }

    IEnumerator AddTagToMoney(GameObject money)
    {
        yield return new WaitForSeconds(0.5f);
        money.tag = "CollectibleMoney";
    }

   
   

   

}
