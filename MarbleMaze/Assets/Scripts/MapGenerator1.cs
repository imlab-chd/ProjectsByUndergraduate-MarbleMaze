using System.Collections.Generic;
using UnityEngine;

public class MapGenerator1 : MonoBehaviour
{
    public List<GameObject> cubePrefabList;//方块预制体列表
    public List<GameObject> obsPrefabList; //障碍物预制体列表
    public Vector3 mapSize;//地图的大小xyz
    [Range(0f,1f)] public float outlinePercent;//方块间的空隙

    public GameObject obsPrefab;//障碍物预制体
    public GameObject targetPrefab;//目的地光圈预制体
    public Vector3 targetAxis;     //目的地坐标
    public List<Coord> allcubeCoord = new List<Coord>();//存储所有位置

    private Queue<Coord> shuffledQueue;//私有辅助混排队列

    [Range(0f,1f)] public float obsPercent;//障碍物占有率
    public Coord playerPos;
    bool[,] mapObstacles;//判断任意坐标上是否有障碍物

    //方块上下移动及预警
    public AudioSource music;                        //音效播放器
    public List<GameObject> cubeList = new List<GameObject>();                //未覆盖障碍物的方块列表
    public List<Coord> allobsCoord = new List<Coord>();                 //障碍物方块坐标列表
    public List<GameObject> CubeRiseDownList;        //上下移动的方块列表
    public List<Transform> AlertList;                //预警列表
    public Transform alert;
    public float smoothTime_ascend;                 //移动方块平滑时间
    public float smoothTime_decend;
    private Vector3 Velocity = Vector3.zero;

    public float timer;                             //计时器


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
        //地图生成
        for(int i = 0;i < mapSize.x; i++)
        {
            for(int j = 0;j < mapSize.z; j++)
            {
                int randomTargetId = Random.Range(0, 100);
                Vector3 newPos = new Vector3(i, 0, j);
                GameObject cube = Instantiate(cubePrefabList[Random.Range(0, cubePrefabList.Count)], newPos,Quaternion.identity);
                if (randomTargetId < 40 && isTarget == false)   //用来生成一个光圈
                {
                    Vector3 newTargetPos = newPos + new Vector3(0, 1.2f, 0);
                    Instantiate(targetPrefab, newTargetPos, Quaternion.identity);
                    targetAxis = newTargetPos;  //保存光圈的坐标
                    isTarget = true;
                }
                else    //光圈位置不生成障碍物
                {
                    allcubeCoord.Add(new Coord(i, j));
                    cubeList.Add(cube);
                }
                cube.transform.localScale *= (1 - outlinePercent);//通过缩放制造间隙
            }
        }

        //障碍物生成部分
        shuffledQueue = new Queue<Coord>(ShuffleCoords(allcubeCoord.ToArray()));

        int obsCount = (int)(mapSize.x * mapSize.z * obsPercent);
        playerPos = new Coord((int)mapSize.x * 3 /5, (int)mapSize.z * 3 / 5);//玩家的位置不生成障碍物
        mapObstacles = new bool[(int)mapSize.x, (int)mapSize.z];//创建地图的bool二维矩阵

        int currentObsCount = 0;//场景当前已经创建的障碍物数量

