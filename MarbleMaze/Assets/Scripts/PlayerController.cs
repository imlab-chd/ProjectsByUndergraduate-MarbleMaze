using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rd;              //����һ������
    public float speed;               //�������ٶ�
    public float timer;               //��ʱ��
    public float gametime = 0;
    public MapGenerator1 map;          //��ȡ��ͼ��Ϣ
    public Vector3 StartForce = Vector3.zero;
    public PauseMenue menue;        //��ȡ�˵�����
    public AudioSource music;       //��ȡ��Դ
    public AudioClip winSound;           //ʤ����Ч
    public AudioClip loseSound;          //ʧ����Ч
    bool isWin,isLose;                 //�ж��Ƿ�ʤ��ʧ��
    public Countgg Countdown;
    public static int showflag = 0;
    public float showTime = 3;
    private float showTimer;


    void Start()
    {
        PlayerBorn();                 //��ҳ�ʼλ��
        speed = 10f;
        timer = 0f;
        isWin = false;
        isLose = false;
        rd = GetComponent<Rigidbody>(); //��ȡ��Ϸ����ĸ������
        music = GetComponent<AudioSource>(); //��ȡ��Ƶ���
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

        if (Input.GetKey(KeyCode.W) | Input.GetKey(KeyCode.S))//���̰���
        {
            StartForce += Vector3.forward * vertical * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.D))
        {
            StartForce += Vector3.right * horizontal * speed * Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.W) | Input.GetKeyUp(KeyCode.S) | Input.GetKeyUp(KeyCode.A) | Input.GetKeyUp(KeyCode.D))//����̧��
        {
            rd.AddForce(StartForce, ForceMode.Impulse);
            StartForce = Vector3.zero;           //������
        }
    }

    public void PlayerBorn()                        //��ҳ�ʼλ��
    {
        Vector3 bornPos = new Vector3(map.playerPos.x, 1.5f, map.playerPos.z);
        transform.position = bornPos;
    }

    public void PlayerWin()
    {
        //���������Ŀ�ĵصľ���
        float dist = (map.targetAxis - transform.localPosition).magnitude;  
        if (dist < 0.8f && isWin == false)           //ֻ����һ��
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
        if(height < 0f && isLose == false)       //����ʧ��
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
            menue.LoseGameTime();  //��ʱʧ��
            isLose = true;
        }
    }

    public void UpdateTime()
    {
        gametime += Time.deltaTime;  //��Ϸʱ��
        CountBar.instance.UpdateTimeBar((int)gametime);
    }
}
