using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Prompt : MonoBehaviour    //键盘操作提示信息
{
    public Image image;
    float colorA;    //图片的透明度
    bool isImage;
    public float timer = 0f;  //图片出现的时间

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

    public void imageAppear()      //图片渐现
    {
        if (colorA <= 0f)//图片已隐藏
        {
            isImage = false;
           
        }
        else if (colorA >= 1)//图片已显示
        {
            isImage = true;
        }

        if (isImage)//渐隐
        {
            colorA -= 1 * Time.deltaTime;
        }
        else//渐现
        {
            colorA += 1 * Time.deltaTime;
        }
        image.color = new Color(image.color.r, image.color.g, image.color.b, colorA);
    }
}