        for(int i = 0;i < obsCount; i++)
        {
            Coord randomCoord = GetRandomCoord();//取得障碍物的坐标

            mapObstacles[randomCoord.x, randomCoord.z] = true;
            currentObsCount++;

            //判断该坐标下的障碍物能否生成
            if(randomCoord != playerPos && MapIsFullyAccessible(mapObstacles, currentObsCount))
            {
                Vector3 newObsPos = new Vector3(randomCoord.x, 1f, randomCoord.z);
                GameObject obstacle = Instantiate(obsPrefabList[Random.Range(0, obsPrefabList.Count)], newObsPos, Quaternion.identity);
                obstacle.transform.localScale = new Vector3(1 - outlinePercent, 1, 1 - outlinePercent);
                allobsCoord.Add(randomCoord);
            }
            else
            {
                mapObstacles[randomCoord.x, randomCoord.z] = false;//还原状态
                currentObsCount--;
            }
        }
    }

    public Coord GetRandomCoord()
    {
        Coord randomCoord = shuffledQueue.Dequeue();//队列：先进先出
        shuffledQueue.Enqueue(randomCoord);//将移出的元素放在队列的最后一个，保证队列完整性，大小不变

        return randomCoord;
    }

    //洪水填充算法DFS判断全联通
    private bool MapIsFullyAccessible(bool[,] _mapObstacles, int _currentObsCount)
    {
        bool[,] mapFlags = new bool[_mapObstacles.GetLength(0), _mapObstacles.GetLength(1)];//用以记录是否被检测

        Queue<Coord> queue = new Queue<Coord>();//所有可行走方块都放在这个队列里
        queue.Enqueue(playerPos);//将玩家位置作为起始点
        mapFlags[playerPos.x, playerPos.z] = true;//玩家的位置标记为已检测

        int accessibleCount = 1;//可行走的方块数量

        while (queue.Count > 0)//使用队列来循坏
        {
            Coord currentCube = queue.Dequeue();

            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    int neighborX = currentCube.x + x;
                    int neighborZ = currentCube.z + z;

                    if (x == 0 || z == 0)//选择上下左右四领域遍历
                    {
                        //边界判断
                        if (neighborX >= 0 && neighborX < _mapObstacles.GetLength(0)
                        && neighborZ >= 0 && neighborZ < _mapObstacles.GetLength(1))
                        {
                            //保证相邻点没有检测到，且此处没有障碍物
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
        int obsTargetCount = (int)(mapSize.x * mapSize.z - _currentObsCount);//当前可行走的

        return accessibleCount == obsTargetCount;
    }

    //随机洗牌算法
    public Coord[] ShuffleCoords(Coord[] dataArray)
    {
        for (int i = 0; i < dataArray.Length; i++)
        {
            int randomNum = Random.Range(i, dataArray.Length);

            //SWAP交换思路实现洗牌算法
            Coord temp = dataArray[randomNum];
            dataArray[randomNum] = dataArray[i];
            dataArray[i] = temp;
        }
        return dataArray;
    }

    //存储方块坐标的结构体
    public struct Coord
    {
        public int x;
        public int z;
        public Coord(int _x, int _z)
        {
            this.x = _x;
            this.z = _z;
        }
        //操作符==和！=重载
        public static bool operator !=(Coord _c1, Coord _c2)
        {
            return !(_c1 == _c2);
        }

        public static bool operator ==(Coord _c1, Coord _c2)
        {
            return (_c1.x == _c2.x) && (_c1.z == _c2.z);
        }
    }

    //方块升降
    //找出能够升降的方块
    public void findObstacle()
    {
        CubeRiseDownList.Clear();
        AlertList.Clear();
        foreach (GameObject cube in cubeList)    //遍历所有方块，随机取出作为障碍物
        {
            Coord cubeCoord = new Coord((int)cube.transform.position.x, (int)cube.transform.position.z);
            bool isobs = allobsCoord.Contains(cubeCoord);
            if (isobs == false)
            {
                int randomObstacleId = Random.Range(0, 100);
                if (randomObstacleId < 10)     //10%的概率方块上下移动
                {
                    CubeRiseDownList.Add(cube);
                    alert = cube.transform.GetChild(0);
                    AlertList.Add(alert);
                }
            }
        }
    }

    //障碍物升起前的预警
    public void Alert()
    {
        music.Play();            //预警的音效播放
        foreach (Transform alert in AlertList)
        {
            alert.GetComponent<ParticleSystem>().Play();
        }
    }

    //障碍物升起
    public void ascendObstacle()
    {
        foreach (GameObject cube in CubeRiseDownList)
        {
            Vector3 target = new Vector3(1 - outlinePercent, 2, 1 - outlinePercent);          //障碍物比例变化
            cube.transform.localScale = Vector3.SmoothDamp(cube.transform.localScale,     //平滑阻尼移动
            target, ref Velocity, smoothTime_ascend);
        }
    }

    //障碍物下降
    public void decendObstacle()
    {
        Vector3 origin = new Vector3(1 - outlinePercent, 1 - outlinePercent, 1 - outlinePercent);    //方块初始比例
        foreach (GameObject cube in CubeRiseDownList)
        {
            cube.transform.localScale = Vector3.MoveTowards(cube.transform.localScale, origin, smoothTime_decend); //平滑移动
        }
    }
}

