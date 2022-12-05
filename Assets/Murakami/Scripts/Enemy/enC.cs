using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enC : MonoBehaviour
{
    private EnemyC ec;
    //private GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        ec = GetComponent<EnemyC>();
        //enemy = GameObject.Find("Enemy").GetComponent<EnemyC>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ec.DoEn = true;
            Debug.Log("EstartŠJŽn");
        }
    }
}
