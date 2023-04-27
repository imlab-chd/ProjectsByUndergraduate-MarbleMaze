using UnityEngine;
using UnityEngine.UI;

public class Countgg : MonoBehaviour
{
    public Image countdown;
    public Text countdownText;
    public float time = 60f; //����ʱ��ʱ��(��)
    public float cost;//����ʱ��
    public float remain;//ʣ��ʱ��
    float countStartAt = 0f;
    public float gametime = 0;

    void Start()
    {
        countStartAt = Time.time;
        remain = time;
    }

    void Update()
    {
        gametime += Time.deltaTime;  //��Ϸʱ��
        if (Time.time - countStartAt >= time)
        {
            return;
        }
        countdown.fillAmount = 1f - (Time.time - countStartAt) / 60;
        cost = Time.time - countStartAt;
        remain = time - cost;
        countdownText.text = ((int)(remain)).ToString() + "��";
    }
}