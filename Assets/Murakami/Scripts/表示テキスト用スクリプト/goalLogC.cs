using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class goalLogC : MonoBehaviour
{
    //TimeTextC
    private TimeTextC tTC;
    //拡大する画像
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

    //ゲームクリアのロゴを拡大する
    private IEnumerator UpScale()
    {
        //一定の大きさまで拡大する
        while(this.transform.localScale.x >= 1000 && this.transform.localScale.y >= 300)
        {
            goalImage.transform.localScale += new Vector3(10,3,0);
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }
}
