using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetEnemyPosition : MonoBehaviour
{

    [SerializeField] private GameObject enemy;
     
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartCountDistance()
    {
        StartCoroutine("ActiveEnemy");
    }

    private IEnumerator ActiveEnemy()
    {
        yield return new WaitForSeconds(10);
        enemy.SetActive(true);
        //Debug.Log("ê¨å˜");
        yield break;
    }
}
