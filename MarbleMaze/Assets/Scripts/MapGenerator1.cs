using System.Collections.Generic;
using UnityEngine;

public class MapGenerator1 : MonoBehaviour
{
    public List<GameObject> cubePrefabList;//����Ԥ�����б�
    public List<GameObject> obsPrefabList; //�ϰ���Ԥ�����б�
    public Vector3 mapSize;//��ͼ�Ĵ�Сxyz
    [Range(0f,1f)] public float outlinePercent;//�����Ŀ�϶

    public GameObject obsPrefab;//�ϰ���Ԥ����
    public GameObject targetPrefab;//Ŀ�ĵع�ȦԤ����
    public Vector3 targetAxis;     //Ŀ�ĵ�����
    public List<Coord> allcubeCoord = new List<Coord>();//�洢����λ��

    private Queue<Coord> shuffledQueue;//˽�и������Ŷ���

    [Range(0f,1f)] public float obsPercent;//�ϰ���ռ����
    public Coord playerPos;
    bool[,] mapObstacles;//�ж������������Ƿ����ϰ���

    //���������ƶ���Ԥ��
    public AudioSource music;                        //��Ч������
    public List<GameObject> cubeList = new List<GameObject>();                //δ�����ϰ���ķ����б�
    public List<Coord> allobsCoord = new List<Coord>();                 //�ϰ��﷽�������б�
    public List<GameObject> CubeRiseDownList;        //�����ƶ��ķ����б�
    public List<Transform> AlertList;                //Ԥ���б�
    public Transform alert;
    public float smoothTime_ascend;                 //�ƶ�����ƽ��ʱ��
    public float smoothTime_decend;
    private Vector3 Velocity = Vector3.zero;

    public float timer;                             //��ʱ��


    void Start()
    {
        GenerateMap();
        timer = 0f;
    }

    private void Update()
    {
        if (timer == 0f)
            findObstacle();
        timer += Time.deltaTime;
        if (timer > 3f && timer < 4)
            Alert();
        if (timer > 4.0f && timer < 10f)
            ascendObstacle();
        if (timer >= 15.0f && timer < 20.0f)
            decendObstacle();
        if (timer > 20f)
            timer = 0f;
    }

    private void GenerateMap()
    {
        bool isTarget = false;
        //��ͼ����
        for(int i = 0;i < mapSize.x; i++)
        {
            for(int j = 0;j < mapSize.z; j++)
            {
                int randomTargetId = Random.Range(0, 100);
                Vector3 newPos = new Vector3(i, 0, j);
                GameObject cube = Instantiate(cubePrefabList[Random.Range(0, cubePrefabList.Count)], newPos,Quaternion.identity);
                if (randomTargetId < 40 && isTarget == false)   //��������һ����Ȧ
                {
                    Vector3 newTargetPos = newPos + new Vector3(0, 1.2f, 0);
                    Instantiate(targetPrefab, newTargetPos, Quaternion.identity);
                    targetAxis = newTargetPos;  //�����Ȧ������
                    isTarget = true;
                }
                else    //��Ȧλ�ò������ϰ���
                {
                    allcubeCoord.Add(new Coord(i, j));
                    cubeList.Add(cube);
                }
                cube.transform.localScale *= (1 - outlinePercent);//ͨ�����������϶
            }
        }

        //�ϰ������ɲ���
        shuffledQueue = new Queue<Coord>(ShuffleCoords(allcubeCoord.ToArray()));

        int obsCount = (int)(mapSize.x * mapSize.z * obsPercent);
        playerPos = new Coord((int)mapSize.x * 3 /5, (int)mapSize.z * 3 / 5);//��ҵ�λ�ò������ϰ���
        mapObstacles = new bool[(int)mapSize.x, (int)mapSize.z];//������ͼ��bool��ά����

        int currentObsCount = 0;//������ǰ�Ѿ��������ϰ�������

        for(int i = 0;i < obsCount; i++)
        {
            Coord randomCoord = GetRandomCoord();//ȡ���ϰ��������

            mapObstacles[randomCoord.x, randomCoord.z] = true;
            currentObsCount++;

            //�жϸ������µ��ϰ����ܷ�����
            if(randomCoord != playerPos && MapIsFullyAccessible(mapObstacles, currentObsCount))
            {
                Vector3 newObsPos = new Vector3(randomCoord.x, 1f, randomCoord.z);
                GameObject obstacle = Instantiate(obsPrefabList[Random.Range(0, obsPrefabList.Count)], newObsPos, Quaternion.identity);
                obstacle.transform.localScale = new Vector3(1 - outlinePercent, 1, 1 - outlinePercent);
                allobsCoord.Add(randomCoord);
            }
            else
            {
                mapObstacles[randomCoord.x, randomCoord.z] = false;//��ԭ״̬
                currentObsCount--;
            }
        }
    }

