using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountBar : MonoBehaviour
{
    public static CountBar instance { get; private set; }
    public Text TimeBar;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Ë¢ÐÂÊ±¼ä
    public void UpdateTimeBar(int curtime)
    {
        int min = curtime / 60;
        int sec = curtime % 60;
        if (sec < 10 && min < 10)
        {
            TimeBar.text = "0" + min.ToString() + ":0" + sec.ToString();
        }
        if (sec > 10 && min < 10)
        {
            TimeBar.text = "0" + min.ToString() + ":" + sec.ToString();
        }
    }
}
