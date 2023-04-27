using UnityEngine;
using UnityEngine.UI;

public class Countgg : MonoBehaviour
{
    public Image countdown;
    public Text countdownText;
    public float time = 60f; //倒计时总时间(秒)
    public float cost;//花费时间
    public float remain;//剩余时间
    float countStartAt = 0f;
    public float gametime = 0;

    void Start()
    {
        countStartAt = Time.time;
        remain = time;
    }

    void Update()
    {
        gametime += Time.deltaTime;  //游戏时间
        if (Time.time - countStartAt >= time)
        {
            return;
        }
        countdown.fillAmount = 1f - (Time.time - countStartAt) / 60;
        cost = Time.time - countStartAt;
        remain = time - cost;
        countdownText.text = ((int)(remain)).ToString() + "秒";
    }
}