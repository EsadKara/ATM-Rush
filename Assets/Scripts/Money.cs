using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Money : MonoBehaviour
{
    [SerializeField] Mesh moneyMesh, goldMesh, diamondMesh;

    public int meshcount, value;

    MeshFilter meshFilter;
    BoxCollider col;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        col = GetComponent<BoxCollider>();
        meshcount = 1;
    }
    void Update()
    {
        if (meshcount >= 3)
            meshcount = 3;

        if (meshcount == 1)
        {
            meshFilter.mesh = moneyMesh;
            transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            col.size = new Vector3(1.6f, 0.8f, 1f);
            value = 2;
        }


        else if (meshcount == 2)
        {
            meshFilter.mesh = goldMesh;
            transform.localScale = new Vector3(0.45f, 1, 0.7f);
            col.size = new Vector3(0.7f, 0.13f, 0.25f);
            value = 5;
        }
        else
        {
            meshFilter.mesh = diamondMesh;
            transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
            col.size = new Vector3(0.5f, 0.35f, 0.45f);
            value = 10;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Lens")
        {
            gameManager.instance.UpdateMoneyValue();
            meshcount++;
        }
        else if (other.gameObject.tag == "Point")
        {
            Destroy(gameObject);
        }
    }
}
