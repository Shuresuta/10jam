using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class GameManager : MonoBehaviour
{
    //���ʃt�@�C���p�X
    [SerializeField] string FilePath;
    //�ȃp�X
    [SerializeField] string ClipPath;

    //���ʂ̎��
    [SerializeField] GameObject OneNotes;//1���[���̕���
    [SerializeField] GameObject TowNotes;//2���[���̕���
    [SerializeField] GameObject ThreeNotes;//3���[���̕���
    [SerializeField] GameObject FourNotes;//4���[���̕���

    //����|�W�V����
    [SerializeField] Transform SpawnPoint1;
    [SerializeField] Transform SpawnPoint2;
    [SerializeField] Transform SpawnPoint3;
    [SerializeField] Transform SpawnPoint4;
    [SerializeField] Transform BeatPoint;

    //�X�R�A
    [SerializeField] GameObject Combo;
    //�X�R�A�\���p�e�L�X�g
    [SerializeField] Text ScoreText;
    [SerializeField] Text ComboText;

    //�Q�[���J�n���̎���
    public float PlayTime;
    //�m�[�c�̏����ʒu���画�胉�C���܂ł̋���
    float Distance;
    //�m�[�c�̏����ʒu���画�胉�C���܂ł̎���
    float During;
    //�Q�[������
    bool isPlaying;
    //Notes�̃C���f�b�N�X
    int GoIndex;

    //�I�[�f�B�I�֘A
    AudioSource Music;

    //�y�ȏ��
    public static string Title;
    int BPM;
    List<GameObject> Notes;

    //�^�C�~���O
    List<float> NoteTimings;
    float GREAT;
    float GOOD;
    float check;
    string noteType;

    //�X�R�A�֘A
    float ComboCount; //�R���{��
    public static float Score;//�X�R�A
    float MaxScore = 1000000; //�V��
    int CheckTimingIndex = 0; //���e���m�[�c�̃C���f�b�N�X
    public static float Gage = 0.1f;//�Q�[�W
    [SerializeField] Image gageImage;

    //���U���g�v���p
    public static float GreatNum = 0;
    public static float CoolNum = 0;
    public static float GoodNum = 0;
    public static float MissNum = 0;

    //����摜
    [SerializeField] GameObject GREATimage;
    //[SerializeField] GameObject COOLimage;
    [SerializeField] GameObject GOODimage;
    [SerializeField] GameObject MISSimage;


    //UniRx
    // �C�x���g��ʒm
    Subject<string> MessageEffectSubject = new Subject<string>();
    // �C�x���g�����m
    public IObservable<string> OnMessageEffect
    {
        get { return MessageEffectSubject; }
    }

    //�A�N�e�B�u��ԂɂȂ����Ƃ��Ă΂��
    void OnEnable()
    {

        Music = this.GetComponent<AudioSource>();

        //�ϐ��ɒl���Z�b�g
        Distance = Math.Abs(BeatPoint.position.x - SpawnPoint1.position.x);
        During = 2 * 1000;//�m�[�c�����ʒu���画�胉�C���܂ł̎��ԁi2000(ms)�j

        //�Q�[���J�n���Ă��Ȃ�
        isPlaying = false;
        GoIndex = 0;

        //����̕�
        GREAT = 40;
        GOOD = 80;
        check = 150;

        //return�L�[�ŃQ�[���X�^�[�g
        this.UpdateAsObservable()
         .Where(_ => !isPlaying)
         .Where(_ => Input.GetKeyDown(KeyCode.Return))
         .Subscribe(_ => play());

        //���ʃ��[�h
        loadChart();
        //this.UpdateAsObservable()
        // .Where(_ => Input.GetKeyDown(KeyCode.Q))
        // .Subscribe(_ => loadChart());

        //���聫������������������������������������������
        // this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.[�L�[])).Subscribe(_ => ���s�������֐�)

        //1�`4���[���̒ʏ�m�[�c
        this.UpdateAsObservable()
          .Where(_ => isPlaying)
          .Where(_ => Input.GetKeyDown(KeyCode.A))
          .Subscribe(_ =>
          {
              beat("OneNotes", Time.time * 1000 - PlayTime);//���,�L�[����͂����Ƃ��Q�[���J�n���牽�b�o�߂�����(*1000��ms)
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

        //���̃m�[�c��
        this.UpdateAsObservable()
          .Where(_ => isPlaying)
          .Where(_ => Notes.Count > CheckTimingIndex)
          .Where(_ => NoteTimings[CheckTimingIndex] == -1)
          .Subscribe(_ => CheckTimingIndex++);

        // �m�[�c�𔭎˂���^�C�~���O���`�F�b�N���Ĕ��˂�����
        this.UpdateAsObservable()
          .Where(_ => isPlaying)
          .Where(_ => Notes.Count > GoIndex)
          .Where(_ => Notes[GoIndex].GetComponent<NoteController>().getTiming() <= ((Time.time * 1000 - PlayTime) + During))
          .Subscribe(_ => {
              Notes[GoIndex].GetComponent<NoteController>().go(Distance, During);
              GoIndex++;
          });

        //���莞�Ԃ��߂�����~�X
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

    //���ʃf�[�^�̃��[�h
    void loadChart()
    {
        //���ʃ��X�g
        Notes = new List<GameObject>();
        //�^�C�~���O���X�g
        NoteTimings = new List<float>();

        //AudioClip�z�u
        Music.clip = (AudioClip)Resources.Load(ClipPath);

        //���ʃt�@�C�����p�X����擾
        string jsonText = Resources.Load<TextAsset>(FilePath).ToString();

        JsonNode json = JsonNode.Parse(jsonText);
        Title = json["title"].Get<string>();
        BPM = int.Parse(json["bpm"].Get<string>());

        //�m�[�c�̎�ނ��`�F�b�N����notes���J��Ԃ�
        foreach (var note in json["notes"])
        {
            //type���擾
            noteType = note["type"].Get<string>();
            //�������擾
            float beat = float.Parse(note["beat"].Get<string>());//���ߐ�
            //�^�C�~���O���擾
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

    // �Q�[���J�n
    void play()
    {

        //�ȍĐ�
        Music.Stop();
        Music.Play();

        //�Q�[���J�n���̎���
        PlayTime = Time.time * 1000;
        //�v���C���ɂ���ithis.UpdateAsObservable()�𓮂����j
        isPlaying = true;
        Debug.Log("Game Start!");       
    }

    //�^�C�~���O���`�F�b�N����
    void beat(string type, float timing)
    {
        float minDiff = -1;
        int minDiffIndex = -1;

        //�L�[���͂������Ԃƍł��߂����ʂ�T��
        for (int i = 0; i < NoteTimings.Count; i++)//NoteTimings.Count�̓m�[�c�̐�
        {

            //NoteTimings[i]�͕��ʃf�[�^��timing
            if (NoteTimings[i] > 0)
            {
                //���������Ԃƈ�ԋ߂����ʂ̎���
                float diff = Math.Abs(NoteTimings[i] - timing);
                //�ł��߂����ʂ̍����o��܂ŌJ��Ԃ�
                if (minDiff == -1 || minDiff > diff)
                {
                    //�ł��߂����ʂ̍�����
                    minDiff = diff;
                    minDiffIndex = i;
                }
            }
        }
        //����͈͓��ɓ����Ă��邩
        if (minDiff != -1 & minDiff < check)
        {
            //����摜�̕\��
            GREATimage.SetActive(false);
            GOODimage.SetActive(false);
            MISSimage.SetActive(false);

            //����
            if (minDiff < GREAT & Notes[minDiffIndex].GetComponent<NoteController>().getType() == type)
            {
                NoteTimings[minDiffIndex] = -1;
                Notes[minDiffIndex].SetActive(false);
                Debug.Log("beat " + type + " GRATE.");
                GREATimage.SetActive(true);
                updateScore("GREAT");
                GreatNum += 1;

                //���肵���Ƃ��ɃG�t�F�N�g���o���Ȃ炱��
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
            Debug.Log("����O");
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
