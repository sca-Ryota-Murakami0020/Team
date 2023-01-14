using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Itemcounter : MonoBehaviour
{
    [SerializeField] private Sprite[] numberImage;
    private int itemCon;
    [SerializeField] private Image image;
    private totalGameManager gm;
    //SpriteRenderer sr;

    private void Awake()
    {
        gm = FindObjectOfType<totalGameManager>();
        //sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        itemCon = gm.PlayerIC;
        image.sprite =  numberImage[itemCon];
    }
}
