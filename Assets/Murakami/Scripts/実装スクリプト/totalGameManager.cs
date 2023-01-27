using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class totalGameManager : MonoBehaviour
{
    #region//タイム関係ステータス
    //ハイスコア用変数
    private float[] bestTime;
    //１回のゲーム時間
    private float totalTime;
    //1プレイのリザルト用のタイム
    private string timeScore;
    //ハイスコアのデータ格納
    private string[] timer;
    //ハイスコア更新を促す用のフラグ
    //private bool counterFlag;
    //プレイ開始とプレイ終了の判定をするフラグ
    private bool timeCounter;
    //スコアの個数を数えるための変数
    private int loadCount;

    //[SerializeField] private Text nowPlayingText;

    private TextMovement textM;
    #endregion

    #region//プレイヤー関係
    //引き継ぐプレイヤーのHp
    private int playerHp = 3;
    //引き継ぐアイテムカウントの変数
    private int playerItemCount = 0;
    #endregion

    #region//プロパティ

    public float[] BestTime//ハイスコアの数値格納配列
    {
        get { return this.bestTime; }
        set { this.bestTime = value; }
    }

    public float TotalTime//1プレイ時間
    {
        get { return this.totalTime; }

        set { this.totalTime = value; }
    }

    /*public bool CounterFlag//
    {
        get { return this.counterFlag; }
        set { this.counterFlag = value; }
    }*/

    public string[] TimeText//ハイスコアの文字列格納配列
    {
        get { return this.timer; }
        set { this.timer = value; }
    }

    public string DispTime//プレイ中に表示するタイム表記
    {
        get { return this.timeScore; }
        set { this.timeScore = value; }
    }

    public int LoadCout//クリアした回数
    {
        get { return this.loadCount; }
        set { this.loadCount = value; }
    }

    public int PlayerHp//プレイヤーのＨｐ
    {
        get { return this.playerHp;}
        set { this.playerHp = value;}
    }

    public int PlayerIC//プレイヤーの鍵の所持数
    {
        get { return this.playerItemCount;}
        set { this.playerItemCount = value;}
    }

    public bool TimeCounter//計測中か判定する
    {
        get { return this.timeCounter;}
        set { this.timeCounter = value;}
    }
    #endregion

    //シングルトン
    private void Awake()
    {
        //AudioSource呼び出し
        //audios = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        timeCounter = false;
        //初期化
        SceneManager.sceneLoaded += StageLoaded;
        loadCount = 0;
        //配列の要素数の定義
        timer = new string[4];
        bestTime = new float[4];

        for (int i = 0; i <= 3; i++)
        {
            timer[i] = i.ToString();
        }
        //nowPlayingText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        #region //時間計測（村上担当）
        if (timeCounter == true)
        {
            this.totalTime += Time.deltaTime;
            //Debug.Log(totalTime);
            //nowPlayingText.text = (totalTime / 3600).ToString("00") + ":" + (totalTime / 120).ToString("00") + ":" + ((int)totalTime % 60).ToString("00");
        }

        /*if(timeCounter == false) (totalTime / 3600).ToString("00") + ":" + (bestTime[i] / 3600).ToString("00") + ":" +
        {
            this.totalTime = 0.0f;
            //nowPlayingText.text = "00:00:00";
        }*/

        //if(!pl.AliveFlag) LoadGameClear();
        #endregion
    }

    #region //リザルト反映の処理
    public void LoadGameClear()
    {
        if (loadCount == 0)
        {
            bestTime[loadCount] = totalTime;
        }
        //２回目プレイ～３回目時の記録を記録
        if (loadCount >= 1 && loadCount <= 2)
        {
            bestTime[loadCount] = totalTime;
            if (loadCount >= 1)
            {
                for (int i = loadCount; i > 0; i--)
                {
                    for (int j = loadCount - 1; j >= 0; j--)
                    {
                        if (bestTime[j] > bestTime[i])
                        {
                            float btt = bestTime[j];
                            bestTime[j] = bestTime[i];
                            bestTime[i] = btt;
                        }
                    }                    
                }
            }
        }
        //4回目以降ハイスコアを出したら記録を更新する
        if (loadCount >= 3)
        {
            bestTime[3] = totalTime;
            for (int i = 3; i >= 1; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (bestTime[j] > bestTime[i])
                    {
                        float bestTimeTem = bestTime[j];
                        bestTime[j] = bestTime[i];
                        bestTime[i] = bestTimeTem;
                        Debug.Log("i: " + bestTime[i]);
                        Debug.Log("j: " + bestTime[j]);
                    }
                }
            }
        }

        //ゲーム中に表示するタイマー表示に与える変数
        timeScore =(totalTime / 60).ToString("00") + ":" + ((int)totalTime % 60).ToString("00");

        for (int i = 0; i <= 2; i++)
        {
            timer[i] = i + 1 + "位:" +  (bestTime[i] / 60).ToString("00") + ":" + ((int)bestTime[i] % 60).ToString("00");
        }

        loadCount += 1;
    }
    #endregion

    #region//呼び出したシーンに応じての処理
    void StageLoaded(Scene nextScene, LoadSceneMode mode)
    {
        //
        if (nextScene.name == "bill" || nextScene.name == "1階" || nextScene.name == "2階" || nextScene.name == "3階")
        {
            timeCounter = true;
            //totalTime = 0.0f;
            //nowPlayingText = GetComponentInChildren<Text>();
        }

        if (nextScene.name == "LoadFirstStage" || nextScene.name == "LoadSecondStage" || nextScene.name == "LoadTherdStage")
        {
            timeCounter = false;
            //nowPlayingText = GetComponentInChildren<Text>();
        }

        if (nextScene.name == "GameOverScene" || nextScene.name == "GoalScene" || nextScene.name == "LoadBill")
        {
            timeCounter = false;
            playerHp = 3;
            playerItemCount = 0;
        }

        if (nextScene.name == "GoalScene")
        {
            textM = FindObjectOfType<TextMovement>();
        }
    }
    #endregion
}
/*
     #region//効果音関係ステータス
    ////効果音の配列設定とAudioSource呼び出し
    [SerializeField] private AudioClip[] bgms;
    private AudioSource audios =null;

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
    //private bool completeRollFlag = false;
    //ジャンプしたときの効果音フラグ
    private bool jumpFlag = false;
    //移動したときの効果音フラグ
    private bool moveFlag = false;
    //Hpが０になったときの効果音
    private bool disappearHpFlag = false;
    //加速してる時に使う移動用効果音
    private bool accelWalkFlag = false;
    //着地した時の効果音
    private bool randingFlag = false;
    #endregion
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

    /*public bool CompleteRollFlag//前転
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

public bool ReturnFlag//取り消し時
{
    get { return this.returnFlag; }
    set { this.returnFlag = value; }
}

public bool DisappeareHp//HP0になった時
{
    get { return this.disappearHpFlag; }
    set { this.disappearHpFlag = value; }
}

public bool AccelWalkFlag//加速時移動
{
    get { return this.accelWalkFlag; }
    set { this.accelWalkFlag = value; }
}

public bool RandingFlag//着地時
{
    get { return this.randingFlag; }
    set { this.randingFlag = value; }
        #region //効果音（森屋担当）
        //特定のフラグがたったら特定の効果音を鳴らす
        /*if (playerDamegeFlag == true)
        {
            audios.clip = bgms[0];
            audios.Play();
            playerDamegeFlag = false;
        }:*/

/*if (wallJumpFlag == true)
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
}*/
/*if(randingFlag == true)
{
    audios.clip = bgms[1];
    audios.Play();
    randingFlag = false;
}

if(accelWalkFlag == true)
{
    audios.clip = bgms[2];
    audios.Play();
    accelWalkFlag = false;
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
}

if(moveFlag == false)
{
    audios.clip = bgms[4];
    audios.Stop();
}

if (displaySwitchingFlag == true)
{
    audios.clip = bgms[5];
    audios.Play();
    displaySwitchingFlag = false;
}

if (playerDeathFlag == true)
{
    audios.clip = bgms[6];
    audios.Play();
    playerDeathFlag = false;
}

if (rankingFlag == true)
{
    audios.clip = bgms[7];
    audios.Play();
    rankingFlag = false;
}

if (decisionFlag == true)
{
    audios.clip = bgms[8];
    audios.Play();
    decisionFlag = false;
}

if (returnFlag == true)
{
    audios.clip = bgms[9];
    audios.Play();
    returnFlag = false;
}

if(disappearHpFlag == true)
{
    audios.clip = bgms[10];
    audios.Play();
    disappearHpFlag = false;
}
#endregion
*/
 
