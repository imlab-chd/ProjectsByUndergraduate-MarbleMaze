using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseMenue : MonoBehaviour
{
    bool isPaused;  //�Ƿ���ͣ

    public Transform pauseMenue;     //��ͣ�˵�
    public Transform winMenue;       //��ʤ�˵�
    public Transform loseMenueJump;  //����ʧ�ܲ˵�
    public Transform loseMenueTime;  //��ʱʧ�ܲ˵�
    public Transform settingMenue;   //���ò˵�
    public Transform helpMenue;      //�����˵�

    public Transform pauseButton;    //��ͣ��ť
    public Transform settingButton;  //���ð�ť
    public Transform helpButton;     //������ť

    public AudioSource BgmControl; //��Ƶ���
    public AudioSource SoundControl_Map; //��Ч���
    public AudioSource SoundControl_Player;
    public AudioSource SoundControl_Countdown;
    public Transform MusicOnToggle;  //����toggle
    public Transform MusicOffToggle;
    public Transform SoundOnToggle;  //��Чtoggle
    public Transform SoundOffToggle;
    public Slider Sd;         //�����������Ѷ�
    public Transform pointMax;//�������Ƶ�
    public TMP_Text timetext1,timetext2,timetext3;
    public Countgg countdown;  //������ʾʱ��

    public MapGenerator1 map;//��ȡ��ͼ��Ϣ

    


    void Start()
    {
        //����UI��ʾ��ʼ��
        pauseMenue.gameObject.SetActive(false);
        winMenue.gameObject.SetActive(false);
        loseMenueJump.gameObject.SetActive(false);
        loseMenueTime.gameObject.SetActive(false);
        settingMenue.gameObject.SetActive(false);
        helpMenue.gameObject.SetActive(false);

        pauseButton.gameObject.SetActive(true);
        settingButton.gameObject.SetActive(true);
        helpButton.gameObject.SetActive(true);

        BgmControl = GameObject.Find("BGM").GetComponent<AudioSource>();
        SoundControl_Map = GameObject.Find("Map").GetComponent<AudioSource>();
        SoundControl_Player = GameObject.Find("Player").GetComponent<AudioSource>();
        SoundControl_Countdown = GameObject.Find("Countdown").GetComponent<AudioSource>();
        isPaused = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            ContinueGame();
        }
    }
    
    //�ص����˵�
    public void BackHome()
    {
        SceneManager.LoadScene("Menue");
    }

    //���¿�ʼ
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }

    //��Ϸ��ͣ
    public void PauseGame()
    {
        isPaused = true;
        pauseMenue.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        Time.timeScale = 0f; 
    }

    //��Ϸ����
    public void ContinueGame()
    {
        isPaused = false;
        pauseMenue.gameObject.SetActive(false);
        settingMenue.gameObject.SetActive(false);
        helpMenue.gameObject.SetActive(false);

        pauseButton.gameObject.SetActive(true);
        settingButton.gameObject.SetActive(true);
        helpButton.gameObject.SetActive(true);
        Time.timeScale = 1f;
    }
    //��Ϸ��ʤ
    public void WinGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        //pauseButton.gameObject.SetActive(false);
        timetext1.text = ((int)(countdown.gametime)).ToString() + "s";
        winMenue.gameObject.SetActive(true);
        Debug.Log("win");
    }

    //��Ϸʧ�ܣ����䣩
    public void LoseGameJump()
    {
        isPaused = true;
        Time.timeScale = 0f;
        //pauseButton.gameObject.SetActive(false);
        timetext2.text = ((int)(countdown.gametime)).ToString() + "s";
        loseMenueJump.gameObject.SetActive(true);
        Debug.Log("lose");
    }

    //��Ϸʧ�ܣ���ʱ��
    public void LoseGameTime()
    { 
        //gametime += Time.deltaTime;  //��Ϸʱ��]
        isPaused = true;
        Time.timeScale = 0f;
        //pauseButton.gameObject.SetActive(false);
        // timetext3.text = ((int)(countdown.cost)).ToString() + "s";
        timetext3.text = ((int)(countdown.gametime)).ToString() + "s";
        timetext3.color = Color.red;
        loseMenueTime.gameObject.SetActive(true);
    }

    //���ð�ť
    public void Setting()
    {
        isPaused = true;
        Time.timeScale = 0f;
        settingButton.gameObject.SetActive(false);
        settingMenue.gameObject.SetActive(true);
    }

    //��������
    public void PlayMusic()
    {
        BgmControl.volume = 0f;
        MusicOnToggle.gameObject.SetActive(false);
        MusicOffToggle.gameObject.SetActive(true);
        //isHaveMusic = false;
    }

    //�ر�����
    public void CloseMusic()
    {
        BgmControl.volume = 1f;
        MusicOnToggle.gameObject.SetActive(true);
        MusicOffToggle.gameObject.SetActive(false);
    }

    //����Ч
    public void PlaySoundEffects()
    {
        SoundControl_Player.volume = 0f;
        SoundControl_Map.volume = 0f;
        SoundOnToggle.gameObject.SetActive(false);
        SoundOffToggle.gameObject.SetActive(true);
        //isHaveSound = false;
    }

    //�ر���Ч
    public void CloseSoundEffects()
    {
        SoundControl_Player.volume = 1f;
        SoundControl_Map.volume = 1f;
        SoundOnToggle.gameObject.SetActive(true);
        SoundOffToggle.gameObject.SetActive(false);
    }

    //��������
    public void Difficultycontrol()
    {
        BgmControl.volume = Sd.value;
        if(Sd.value == 1f)
        {
            pointMax.gameObject.SetActive(true);
        }
        else
        {
            pointMax.gameObject.SetActive(false);
        }
    }

    public void help()
    {
        isPaused = true;
        Time.timeScale = 0f;
        helpButton.gameObject.SetActive(false);
        helpMenue.gameObject.SetActive(true);
    }
}
