using UnityEngine;

public class arrowline : MonoBehaviour
{
    public MeshRenderer meshRenderer;       //箭头3D对象Quad预制体
    MeshRenderer mr;
    Transform tran;
    public float speed;
    Vector3 start, end;                    //记录箭头起始位置
                                                                                                                                                       
    public float xscale = 1f;              //缩放比例                                                                                                                                     
    public float yscale = 1f;

    void Start()
    {
        //箭头宽度缩放值
        xscale = meshRenderer.transform.localScale.x;
        //箭头长度缩放值
        yscale = meshRenderer.transform.localScale.y;
        mr = Instantiate(meshRenderer, start, Quaternion.identity);  //实例化箭头
        tran = mr.transform;  
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");//AD
        float vertical = Input.GetAxis("Vertical");//WS


        //向上移动
        if (Input.GetKey(KeyCode.W))
        {
            tran.eulerAngles = new Vector3(90, 0, 0);
            start = transform.position + new Vector3(0, -0.4f, 0.7f);
            end += Vector3.forward * vertical * speed * Time.deltaTime;
            DrawLine(end);
        }
        //向下移动
        else if (Input.GetKey(KeyCode.S))
        {
            tran.eulerAngles = new Vector3(90, 180, 0);
            start = transform.position + new Vector3(0, -0.4f, -0.7f);
            end += Vector3.forward * vertical * speed * Time.deltaTime;
            DrawLine(end);
        }
        //向左移动
        else if (Input.GetKey(KeyCode.A))
        {
            tran.eulerAngles = new Vector3(90, -90, 0);
            start = transform.position + new Vector3(-0.7f, -0.4f, 0);
            end += Vector3.right * horizontal * speed * Time.deltaTime;
            DrawLine(end);
        }
        //向右移动
        else if (Input.GetKey(KeyCode.D))
        {
            tran.eulerAngles = new Vector3(90, 90, 0);
            start = transform.position + new Vector3(0.7f, -0.4f, 0);
            end += Vector3.right * horizontal * speed * Time.deltaTime;
            DrawLine(end);
        }
        else   //重置
        {
            end = Vector3.zero;
            tran.localScale = new Vector3(xscale, 0, 1);
            tran.position = start;
        }
    }

    public void DrawLine(Vector3 end)
    {
        tran.position = start;
        var length = Vector3.Magnitude(end);
        tran.localScale = new Vector3(xscale, 1, length);
    }
}

