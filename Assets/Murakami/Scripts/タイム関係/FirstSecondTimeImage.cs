using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//0`9‚Ü‚Å‚ÌˆêŒ…‚Ü‚Å‚ÌImage‚Ì•\¦
public class FirstSecondTimeImage : MonoBehaviour
{
    //TotalGameManger
    private totalGameManager gm;
    //•\¦‚·‚é‰æ‘œ‚Ì”z—ñ
    [SerializeField] private Sprite[] numberImage;
    //‰æ‘œ‚ğ•\¦‚·‚éImage
    [SerializeField] private Image image;
    //TotalGameManager‚©‚çæ“¾‚µ‚½ƒQ[ƒ€ŠÔ‚ğintŒ^‚Æ‚µ‚Äˆµ‚¦‚é‚æ‚¤‚É‚·‚é‚½‚ß‚Ì•Ï”
    private int firstSCount;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<totalGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //TotalGameManager‚©‚çæ“¾‚µ‚½ƒQ[ƒ€ŠÔ‚ğintŒ^‚É•ÏŠ·
        firstSCount = Mathf.FloorToInt(gm.TotalTime % 10);
        //ƒQ[ƒ€ŠÔ‚Ì1•b’PˆÊ‚Ì•\¦‚ğs‚¤(0`9)
        image.sprite = numberImage[firstSCount];
    }
}
