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
        //EnemyC‚©‚çó‚¯æ‚éw¦
        StartCoroutine("ActiveEnemy");
    }

    private IEnumerator ActiveEnemy()
    {
        //10•bŠÔ‘Ò‹@‚·‚é
        yield return new WaitForSeconds(10);
        //10•bŒã‚ÉEnemyC‚Ì•û‚ÅSetSctive‚ğfalse‚É‚µ‚½GameObject‚ÌSetActive‚ğtrue‚É‚µ‚ÄÄ‚ÑŠˆ“®‚Å‚«‚é‚æ‚¤‚É‚µ‚Ä‚ ‚°‚é
        enemy.SetActive(true);
        yield break;
    }
}
