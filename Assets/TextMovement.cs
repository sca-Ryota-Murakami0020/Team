using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextMovement : MonoBehaviour
{
    [SerializeField] Text[] text = new Text[3];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //シーンがここにうつってきたら
        if (false)
        {
            StartCoroutine("Movement");
        }
    }

    private IEnumerator Movement()
    {
        for(int i = 0; i < 3; i++)
        {
            Vector3 pos = text[i].transform.position;
            //text[i].transform.positionを右から左へ
            while (true)
            {
                //少し座標を左へ
                pos.x -= 10.0f;
                //左端に達したらbreak
                if(pos.x <= 350)
                {
                    break;
                }
                yield return null;//1F待つ
            }
            yield return new WaitForSeconds(1);
        }
    }
}
