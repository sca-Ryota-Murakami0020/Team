using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerposition : MonoBehaviour
{
    [SerializeField] private GameObject resetPosition;
    private Player pl;

    // Start is called before the first frame update
    void Start()
    {
        pl = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {

            pl.transform.position = resetPosition.transform.position;
        }
    }
}
