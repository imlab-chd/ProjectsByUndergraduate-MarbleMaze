using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rd;              //定义一个刚体
    public float speed;               //蓄力的速度
    public float timer;               //计时器
    public float gametime = 0;
    public MapGenerator1 map;          //获取地图信息
    public Vector3 StartForce = Vector3.zero;
    public PauseMenue menue;        //获取菜单界面
    public AudioSource music;       //获取音源
    public AudioClip winSound;           //胜利音效
    public AudioClip loseSound;          //失败音效
    bool isWin,isLose;                 //判断是否胜利失败
    public Countgg Countdown;
    public static int showflag = 0;
    public float showTime = 3;
    private float showTimer;


    void Start()
    {
        PlayerBorn();                 //玩家初始位置
        speed = 10f;
        timer = 0f;
        isWin = false;
        isLose = false;
        rd = GetComponent<Rigidbody>(); //获取游戏物体的刚体组件
        music = GetComponent<AudioSource>(); //获取音频组件
    }
    void Update()
    {
        if (showflag != 1)
        {
        UpdateTime();
        }
        showTime -= Time.deltaTime;
        PlayerMove();
        PlayerWin();
        PlayerLose();
    }

    public void PlayerMove()
    {
        float horizontal = Input.GetAxis("Horizontal");//AD
        float vertical = Input.GetAxis("Vertical");//WS

        if (Input.GetKey(KeyCode.W) | Input.GetKey(KeyCode.S))//键盘按下
        {
            StartForce += Vector3.forward * vertical * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.D))
        {
            StartForce += Vector3.right * horizontal * speed * Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.W) | Input.GetKeyUp(KeyCode.S) | Input.GetKeyUp(KeyCode.A) | Input.GetKeyUp(KeyCode.D))//键盘抬起
        {
            rd.AddForce(StartForce, ForceMode.Impulse);
            StartForce = Vector3.zero;           //重置力
        }
    }

    public void PlayerBorn()                        //玩家初始位置
    {
        Vector3 bornPos = new Vector3(map.playerPos.x, 1.5f, map.playerPos.z);
        transform.position = bornPos;
    }

    public void PlayerWin()
    {
        //计算玩家与目的地的距离
        float dist = (map.targetAxis - transform.localPosition).magnitude;  
        if (dist < 0.8f && isWin == false)           //只调用一次
        {
            music.clip = winSound;
            music.Play();
            menue.WinGame();
            isWin = true;
        }
    }

    public void PlayerLose()
    {
        float height = transform.position.y;
        if(height < 0f && isLose == false)       //掉落失败
        {
            music.clip = loseSound;
            music.Play();
            menue.LoseGameJump();
            isLose = true;
             
        }
       // if (Countdown.remain < 1f && isLose == false)
       if(gametime > 60f && isLose == false)
        {
            music.clip = loseSound;
            music.Play();
            menue.LoseGameTime();  //超时失败
            isLose = true;
        }
    }

    public void UpdateTime()
    {
        gametime += Time.deltaTime;  //游戏时间
        CountBar.instance.UpdateTimeBar((int)gametime);
    }
}
