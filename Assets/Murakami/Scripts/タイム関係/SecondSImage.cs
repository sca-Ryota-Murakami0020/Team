using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondSImage : MonoBehaviour
{
    //TotalGameManager
    private totalGameManager gm;
    //•\¦‚·‚é‰æ‘œ‚Ì”z—ñ
    [SerializeField] private Sprite[] numberImage;
    //‰æ‘œ‚ğ•\¦‚·‚éImage
    [SerializeField] private Image image;
    //TotalGameManager‚©‚çæ“¾‚µ‚½ƒQ[ƒ€ŠÔ‚ğintŒ^‚Æ‚µ‚Äˆµ‚¦‚é‚æ‚¤‚É‚·‚é‚½‚ß‚Ì•Ï”
    private int secondSCount;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<totalGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //TotalGameManager‚©‚çæ“¾‚µ‚½ƒQ[ƒ€ŠÔ‚ğintŒ^‚É•ÏŠ·
        secondSCount = Mathf.FloorToInt(gm.TotalTime % 60);
        //ƒQ[ƒ€ŠÔ‚Ì10•b’PˆÊ‚Ì•\¦‚ğs‚¤(0`5)
        image.sprite = numberImage[secondSCount / 10];
    }
}
