using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class goalLogC : MonoBehaviour
{
    private TimeTextC tTC;
    [SerializeField] private RectTransform goalImage;

    // Start is called before the first frame update
    void Start()
    {
        tTC = FindObjectOfType<TimeTextC>();
        goalImage = gameObject.GetComponent<RectTransform>();
        //画像の大きさを0にする
        goalImage.transform.localScale = new Vector3(0,0,0);       
    }

    public void SrartGoalCorotine()
    {
        //コルーチン開始
        StartCoroutine("UpScale");
    }

    private IEnumerator UpScale()
    {
        while(this.transform.localScale.x >= 1000 && this.transform.localScale.y >= 300)
        {
            Debug.Log("拡大中");
            goalImage.transform.localScale += new Vector3(10,3,0);
            yield return new WaitForSeconds(0.1f);
        }
        tTC.SetStartTimer();
        yield break;
    }
}
