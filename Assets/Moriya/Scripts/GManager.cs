using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviour
{
    //GM

    ////効果音の配列設定とAudioSource呼び出し
    [SerializeField] private AudioClip[] bgms;
    private AudioSource audios;

    //敵に当たった時になる効果音フラグ
    private bool playerDamegeFlag = false;
    //壁ジャンプしたときの効果音フラグ
    private bool wallJumpFlag = false;
    //ローリングしたときの効果音フラグ
    private bool completeRollFlag = false;
    //ジャンプしたときの効果音フラグ
    private bool jumpFlag = false;
    //移動したときの効果音フラグ
    private bool moveFlag = false;
    //ワイヤーを発射したときの効果音フラグ
    private bool wireStartFlag = false;
    //ワイヤーが当たった時の効果音フラグ
    private bool wireStopFlag = false;


    //シングルトン
    private void Awake()
    {
        //AudioSource呼び出し
        audios = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
    }

    public bool PDFlag//プレイヤーダメージ
    {
        get { return this.playerDamegeFlag; }
        set { this.playerDamegeFlag = value; }
    }

    public bool WallJumpFlag//壁ジャンプ
    {
        get { return this.wallJumpFlag; }
        set { this.wallJumpFlag = value; }
    }

    public bool CompleteRollFlag//前転
    {
        get { return this.completeRollFlag; }
        set { this.completeRollFlag = value; }
    }

    public bool JumpFlag//ジャンプ
    {
        get { return this.jumpFlag; }
        set { this.jumpFlag = value; }
    }

    public bool MoveFlag//移動
    {
        get { return this.moveFlag; }
        set { this.moveFlag = value; }
    }

    public bool WireStartFlag//ワイヤースタート
    {
        get { return this.wireStartFlag; }
        set { this.wireStartFlag = value; }
    }

    public bool WireStopFlag//ワイヤーストップ
    {
        get { return this.wireStopFlag; }
        set { this.wireStopFlag = value; }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //特定のフラグがたったら特定の効果音を鳴らす
        if (playerDamegeFlag == true)
        {
            audios.clip = bgms[0];
            audios.Play();
            playerDamegeFlag = false;
        }

        if (wallJumpFlag == true)
        {
            audios.clip = bgms[1];
            audios.Play();
            wallJumpFlag = false;
        }

        if (completeRollFlag == true)
        {
            audios.clip = bgms[2];
            audios.Play();
            completeRollFlag = false;
        }

        if (jumpFlag == true)
        {
            audios.clip = bgms[3];
            audios.Play();
            jumpFlag = false;
        }

        if (moveFlag == true)
        {
            audios.clip = bgms[4];
            audios.Play();
            moveFlag = false;
        }

        if (wireStartFlag == true)
        {
            audios.clip = bgms[5];
            audios.Play();
            wireStartFlag = false;
        }

        if (wireStopFlag == true)
        {
            audios.clip = bgms[6];
            audios.Play();
            wireStopFlag = false;
        }
    }
}


