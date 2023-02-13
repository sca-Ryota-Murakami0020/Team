using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class goalLogC : MonoBehaviour
{
    //TimeTextC
    private TimeTextC tTC;
    //Šg‘å‚·‚é‰æ‘œ
    [SerializeField] private RectTransform goalImage;

    // Start is called before the first frame update
    void Start()
    {
        tTC = FindObjectOfType<TimeTextC>();
        goalImage = gameObject.GetComponent<RectTransform>();
        //‰æ‘œ‚Ì‘å‚«‚³‚ð0‚É‚·‚é
        goalImage.transform.localScale = new Vector3(0,0,0);       
    }

    public void SrartGoalCorotine()
    {
        //ƒRƒ‹[ƒ`ƒ“ŠJŽn
        StartCoroutine("UpScale");
    }

    //ƒQ[ƒ€ƒNƒŠƒA‚ÌƒƒS‚ðŠg‘å‚·‚é
    private IEnumerator UpScale()
    {
        //ˆê’è‚Ì‘å‚«‚³‚Ü‚ÅŠg‘å‚·‚é
        while(this.transform.localScale.x >= 1000 && this.transform.localScale.y >= 300)
        {
            goalImage.transform.localScale += new Vector3(10,3,0);
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }
}
