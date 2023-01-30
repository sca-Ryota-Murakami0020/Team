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
        //�摜�̑傫����0�ɂ���
        goalImage.transform.localScale = new Vector3(0,0,0);       
    }

    public void SrartGoalCorotine()
    {
        //�R���[�`���J�n
        StartCoroutine("UpScale");
    }

    private IEnumerator UpScale()
    {
        while(this.transform.localScale.x >= 1000 && this.transform.localScale.y >= 300)
        {
            Debug.Log("�g�咆");
            goalImage.transform.localScale += new Vector3(10,3,0);
            yield return new WaitForSeconds(0.1f);
        }
        tTC.SetStartTimer();
        yield break;
    }
}
