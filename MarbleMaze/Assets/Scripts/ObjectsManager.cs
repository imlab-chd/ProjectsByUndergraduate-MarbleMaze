using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsManager : MonoBehaviour
{
    //MeshRenderer���
    // public MeshRenderer thisRenderer;
    public Image countdown1;
    public Text time;
    public Text CountBar;
    public Text gametime;
    //����һ����������ʱ��仯ֵ
    float shankeTime = 0f;
    //�Ƿ�ʼ��˸
    // public bool isShake = false;
    public float gametime1 = 0;
    public float gametime2 = 0;
    public AudioSource timemusic;       //��ȡ��Դ
    public AudioClip timesound;           //����ʱ��Ч
    public bool isSoundPlay = false;   //�ж���Ч�Ƿ����

     protected virtual void Start()
    {
        timemusic = GetComponent<AudioSource>(); //��ȡ��Ƶ���
    }
    private void Awake()
    {
       // music = gameObject.AddComponent<AudioSource>();  //���������һ��AudioSource���
      //  music.playOnAwake = false;  //���ò�һ��ʼ�Ͳ�����Ч
        //timesound = Resources.Load<AudioClip>("timemusic/Sound");
    }


    // Update is called once per frame
    void Update()
    {
        ToChangeColor();
        Tomusic();
    }
    /// <summary>
    /// �ı���ɫ�߼�
    /// </summary>
    void Tomusic()
    {
        gametime2 += Time.deltaTime;
        if (gametime2 > 55 && isSoundPlay == false)
        {
            timemusic.clip = timesound;
            timemusic.Play();
            isSoundPlay = true;
        }
    }
    void ToChangeColor()
    {
        gametime1 += Time.deltaTime;
        //if (isShake)
        if (gametime1 > 50)
        {
              //timemusic.clip = timesound;
            // timemusic.loop = true;
            //timemusic.volume = 2f;
             // timemusic.Play();
           // MusicController.instance.AudioPlay(winSound);
            //if (shankeTime % 1 > 0.5f)
            if (gametime1 % 1 > 0.5f)
            {
                // thisRenderer.material.color = Color.blue;
                // countdown1.material.color = Color.red;
                countdown1.color = Color.red;
                //  countdown1.color = new Color(255/255f,0/255f,0/255f);
                // countdown2.color = Color.blue;
                // countdown3.color = Color.blue;
                time.color = Color.red;
                CountBar.color = Color.red;
                gametime.color = Color.red;
               
            }
            else
            {
                //  thisRenderer.material.color = Color.white;
                // countdown1.material.color = Color.white;
                countdown1.color = Color.white;
                //countdown1.color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
                //   countdown2.color = Color.white;
                // countdown3.color = Color.white;
                time.color = Color.white;
                CountBar.color = Color.white;
                gametime.color = Color.white;
                
            }
        }
    }

}