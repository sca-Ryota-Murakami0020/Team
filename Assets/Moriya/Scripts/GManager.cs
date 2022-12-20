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

    //画面切り変わる時
    private bool displaySwitchingFlag = false;
    //タイムアップor死亡時
    private bool playerDeathFlag = false;
    //リザルト時のランキング決定時
    private bool rankingFlag = false;
    //ボタン押したときの効果音(決定時)
    private bool decisionFlag = false;
    //ボタン押したときの効果音(取り消し時)
    private bool returnFlag = false;


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

    public bool DisplaySwitchingFlag//画面切り替え
    {
        get { return this.displaySwitchingFlag; }
        set { this.displaySwitchingFlag = value; }
    }

    public bool PlayerDeathFlag//プレイヤー死亡時
    {
        get { return this.playerDeathFlag; }
        set { this.playerDeathFlag = value; }
    }

    public bool RankingFlag//ランキング決定時
    {
        get { return this.rankingFlag; }
        set { this.rankingFlag = value; }
    }

    public bool DecisionFlag//ボタン決定時
    {
        get { return this.decisionFlag; }
        set { this.decisionFlag = value; }
    }

    private bool ReturnFlag//取り消し時
    {
        get { return this.returnFlag; }
        set { this.returnFlag = value; }
    }


    //シングルトン
    private void Awake()
    {
        //AudioSource呼び出し
        audios = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
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

        if(displaySwitchingFlag == true)
        {
            audios.clip = bgms[5];
            audios.Play();
            displaySwitchingFlag = false;
        }

        if(playerDeathFlag == true)
        {
            audios.clip = bgms[6];
            audios.Play();
            playerDeathFlag = false;
        }

        if(rankingFlag == true)
        {
            audios.clip = bgms[7];
            audios.Play();
            rankingFlag = false;
        }

        if(decisionFlag == true)
        {
            audios.clip = bgms[9];
            audios.Play();
            decisionFlag = false;
        }

        if(returnFlag == true)
        {
            audios.clip = bgms[10];
            audios.Play();
            decisionFlag = false;
        }
    }
}


