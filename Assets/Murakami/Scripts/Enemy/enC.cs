using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class enC : MonoBehaviour
{
    private EnemyC ec;

    //private GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        ec = GetComponentInParent<EnemyC>();
        //enemy = GameObject.Find("Enemy").GetComponent<EnemyC>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && this.gameObject.CompareTag("PlayerD"))
        {       
            Debug.Log("�J�n");
            ec.DoEn = true;
        }
        /*
        if (other.gameObject == ec.StartP && !ec.DoEn && this.gameObject.CompareTag("PlayerD"))
        {
            Debug.Log("�J�n�n�_�ɓ��B���o�O");
        }

        else if (other.gameObject == ec.EndP && !ec.DoEn && this.gameObject.CompareTag("PlayerD"))
        {
            Debug.Log("�I�_�ɓ��B���o�O");
        }*/
    }
}