    public Coord GetRandomCoord()
    {
        Coord randomCoord = shuffledQueue.Dequeue();//���У��Ƚ��ȳ�
        shuffledQueue.Enqueue(randomCoord);//���Ƴ���Ԫ�ط��ڶ��е����һ������֤���������ԣ���С����

        return randomCoord;
    }

    //��ˮ����㷨DFS�ж�ȫ��ͨ
    private bool MapIsFullyAccessible(bool[,] _mapObstacles, int _currentObsCount)
    {
        bool[,] mapFlags = new bool[_mapObstacles.GetLength(0), _mapObstacles.GetLength(1)];//���Լ�¼�Ƿ񱻼��

        Queue<Coord> queue = new Queue<Coord>();//���п����߷��鶼�������������
        queue.Enqueue(playerPos);//�����λ����Ϊ��ʼ��
        mapFlags[playerPos.x, playerPos.z] = true;//��ҵ�λ�ñ��Ϊ�Ѽ��

        int accessibleCount = 1;//�����ߵķ�������

        while (queue.Count > 0)//ʹ�ö�����ѭ��
        {
            Coord currentCube = queue.Dequeue();

            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    int neighborX = currentCube.x + x;
                    int neighborZ = currentCube.z + z;

                    if (x == 0 || z == 0)//ѡ�������������������
                    {
                        //�߽��ж�
                        if (neighborX >= 0 && neighborX < _mapObstacles.GetLength(0)
                        && neighborZ >= 0 && neighborZ < _mapObstacles.GetLength(1))
                        {
                            //��֤���ڵ�û�м�⵽���Ҵ˴�û���ϰ���
                            if (!mapFlags[neighborX, neighborZ] && !_mapObstacles[neighborX, neighborZ])
                            {
                                mapFlags[neighborX, neighborZ] = true;
                                accessibleCount++;
                                queue.Enqueue(new Coord(neighborX, neighborZ));
                            }
                        }
                    }
                }
            }
        }
        int obsTargetCount = (int)(mapSize.x * mapSize.z - _currentObsCount);//��ǰ�����ߵ�

        return accessibleCount == obsTargetCount;
    }

    //���ϴ���㷨
    public Coord[] ShuffleCoords(Coord[] dataArray)
    {
        for (int i = 0; i < dataArray.Length; i++)
        {
            int randomNum = Random.Range(i, dataArray.Length);

            //SWAP����˼·ʵ��ϴ���㷨
            Coord temp = dataArray[randomNum];
            dataArray[randomNum] = dataArray[i];
            dataArray[i] = temp;
        }
        return dataArray;
    }

    //�洢��������Ľṹ��
    public struct Coord
    {
        public int x;
        public int z;
        public Coord(int _x, int _z)
        {
            this.x = _x;
            this.z = _z;
        }
        //������==�ͣ�=����
        public static bool operator !=(Coord _c1, Coord _c2)
        {
            return !(_c1 == _c2);
        }

        public static bool operator ==(Coord _c1, Coord _c2)
        {
            return (_c1.x == _c2.x) && (_c1.z == _c2.z);
        }
    }

    //��������
    //�ҳ��ܹ������ķ���
    public void findObstacle()
    {
        CubeRiseDownList.Clear();
        AlertList.Clear();
        foreach (GameObject cube in cubeList)    //�������з��飬���ȡ����Ϊ�ϰ���
        {
            Coord cubeCoord = new Coord((int)cube.transform.position.x, (int)cube.transform.position.z);
            bool isobs = allobsCoord.Contains(cubeCoord);
            if (isobs == false)
            {
                int randomObstacleId = Random.Range(0, 100);
                if (randomObstacleId < 10)     //10%�ĸ��ʷ��������ƶ�
                {
                    CubeRiseDownList.Add(cube);
                    alert = cube.transform.GetChild(0);
                    AlertList.Add(alert);
                }
            }
        }
    }

    //�ϰ�������ǰ��Ԥ��
    public void Alert()
    {
        music.Play();            //Ԥ������Ч����
        foreach (Transform alert in AlertList)
        {
            alert.GetComponent<ParticleSystem>().Play();
        }
    }

    //�ϰ�������
    public void ascendObstacle()
    {
        foreach (GameObject cube in CubeRiseDownList)
        {
            Vector3 target = new Vector3(1 - outlinePercent, 2, 1 - outlinePercent);          //�ϰ�������仯
            cube.transform.localScale = Vector3.SmoothDamp(cube.transform.localScale,     //ƽ�������ƶ�
            target, ref Velocity, smoothTime_ascend);
        }
    }

    //�ϰ����½�
    public void decendObstacle()
    {
        Vector3 origin = new Vector3(1 - outlinePercent, 1 - outlinePercent, 1 - outlinePercent);    //�����ʼ����
        foreach (GameObject cube in CubeRiseDownList)
        {
            cube.transform.localScale = Vector3.MoveTowards(cube.transform.localScale, origin, smoothTime_decend); //ƽ���ƶ�
        }
    }
}

