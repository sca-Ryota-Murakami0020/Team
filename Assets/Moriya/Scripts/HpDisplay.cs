using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpDisplay : MonoBehaviour
{
    private Player p; 
    // Start is called before the first frame update
    void Start()
    {
        p = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
