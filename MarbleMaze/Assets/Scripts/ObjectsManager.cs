using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsManager : MonoBehaviour
{
    //MeshRenderer组件
    // public MeshRenderer thisRenderer;
    public Image countdown1;
    public Text time;
    public Text CountBar;
    public Text gametime;
    //创建一个常量接收时间变化值
    float shankeTime = 0f;
    //是否开始闪烁
    // public bool isShake = false;
    public float gametime1 = 0;
    public float gametime2 = 0;
    public AudioSource timemusic;       //获取音源
    public AudioClip timesound;           //倒计时音效
    public bool isSoundPlay = false;   //判断音效是否调用

     protected virtual void Start()
    {
        timemusic = GetComponent<AudioSource>(); //获取音频组件
    }
    private void Awake()
    {
       // music = gameObject.AddComponent<AudioSource>();  //给对象添加一个AudioSource组件
      //  music.playOnAwake = false;  //设置不一开始就播放音效
        //timesound = Resources.Load<AudioClip>("timemusic/Sound");
    }


    // Update is called once per frame
    void Update()
    {
        ToChangeColor();
        Tomusic();
    }
    /// <summary>
    /// 改变颜色逻辑
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