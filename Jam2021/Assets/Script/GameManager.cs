using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class GameManager : MonoBehaviour
{
    //譜面ファイルパス
    [SerializeField] string FilePath;
    //曲パス
    [SerializeField] string ClipPath;

    //譜面の種類
    [SerializeField] GameObject OneNotes;//1レーンの譜面
    [SerializeField] GameObject TowNotes;//2レーンの譜面
    [SerializeField] GameObject ThreeNotes;//3レーンの譜面
    [SerializeField] GameObject FourNotes;//4レーンの譜面

    //判定ポジション
    [SerializeField] Transform SpawnPoint1;
    [SerializeField] Transform SpawnPoint2;
    [SerializeField] Transform SpawnPoint3;
    [SerializeField] Transform SpawnPoint4;
    [SerializeField] Transform BeatPoint;

    //スコア
    [SerializeField] GameObject Combo;
    //スコア表示用テキスト
    [SerializeField] Text ScoreText;
    [SerializeField] Text ComboText;

    //ゲーム開始時の時間
    public float PlayTime;
    //ノーツの初期位置から判定ラインまでの距離
    float Distance;
    //ノーツの初期位置から判定ラインまでの時間
    float During;
    //ゲーム中か
    bool isPlaying;
    //Notesのインデックス
    int GoIndex;

    //オーディオ関連
    AudioSource Music;

    //楽曲情報
    public static string Title;
    int BPM;
    List<GameObject> Notes;

    //タイミング
    List<float> NoteTimings;
    float GREAT;
    float GOOD;
    float check;
    string noteType;

    //スコア関連
    float ComboCount; //コンボ数
    public static float Score;//スコア
    float MaxScore = 1000000; //天井
    int CheckTimingIndex = 0; //今弾くノーツのインデックス
    public static float Gage = 0.1f;//ゲージ
    [SerializeField] Image gageImage;

    //リザルト計数用
    public static float GreatNum = 0;
    public static float CoolNum = 0;
    public static float GoodNum = 0;
    public static float MissNum = 0;

    //判定画像
    [SerializeField] GameObject GREATimage;
    //[SerializeField] GameObject COOLimage;
    [SerializeField] GameObject GOODimage;
    [SerializeField] GameObject MISSimage;


    //UniRx
    // イベントを通知
    Subject<string> MessageEffectSubject = new Subject<string>();
    // イベントを検知
    public IObservable<string> OnMessageEffect
    {
        get { return MessageEffectSubject; }
    }

    //アクティブ状態になったとき呼ばれる
    void OnEnable()
    {

        Music = this.GetComponent<AudioSource>();

        //変数に値をセット
        Distance = Math.Abs(BeatPoint.position.x - SpawnPoint1.position.x);
        During = 2 * 1000;//ノーツ初期位置から判定ラインまでの時間（2000(ms)）

        //ゲーム開始していない
        isPlaying = false;
        GoIndex = 0;

        //判定の幅
        GREAT = 40;
        GOOD = 80;
        check = 150;

        //returnキーでゲームスタート
        this.UpdateAsObservable()
         .Where(_ => !isPlaying)
         .Where(_ => Input.GetKeyDown(KeyCode.Return))
         .Subscribe(_ => play());

        //譜面ロード
        loadChart();
        //this.UpdateAsObservable()
        // .Where(_ => Input.GetKeyDown(KeyCode.Q))
        // .Subscribe(_ => loadChart());

        //判定↓＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
        // this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.[キー])).Subscribe(_ => 実行したい関数)

        //1～4レーンの通常ノーツ
        this.UpdateAsObservable()
          .Where(_ => isPlaying)
          .Where(_ => Input.GetKeyDown(KeyCode.A))
          .Subscribe(_ =>
          {
              beat("OneNotes", Time.time * 1000 - PlayTime);//種類,キーを入力したときゲーム開始から何秒経過したか(*1000でms)
          });

        this.UpdateAsObservable()
          .Where(_ => isPlaying)
          .Where(_ => Input.GetKeyDown(KeyCode.S))
          .Subscribe(_ =>
          {
              beat("TowNotes", Time.time * 1000 - PlayTime);
          });

        this.UpdateAsObservable()
         .Where(_ => isPlaying)
         .Where(_ => Input.GetKeyDown(KeyCode.K))
         .Subscribe(_ =>
         {
             beat("ThreeNotes", Time.time * 1000 - PlayTime);
         });

        this.UpdateAsObservable()
         .Where(_ => isPlaying)
         .Where(_ => Input.GetKeyDown(KeyCode.L))
         .Subscribe(_ =>
         {
             beat("FourNotes", Time.time * 1000 - PlayTime);
         });

        //次のノーツへ
        this.UpdateAsObservable()
          .Where(_ => isPlaying)
          .Where(_ => Notes.Count > CheckTimingIndex)
          .Where(_ => NoteTimings[CheckTimingIndex] == -1)
          .Subscribe(_ => CheckTimingIndex++);

        // ノーツを発射するタイミングかチェックして発射させる
        this.UpdateAsObservable()
          .Where(_ => isPlaying)
          .Where(_ => Notes.Count > GoIndex)
          .Where(_ => Notes[GoIndex].GetComponent<NoteController>().getTiming() <= ((Time.time * 1000 - PlayTime) + During))
          .Subscribe(_ => {
              Notes[GoIndex].GetComponent<NoteController>().go(Distance, During);
              GoIndex++;
          });

        //判定時間を過ぎたらミス
        this.UpdateAsObservable()
          .Where(_ => isPlaying)
          .Where(_ => Notes.Count > CheckTimingIndex)
          .Where(_ => NoteTimings[CheckTimingIndex] != -1)
          .Where(_ => NoteTimings[CheckTimingIndex] < ((Time.time * 1000 - PlayTime) - check / 2))
          .Subscribe(_ => {
              updateScore("MISS");
              CheckTimingIndex++;

          });
    }

    //譜面データのロード
    void loadChart()
    {
        //譜面リスト
        Notes = new List<GameObject>();
        //タイミングリスト
        NoteTimings = new List<float>();

        //AudioClip配置
        Music.clip = (AudioClip)Resources.Load(ClipPath);

        //譜面ファイルをパスから取得
        string jsonText = Resources.Load<TextAsset>(FilePath).ToString();

        JsonNode json = JsonNode.Parse(jsonText);
        Title = json["title"].Get<string>();
        BPM = int.Parse(json["bpm"].Get<string>());

        //ノーツの種類をチェックしてnotes分繰り返す
        foreach (var note in json["notes"])
        {
            //typeを取得
            noteType = note["type"].Get<string>();
            //拍数を取得
            float beat = float.Parse(note["beat"].Get<string>());//小節数
            //タイミングを取得
            float timing = float.Parse(note["timing"].Get<string>());

           

            GameObject Note;

            if (noteType == "OneNotes")
            {
                Note = Instantiate(OneNotes, SpawnPoint1.position, Quaternion.identity);
            }
            else if(noteType == "TowNotes")
            {
                Note = Instantiate(TowNotes, SpawnPoint2.position, Quaternion.identity);
            }
            else if (noteType == "ThreeNotes")
            {
                Note = Instantiate(ThreeNotes, SpawnPoint3.position, Quaternion.identity);
            }
            else if (noteType == "FourNotes")
            {
                Note = Instantiate(FourNotes, SpawnPoint4.position, Quaternion.identity);
            }
            else
            {
                Note = Instantiate(OneNotes, SpawnPoint1.position, Quaternion.identity);
            }

            Note.GetComponent<NoteController>().setParameter(noteType, timing);
            Notes.Add(Note);
            NoteTimings.Add(timing);


        }
    }

    // ゲーム開始
    void play()
    {

        //曲再生
        Music.Stop();
        Music.Play();

        //ゲーム開始時の時間
        PlayTime = Time.time * 1000;
        //プレイ中にする（this.UpdateAsObservable()を動かす）
        isPlaying = true;
        Debug.Log("Game Start!");       
    }

    //タイミングをチェックする
    void beat(string type, float timing)
    {
        float minDiff = -1;
        int minDiffIndex = -1;

        //キー入力した時間と最も近い譜面を探す
        for (int i = 0; i < NoteTimings.Count; i++)//NoteTimings.Countはノーツの数
        {

            //NoteTimings[i]は譜面データのtiming
            if (NoteTimings[i] > 0)
            {
                //押した時間と一番近い譜面の時差
                float diff = Math.Abs(NoteTimings[i] - timing);
                //最も近い譜面の差が出るまで繰り返す
                if (minDiff == -1 || minDiff > diff)
                {
                    //最も近い譜面の差を代入
                    minDiff = diff;
                    minDiffIndex = i;
                }
            }
        }
        //判定範囲内に入っているか
        if (minDiff != -1 & minDiff < check)
        {
            //判定画像の表示
            GREATimage.SetActive(false);
            GOODimage.SetActive(false);
            MISSimage.SetActive(false);

            //判定
            if (minDiff < GREAT & Notes[minDiffIndex].GetComponent<NoteController>().getType() == type)
            {
                NoteTimings[minDiffIndex] = -1;
                Notes[minDiffIndex].SetActive(false);
                Debug.Log("beat " + type + " GRATE.");
                GREATimage.SetActive(true);
                updateScore("GREAT");
                GreatNum += 1;

                //判定したときにエフェクトを出すならこれ
                //Vector3 tmp = GameObject.Find("Player").transform.position;
                //tmp.z -= 0.2f;
                //GameObject obj = (GameObject)Resources.Load("shortBom");
                //Instantiate(obj, tmp, Quaternion.identity);

            }
            else if (minDiff < GOOD & Notes[minDiffIndex].GetComponent<NoteController>().getType() == type)
            {
                NoteTimings[minDiffIndex] = -1;
                Notes[minDiffIndex].SetActive(false);
                Debug.Log("beat " + type + " GOOD.");
                GOODimage.SetActive(true);
                updateScore("GOOD");
                GoodNum += 1;
            }
            else
            {
                NoteTimings[minDiffIndex] = -1;
                Notes[minDiffIndex].SetActive(false);
                Debug.Log("beat " + type + " MISS.");

                updateScore("MISS");


            }
        }
        else
        {
            Debug.Log("判定外");
        }
    }

    void updateScore(string result)
    {
        if (result == "GREAT")
        {
            float plusScore;
            float NoteNum = NoteTimings.Count;
            plusScore = MaxScore / NoteNum;
            ComboCount++;
            Score += plusScore;

            Gage += 0.01f;
            if (Gage > 1) Gage = 1;
            gageImage.GetComponent<Image>().fillAmount = Gage;
            if (Gage >= 0.7f)
            {
                gageImage.GetComponent<Image>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 0 / 255.0f, 255.0f / 255.0f);
            }

        }
        else if (result == "GOOD")
        {
            float plusScore;
            float NoteNum = NoteTimings.Count;
            plusScore = MaxScore / NoteNum;
            plusScore = plusScore / 2;
            ComboCount++;
            Score += plusScore;

            Gage += 0.01f;
            if (Gage > 1) Gage = 1;
            gageImage.GetComponent<Image>().fillAmount = Gage;
            if (Gage >= 0.7f)
            {
                gageImage.GetComponent<Image>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 0 / 255.0f, 255.0f / 255.0f);
            }
        }
        else if (result == "MISS")
        {
            MISSimage.SetActive(false);
            ComboCount = 0;
            Gage -= 0.01f;
            MissNum += 1;
            MISSimage.SetActive(true);
            //if (Gage < 0) Gage = 0;
            //gageImage.GetComponent<Image>().fillAmount = Gage;
            //if (Gage <= 0.7f)
            //{
            //    gageImage.GetComponent<Image>().color = new Color(0 / 255.0f, 150 / 255.0f, 255 / 255.0f, 255.0f / 255.0f);
            //}
        }
        else
        {
            ComboCount = 0; // default failure
        }

        ComboText.text = ComboCount.ToString();
        Score = (int)Score;
        ScoreText.text = Score.ToString();

        if (ComboCount >= 2)
        {
            Combo.SetActive(true);
        }
        else
        {
            Combo.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
