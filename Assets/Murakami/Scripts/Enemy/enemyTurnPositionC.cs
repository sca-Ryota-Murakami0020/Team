using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�G������_�ɂ������̔�����s��
public class enemyTurnPositionC : MonoBehaviour
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
        if (other.gameObject == ec.StartP && !ec.DoEn && this.gameObject.CompareTag("PositionD"))
        {
            Debug.Log("Cstart�J�n");
            ec.returnLookEndPosition();
        }

        else if (other.gameObject == ec.EndP && !ec.DoEn && this.gameObject.CompareTag("PositionD"))
        {
            Debug.Log("Cend�J�n");
            ec.returnLookStartPosition();
        }
    }
}
