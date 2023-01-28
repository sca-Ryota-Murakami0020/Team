using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondMImage : MonoBehaviour
{
    //TotalGameManager
    private totalGameManager gm;
    //•\¦‚·‚é‰æ‘œ‚Ì”z—ñ
    [SerializeField] private Sprite[] numberImage;
    //‰æ‘œ‚ğ•\¦‚·‚éImage
    [SerializeField] private Image image;
    //TotalGameManager‚©‚çæ“¾‚µ‚½ƒQ[ƒ€ŠÔ‚ğintŒ^‚Æ‚µ‚Äˆµ‚¦‚é‚æ‚¤‚É‚·‚é‚½‚ß‚Ì•Ï”
    private int SecondMCount;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<totalGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //TotalGameManager‚©‚çæ“¾‚µ‚½ƒQ[ƒ€ŠÔ‚ğintŒ^‚É•ÏŠ·
        SecondMCount = Mathf.FloorToInt(gm.TotalTime / 600);
        //ƒQ[ƒ€ŠÔ‚Ì10•b’PˆÊ‚Ì•\¦‚ğs‚¤(0`5)
        image.sprite = numberImage[SecondMCount];
    }
}
