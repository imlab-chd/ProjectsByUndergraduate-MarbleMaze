using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseMenue : MonoBehaviour
{
    bool isPaused;  //是否暂停

    public Transform pauseMenue;     //暂停菜单
    public Transform winMenue;       //获胜菜单
    public Transform loseMenueJump;  //掉落失败菜单
    public Transform loseMenueTime;  //超时失败菜单
    public Transform settingMenue;   //设置菜单
    public Transform helpMenue;      //帮助菜单

    public Transform pauseButton;    //暂停按钮
    public Transform settingButton;  //设置按钮
    public Transform helpButton;     //帮助按钮

    public AudioSource BgmControl; //音频组件
    public AudioSource SoundControl_Map; //音效组件
    public AudioSource SoundControl_Player;
    public AudioSource SoundControl_Countdown;
    public Transform MusicOnToggle;  //音乐toggle
    public Transform MusicOffToggle;
    public Transform SoundOnToggle;  //音效toggle
    public Transform SoundOffToggle;
    public Slider Sd;         //滑动条控制难度
    public Transform pointMax;//音量控制点
    public TMP_Text timetext1,timetext2,timetext3;
    public Countgg countdown;  //用来显示时间

    public MapGenerator1 map;//获取地图信息

    


    void Start()
    {
        //界面UI显示初始化
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
    
    //回到主菜单
    public void BackHome()
    {
        SceneManager.LoadScene("Menue");
    }

    //重新开始
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }

    //游戏暂停
    public void PauseGame()
    {
        isPaused = true;
        pauseMenue.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        Time.timeScale = 0f; 
    }

    //游戏继续
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
    //游戏获胜
    public void WinGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        //pauseButton.gameObject.SetActive(false);
        timetext1.text = ((int)(countdown.gametime)).ToString() + "s";
        winMenue.gameObject.SetActive(true);
        Debug.Log("win");
    }

    //游戏失败（掉落）
    public void LoseGameJump()
    {
        isPaused = true;
        Time.timeScale = 0f;
        //pauseButton.gameObject.SetActive(false);
        timetext2.text = ((int)(countdown.gametime)).ToString() + "s";
        loseMenueJump.gameObject.SetActive(true);
        Debug.Log("lose");
    }

    //游戏失败（超时）
    public void LoseGameTime()
    { 
        //gametime += Time.deltaTime;  //游戏时间]
        isPaused = true;
        Time.timeScale = 0f;
        //pauseButton.gameObject.SetActive(false);
        // timetext3.text = ((int)(countdown.cost)).ToString() + "s";
        timetext3.text = ((int)(countdown.gametime)).ToString() + "s";
        timetext3.color = Color.red;
        loseMenueTime.gameObject.SetActive(true);
    }

    //设置按钮
    public void Setting()
    {
        isPaused = true;
        Time.timeScale = 0f;
        settingButton.gameObject.SetActive(false);
        settingMenue.gameObject.SetActive(true);
    }

    //播放音乐
    public void PlayMusic()
    {
        BgmControl.volume = 0f;
        MusicOnToggle.gameObject.SetActive(false);
        MusicOffToggle.gameObject.SetActive(true);
        //isHaveMusic = false;
    }

    //关闭音乐
    public void CloseMusic()
    {
        BgmControl.volume = 1f;
        MusicOnToggle.gameObject.SetActive(true);
        MusicOffToggle.gameObject.SetActive(false);
    }

    //打开音效
    public void PlaySoundEffects()
    {
        SoundControl_Player.volume = 0f;
        SoundControl_Map.volume = 0f;
        SoundOnToggle.gameObject.SetActive(false);
        SoundOffToggle.gameObject.SetActive(true);
        //isHaveSound = false;
    }

    //关闭音效
    public void CloseSoundEffects()
    {
        SoundControl_Player.volume = 1f;
        SoundControl_Map.volume = 1f;
        SoundOnToggle.gameObject.SetActive(true);
        SoundOffToggle.gameObject.SetActive(false);
    }

    //音量控制
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
