using UnityEngine;

public class arrowline : MonoBehaviour
{
    public MeshRenderer meshRenderer;       //��ͷ3D����QuadԤ����
    MeshRenderer mr;
    Transform tran;
    public float speed;
    Vector3 start, end;                    //��¼��ͷ��ʼλ��
                                                                                                                                                       
    public float xscale = 1f;              //���ű���                                                                                                                                     
    public float yscale = 1f;

    void Start()
    {
        //��ͷ�������ֵ
        xscale = meshRenderer.transform.localScale.x;
        //��ͷ��������ֵ
        yscale = meshRenderer.transform.localScale.y;
        mr = Instantiate(meshRenderer, start, Quaternion.identity);  //ʵ������ͷ
        tran = mr.transform;  
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");//AD
        float vertical = Input.GetAxis("Vertical");//WS


        //�����ƶ�
        if (Input.GetKey(KeyCode.W))
        {
            tran.eulerAngles = new Vector3(90, 0, 0);
            start = transform.position + new Vector3(0, -0.4f, 0.7f);
            end += Vector3.forward * vertical * speed * Time.deltaTime;
            DrawLine(end);
        }
        //�����ƶ�
        else if (Input.GetKey(KeyCode.S))
        {
            tran.eulerAngles = new Vector3(90, 180, 0);
            start = transform.position + new Vector3(0, -0.4f, -0.7f);
            end += Vector3.forward * vertical * speed * Time.deltaTime;
            DrawLine(end);
        }
        //�����ƶ�
        else if (Input.GetKey(KeyCode.A))
        {
            tran.eulerAngles = new Vector3(90, -90, 0);
            start = transform.position + new Vector3(-0.7f, -0.4f, 0);
            end += Vector3.right * horizontal * speed * Time.deltaTime;
            DrawLine(end);
        }
        //�����ƶ�
        else if (Input.GetKey(KeyCode.D))
        {
            tran.eulerAngles = new Vector3(90, 90, 0);
            start = transform.position + new Vector3(0.7f, -0.4f, 0);
            end += Vector3.right * horizontal * speed * Time.deltaTime;
            DrawLine(end);
        }
        else   //����
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

