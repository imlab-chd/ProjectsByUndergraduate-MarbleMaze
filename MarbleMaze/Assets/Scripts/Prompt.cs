using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Prompt : MonoBehaviour    //���̲�����ʾ��Ϣ
{
    public Image image;
    float colorA;    //ͼƬ��͸����
    bool isImage;
    public float timer = 0f;  //ͼƬ���ֵ�ʱ��

    void Start()
    {
        //image = GetComponent<Image>();
        colorA = image.color.a;
        colorA = -0.01f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer < 2f)
        {
            imageAppear();
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    public void imageAppear()      //ͼƬ����
    {
        if (colorA <= 0f)//ͼƬ������
        {
            isImage = false;
           
        }
        else if (colorA >= 1)//ͼƬ����ʾ
        {
            isImage = true;
        }

        if (isImage)//����
        {
            colorA -= 1 * Time.deltaTime;
        }
        else//����
        {
            colorA += 1 * Time.deltaTime;
        }
        image.color = new Color(image.color.r, image.color.g, image.color.b, colorA);
    }
}
