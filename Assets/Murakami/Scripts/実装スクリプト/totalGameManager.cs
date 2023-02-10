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
    private float totalTime = 0.0f;
    //スコアの個数を数えるための変数
    private int loadCount = 0;
    //プレイ開始とプレイ終了の判定をするフラグ
    private bool timeCounter = false;
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
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //シーンに応じた処理ができるようにする
        SceneManager.sceneLoaded += StageLoaded;
        //配列の要素数の定義
        bestTime = new float[4];
    }

    // Update is called once per frame
    void Update()
    {
        #region //時間計測（村上担当）
        //計測させるフラグtimeCoounterがtrueの間は時間計測を行う
        if (timeCounter == true)
        {
            this.totalTime += Time.deltaTime;
        }
        #endregion
    }

    #region //リザルト反映の処理
    public void LoadGameClear()
    {
        //初回のプレイの記録をハイスコア第1位の配列に格納する
        if (loadCount == 0)
        {
            //1位のランキングの要素に格納する
            bestTime[loadCount] = totalTime;
        }

        //２回目プレイ～３回目時の記録を記録
        if (loadCount >= 1 && loadCount <= 2)
        {
            //プレイ回数に応じた番地にクリア時間を代入する
            //プレイ回数に応じた番地にした理由は既にデータが格納されている番地に格納しようとすると
            //データが上書きされるのでランキングが正常に動作しないから
            bestTime[loadCount] = totalTime;
            //下のバブルソートでランキングを更新する
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
            //bestTimeの3番地は必ず最新のクリア時間を入れる
            bestTime[3] = totalTime;
            //ここのバブルソートでランキングの更新を行う
            for (int i = 3; i >= 1; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (bestTime[j] > bestTime[i])
                    {
                        float bestTimeTem = bestTime[j];
                        bestTime[j] = bestTime[i];
                        bestTime[i] = bestTimeTem;
                    }
                }
            }
        }

        //リザルト画面に飛んだ回数を足す
        loadCount += 1;
    }
    #endregion

    #region//呼び出したシーンに応じての処理
    void StageLoaded(Scene nextScene, LoadSceneMode mode)
    {
        //それぞれのプレイシーンが呼び出されたら
        if (nextScene.name == "bill" || nextScene.name == "1階" || nextScene.name == "2階" || nextScene.name == "3階" || nextScene.name == "LastScene")
        {
            //タイム計測を開始or再開させる
            timeCounter = true;
        }

        //それぞれのプレイシーンの前に呼び出すロードシーンが呼び出されたら
        if (nextScene.name == "LoadFirstStage" || nextScene.name == "LoadSecondStage" || nextScene.name == "LoadTherdStage" || nextScene.name == "LoadLastStage")
        {
            //タイム計測を一旦停止させる
            timeCounter = false;
        }

        //ゲームオーバーシーンorリザルト画面が呼び出されたら
        if (nextScene.name == "GameOverScene" || nextScene.name == "GoalScene")
        {
            //タイム計測を一旦停止させる
            timeCounter = false;
            //プレイヤーに与える体力を初期化
            playerHp = 3;
            //アイテムの所持数も初期化
            playerItemCount = 0;
        }

        //あらゆる場面からリトライする処理を受けたら
        if ( nextScene.name == "LoadBill")
        {
            //タイム計測を一旦停止する
            timeCounter = false;
            //プレイヤーの体力の初期化
            playerHp = 3;
            //アイテムの所持数の初期化
            playerItemCount = 0;
            //１ゲームのプレイ時間の初期化
            totalTime = 0.0f;
        }
    }
    #endregion
}
 
