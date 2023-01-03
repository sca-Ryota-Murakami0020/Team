using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class totalGameManager : MonoBehaviour
{
    #region//効果音関係ステータス
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
    //Hpが０になったときの効果音
    private bool disappearHpFlag = false;
    #endregion

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
    private bool counterFlag;
    //プレイ開始とプレイ終了の判定をするフラグ
    private bool startFlag;
    //スコアの個数を数えるための変数
    private int loadCount;

    [SerializeField] private Text nowPlayingText;

    private PlayerC pl;

    private TextMovement textM;
    #endregion

    #region//プロパティ
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

    public bool ReturnFlag//取り消し時
    {
        get { return this.returnFlag; }
        set { this.returnFlag = value; }
    }

    public bool DisappeareHp//HP0になった時
    {
        get { return this.disappearHpFlag;}
        set { this.disappearHpFlag = value;}
    }

    public float[] BestTime//
    {
        get { return this.bestTime; }
        set { this.bestTime = value; }
    }

    public float TotalTime//
    {
        get { return this.totalTime; }
        set { this.totalTime = value; }
    }

    public bool CounterFlag//
    {
        get { return this.counterFlag; }
        set { this.counterFlag = value; }
    }

    public string[] TimeText//
    {
        get { return this.timer; }
        set { this.timer = value; }
    }

    public string DispTime//
    {
        get { return this.timeScore; }
        set { this.timeScore = value; }
    }

    public int LoadCout//
    {
        get { return this.loadCount; }
        set { this.loadCount = value; }
    }
    #endregion

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
        startFlag = false;
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
        nowPlayingText = GetComponentInChildren<Text>();
        pl = GameObject.Find("Player").GetComponent<PlayerC>();
    }

    // Update is called once per frame
    void Update()
    {
        #region //効果音（森屋担当）
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
            audios.clip = bgms[9];
            audios.Play();
            decisionFlag = false;
        }

        if (returnFlag == true)
        {
            audios.clip = bgms[10];
            audios.Play();
            returnFlag = false;
        }

        if(disappearHpFlag == true)
        {
            audios.clip = bgms[11];
            audios.Play();
            disappearHpFlag = false;
        }
        #endregion

        #region //時間計測（村上担当）
        if (startFlag == true)
        {
            this.totalTime += Time.deltaTime;
            nowPlayingText.text = (totalTime / 3600).ToString("00") + ":" + (totalTime / 120).ToString("00") + ":" + ((int)totalTime % 60).ToString("00");
        }
        if(!pl.AliveFlag) LoadGameClear();
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
        timeScore = (totalTime / 3600).ToString("00") + ":" + (totalTime / 120).ToString("00") + ":" + ((int)totalTime % 60).ToString("00");

        for (int i = 0; i <= 2; i++)
        {
            timer[i] = i + 1 + "位:" + (bestTime[i] / 3600).ToString("00") + ":" + (bestTime[i] / 120).ToString("00") + ":" + ((int)bestTime[i] % 60).ToString("00");
        }
        loadCount += 1;
        totalTime = 0.0f;
    }
    #endregion

    #region//呼び出したシーンに応じての処理
    void StageLoaded(Scene nextScene, LoadSceneMode mode)
    {
        if (nextScene.name == "村上用")
        {
            startFlag = true;
            //Stage2が読み込まれたときにしたい処理
            //if(loadCount >= 3) bestTime[3] = 0.0f;
        }

        if (nextScene.name == "村上用Title")
        {
            startFlag = false;
        }

        if (nextScene.name == "GoalScene")
        {
            textM = FindObjectOfType<TextMovement>();
        }
    }
    #endregion
}
