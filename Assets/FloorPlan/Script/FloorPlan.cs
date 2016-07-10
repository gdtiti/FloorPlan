using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class FloorPlan : MonoBehaviour {
    public GameObject[] furniture;              //家具の読み込み
    private GameObject[] onFurniture;           //置いた家具
    private GameObject[] tempFurniture;         //swap用の家具のゲームオブジェクト

    public Camera mainCamera;                   //メインカメラ(上からのカメラ)
    public Camera sceneViewCamera;              //カメラ２つ目(SceneViewカメラ)
    private Vector3 position;                   //マウスのスクリーン座標
    private Vector3 screenToWorldPointPosition; //マウスのワールド座標
    private GameObject sphere;                  //マウスポインタ
    private List<GameObject> cubeList;          //２つだけcube置くためのリスト
    private List<Vector3> cubePoint;            //cubeの座標のリスト
    public static bool floorFlag;               //最初にFloor設定するためのフラグ
    private List<Vector4> room;                 //レイアウトする部屋の両端の座標
    public Texture twall,tfloor;                //床と壁のテクスチャ
    private float mincost;                      //mincost
    
    void createFloor()
    {
        position = Input.mousePosition;
        position.z = mainCamera.GetComponent<Transform>().position.y;
        screenToWorldPointPosition = mainCamera.ScreenToWorldPoint(position);
        sphere.transform.position = screenToWorldPointPosition;
        if (Input.GetMouseButtonDown(0))
        {
            if (cubeList.Count == 2)
            {
                GameObject[] wall1 = new GameObject[4];
                GameObject[] bwall1 = new GameObject[4];
                GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Quad);
                //floor
                floor.GetComponent<Renderer>().material.color = new Color(255f/255f, 214f/255f, 180f/255f, 125f/255f);
                floor.transform.position = new Vector3((cubeList[0].transform.position.x + cubeList[1].transform.position.x) / 2.0f, 0.01f, (cubeList[0].transform.position.z + cubeList[1].transform.position.z) / 2.0f);
                floor.transform.eulerAngles = new Vector3(90f, 0f, 0f);
                floor.transform.localScale = new Vector3(Mathf.Abs(cubeList[0].transform.position.x - cubeList[1].transform.position.x), Mathf.Abs(cubeList[0].transform.position.z - cubeList[1].transform.position.z), 1f);
                floor.GetComponent<Renderer>().material.mainTexture = tfloor;
                for (int i = 0; i < 4; i++)
                {
                    wall1[i] = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    wall1[i].GetComponent<Renderer>().material.color = new Color(255f/255f, 255f/255f, 255f/255f);
                    wall1[i].GetComponent<Renderer>().material.mainTexture = twall;
                    bwall1[i] = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    bwall1[i].GetComponent<Renderer>().material.color = new Color(255f/255f, 255f/255f, 255f/255f);
                    bwall1[i].GetComponent<Renderer>().material.mainTexture = twall;                    
                }
                float margin = 0.01f;

                //top
                wall1[0].transform.position = new Vector3((cubeList[0].transform.position.x + cubeList[1].transform.position.x) / 2.0f,
                                                    0.5f, cubeList[0].transform.position.z - margin);
                bwall1[0].transform.position = wall1[0].transform.position;
                wall1[0].transform.eulerAngles = new Vector3(0, 0, 0);
                bwall1[0].transform.eulerAngles = new Vector3(0, 180f, 0);
                wall1[0].transform.localScale = new Vector3(Mathf.Abs(cubeList[0].transform.position.x - cubeList[1].transform.position.x), 2.5f, 1f);
                bwall1[0].transform.localScale = wall1[0].transform.localScale;

                //right
                wall1[1].transform.position = new Vector3(cubeList[1].transform.position.x-margin, 0.5f, 
                    (cubeList[0].transform.position.z+cubeList[1].transform.position.z)/2.0f);
                bwall1[1].transform.position = wall1[1].transform.position;
                wall1[1].transform.eulerAngles = new Vector3(0, 90f, 0);
                bwall1[1].transform.eulerAngles = new Vector3(0, 270f, 0);
                wall1[1].transform.localScale = new Vector3(Mathf.Abs(cubeList[0].transform.position.z - cubeList[1].transform.position.z), 2.5f, 1f);
                bwall1[1].transform.localScale = wall1[1].transform.localScale;

                //bottom
                wall1[2].transform.position = new Vector3((cubeList[0].transform.position.x + cubeList[1].transform.position.x) / 2.0f,
                                                    0.5f, cubeList[1].transform.position.z+margin);
                bwall1[2].transform.position = wall1[2].transform.position;
                wall1[2].transform.eulerAngles = new Vector3(0, 0, 0);
                bwall1[2].transform.eulerAngles = new Vector3(0, 180f, 0);
                wall1[2].transform.localScale = new Vector3(Mathf.Abs(cubeList[0].transform.position.x - cubeList[1].transform.position.x), 2.5f, 1f);
                bwall1[2].transform.localScale = wall1[2].transform.localScale;

                //left
                wall1[3].transform.position = new Vector3(cubeList[0].transform.position.x+margin, 0.5f,
                    (cubeList[0].transform.position.z+cubeList[1].transform.position.z)/2.0f);
                bwall1[3].transform.position = wall1[3].transform.position;
                wall1[3].transform.eulerAngles = new Vector3(0, 90f, 0);
                bwall1[3].transform.eulerAngles = new Vector3(0, 270f, 0);
                wall1[3].transform.localScale = new Vector3(Mathf.Abs(cubeList[0].transform.position.z - cubeList[1].transform.position.z), 2.5f, 1f);
                bwall1[3].transform.localScale = wall1[3].transform.localScale;

                cubePoint.Add(cubeList[0].transform.position);
                cubePoint.Add(cubeList[1].transform.position);

                for (int i = 0; i < 2; i++)
                {
                    Destroy(cubeList[i]);
                }
                cubeList.Clear();
            }
            else
            {
                GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Plane);
                cube1.transform.position = screenToWorldPointPosition;
                cube1.transform.position += new Vector3(0.0f, 0.1f, 0.0f);
                cube1.GetComponent<Renderer>().material.color = Color.yellow;
                cube1.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                cube1.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color");
                
                if(cubePoint.Count >= 2)
                {
                    float threshold = 0.3f;
                    for(int i = 0; i < cubePoint.Count; i++)
                    {
                        //Debug.Log(Mathf.Abs(cube1.transform.position.x - cubePoint[i].x));
                        if(Mathf.Abs(cube1.transform.position.x - cubePoint[i].x) < threshold)
                        {   
                            cube1.transform.position = new Vector3(cubePoint[i].x,cube1.transform.position.y,cube1.transform.position.z);
                        }
                        if(Mathf.Abs(cube1.transform.position.z - cubePoint[i].z) < threshold)
                        {
                            cube1.transform.position = new Vector3(cube1.transform.position.x, cube1.transform.position.y, cubePoint[i].z);
                        }
                    }
                }
                cubeList.Add(cube1);
            }
            
        }
    }

    void setLightPosition()
    {
        position = Input.mousePosition;
        position.z = mainCamera.GetComponent<Transform>().position.y;
        screenToWorldPointPosition = mainCamera.ScreenToWorldPoint(position);
        //sphere.transform.position = screenToWorldPointPosition;
        if (Input.GetMouseButtonDown(0))
        {
            GameObject lighting = new GameObject("Lighting Equipment");
            Light lightComp = lighting.AddComponent<Light>();
            lightComp.type = LightType.Point;
            lightComp.color = Color.white;
            lighting.transform.position = new Vector3(screenToWorldPointPosition.x, 1.6f, screenToWorldPointPosition.z);
        }
    }

    private bool once = false;
    void initFurniture()
    {
        position = Input.mousePosition;
        position.z = mainCamera.GetComponent<Transform>().position.y;
        screenToWorldPointPosition = mainCamera.ScreenToWorldPoint(position);
        //sphere.transform.position = screenToWorldPointPosition;

        if (!once)
        {

            //float jou = 1.824f;//１畳の大きさ
            float jou = 2.1f;
            for (int i = 0; i < cubePoint.Count; i += 2)
            {
                //Debug.Log(cubePoint[i]);
                //部屋の大きさの最低は４畳半以上
                if(Mathf.Abs(cubePoint[i].x - cubePoint[i + 1].x) * Mathf.Abs(cubePoint[i].z - cubePoint[i + 1].z) > 4.5 * jou)
                {
                    //room.Add(new Vector4(cubePoint[i].x, cubePoint[i].z, cubePoint[i + 1].x, cubePoint[i + 1].z));
                    if (cubePoint[i].x < cubePoint[i + 1].x)
                    {
                        room.Add(new Vector4(cubePoint[i].x, cubePoint[i].z, cubePoint[i + 1].x, cubePoint[i + 1].z));
                    }
                    else
                    {
                        room.Add(new Vector4(cubePoint[i + 1].x, cubePoint[i + 1].z, cubePoint[i].x, cubePoint[i].z));
                    }
                }
            }
            //Debug.Log(room.Count);
            //Debug.Log("-----------");
            for(int i = 0; i < furniture.Length; i++)
            {
                initoneFurniturePosition(i);
            }
            roomTextSave(ref room);
            once = true;
        }
    }

    void initoneFurniturePosition(int i)
    {
        Collider[] col;
        if (onFurniture.Length != 0 && onFurniture[i] != null) Destroy(onFurniture[i]);
        onFurniture[i] = Instantiate(this.furniture[i]);
        do
        {
            int roomID = Random.Range(0, room.Count);
            onFurniture[i].transform.position =
             new Vector3(Random.Range(room[roomID].x, room[roomID].z), 0.02f, Random.Range(room[roomID].y, room[roomID].w));
            onFurniture[i].transform.rotation =
             Quaternion.Euler(0.0f, Random.Range(-180.0f, 180f), 0.0f);
            BoxCollider box = onFurniture[i].GetComponent<BoxCollider>();
            col = Physics.OverlapBox(box.bounds.center, box.bounds.size / 2.0f, onFurniture[i].transform.rotation);
        } while (col.Length > 1);
    }

    void MetropolisHastings()
    {
        float p0, cost, p, alpha, acost;

        cost = costFunction();
        p0 = densityFunction(cost);

        int move = Random.Range(0, 3);
        //GaussianMove
        if (move == 0)
        {
            int fid = Random.Range(0, furniture.Length);
            //家具がどの部屋にあるのか調べるルーチン
            int rid = -9999;
            for (int i = 0; i < room.Count; i++)
            {
                if (onFurniture[fid].transform.position.x > room[i].x && onFurniture[fid].transform.position.x < room[i].z)
                {
                    if (onFurniture[fid].transform.position.z < room[i].y && onFurniture[fid].transform.position.z > room[i].w)
                    {
                        rid = i;
                    }
                    else if (onFurniture[fid].transform.position.z > room[i].y && onFurniture[fid].transform.position.z < room[i].w)
                    {
                        rid = i;
                    }
                }
                if (rid != -9999)
                {
                    break;
                }
            }
            Debug.Log("onFurniture[" + fid.ToString() + "]:room[" + rid.ToString() + "]");

            Collider[] col1; Vector3 diff;
            float mu = 0.0f, sigma = 0.1f;
            int limit = 0;
            tempFurniture = onFurniture;
            do
            {
                diff = new Vector3(rand_normal(mu, sigma), 0.0f, rand_normal(mu, sigma));
                if (limit++ > 100)
                {
                    diff = new Vector3(0.0f, 0.0f, 0.0f);
                    break;
                }
                tempFurniture[fid].transform.position += diff;
                BoxCollider box = tempFurniture[fid].GetComponent<BoxCollider>();
                col1 = Physics.OverlapBox(box.bounds.center, box.bounds.size / 2.0f, onFurniture[fid].transform.rotation);
                tempFurniture[fid].transform.position -= diff;
            } while (col1.Length > 1);
            onFurniture[fid].transform.position += diff;
        }
        //RotationGaussian
        else if (move == 1)
        {
            int fid = Random.Range(0, furniture.Length);
            Collider[] col1; Vector3 rdiff;
            float mu = 0.0f, sigma = 10.0f;
            int limit = 0;
            tempFurniture = onFurniture;
            do
            {
                rdiff = new Vector3(0.0f, rand_normal(mu, sigma), 0.0f);
                if (limit++ > 100)
                {
                    rdiff = new Vector3(0.0f, 0.0f, 0.0f);
                    break;
                }
                tempFurniture[fid].transform.eulerAngles += rdiff;
                BoxCollider box = tempFurniture[fid].GetComponent<BoxCollider>();
                col1 = Physics.OverlapBox(box.bounds.center, box.bounds.size / 2.0f, onFurniture[fid].transform.rotation);
                tempFurniture[fid].transform.eulerAngles -= rdiff;
            } while (col1.Length > 1);
            onFurniture[fid].transform.eulerAngles += rdiff;
        }
        //RandomSwap(部屋間の移動も含める)
        else if (move == 2)
        {
            Vector3 ttemp;
            Collider[] col1, col2;
            int onefID, twofID, limit = 0;
            tempFurniture = onFurniture;
            do
            {
                if (limit++ > 100)
                {
                    onefID = twofID = 0;
                    break;
                }
                onefID = Random.Range(0, furniture.Length);
                twofID = onefID;
                while (twofID == onefID)
                {
                    twofID = Random.Range(0, furniture.Length);
                }
                ttemp = tempFurniture[onefID].transform.position;
                tempFurniture[onefID].transform.position = tempFurniture[twofID].transform.position;
                tempFurniture[twofID].transform.position = ttemp;
                BoxCollider box1 = tempFurniture[onefID].GetComponent<BoxCollider>();
                BoxCollider box2 = tempFurniture[twofID].GetComponent<BoxCollider>();
                col1 = Physics.OverlapBox(box1.bounds.center, box1.bounds.size / 2.0f, tempFurniture[onefID].transform.rotation);
                col2 = Physics.OverlapBox(box2.bounds.center, box2.bounds.size / 2.0f, tempFurniture[twofID].transform.rotation);
                tempFurniture = onFurniture;
            } while (col1.Length > 1 || col2.Length>1);
            ttemp = onFurniture[onefID].transform.position;
            onFurniture[onefID].transform.position = onFurniture[twofID].transform.position;
            onFurniture[twofID].transform.position = ttemp;
        }
    }

    float rand_normal(float mu, float sigma)
    {
        float z = Mathf.Sqrt(-2.0f*Mathf.Log(Random.Range(0.0f,1.0f)))*Mathf.Sin(2.0f*Mathf.PI*Random.Range(0.0f,1.0f));
        return mu + sigma * z;
    }


    float costFunction()
    {
        float cost;

        float mpd = 0, mpa = 0, wpd = 6.5f, wpa = 4.0f;     //Pairwise Relationship
        float mcd = 0, mca = 0, wcd = 4.5f, wca = 3.0f;     //Conversation
        float mvb = 0, wvb = 3.0f;                          //Balance 15
        float mfa = 0, mwa = 0, wfa = 3.5f, wwa = 3.5f;     //Alignment 2.5 2.5

        cost = Random.value;
        return cost;
    }

    float densityFunction(float cost)
    {
        float beta = 0.85f;
        return Mathf.Exp(-beta * cost);
    }

    float t(float d, float m, float M, int a = 2)
    {
        float result = 1.0f;
        if(d< m) { for(int i=0;i< a; i++) { result *= (d / m); } }
        else if(d>=m && d<= M) { result = 1.0f; }
        else if(d> M) { for(int i=0;i< a; i++) { result *= (M / d); } }
        return result;
    }


	void Start () {
        //平行投影(true)透視投影(false)
        mainCamera.orthographic = true;

        //先にMainCameraから利用する
        mainCamera.enabled = true;
        sceneViewCamera.enabled = false;

        Random.seed = 0;
        //floorPlanの設定用のflag
        floorFlag = true;
        //マウスポインタの赤い球
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = new Vector3(0, 4, 0);
        sphere.GetComponent<Renderer>().material.color = Color.red;
        sphere.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color");
        sphere.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        cubeList = new List<GameObject>();
        cubePoint = new List<Vector3>();
        room = new List<Vector4>();
        onFurniture = new GameObject[furniture.Length];
        tempFurniture = new GameObject[furniture.Length];

        mincost = 9999;
    }
	
	void Update () {
        position = Input.mousePosition;
        position.z = mainCamera.GetComponent<Transform>().position.y;
        screenToWorldPointPosition = mainCamera.ScreenToWorldPoint(position);
        sphere.transform.position = screenToWorldPointPosition;

        //間取り設定
        if (floorFlag)
        {
            createFloor();
        }

        //PointLightを配置
        if (guiSceneScript.lightFlag)
        {
            if(cubeList.Count != 0)
            {
                for(int i = 0; i < cubeList.Count; i++)
                {
                    Destroy(cubeList[i]);
                }
                cubeList.Clear();
            }
            mainCamera.orthographic = false;
            setLightPosition();
        }

        //家具を初期位置に配置
        if (guiSceneScript.furnitureFlag)
        {
            if (cubeList.Count != 0)
            {
                for (int i = 0; i < cubeList.Count; i++)
                {
                    Destroy(cubeList[i]);
                }
                cubeList.Clear();
            }
            mainCamera.orthographic = false;
            initFurniture();
        }

        //提案レイアウト(MetropolisHastings)
        if (guiSceneScript.suggestion || Input.GetKey(KeyCode.Q))
        {
            mainCamera.orthographic = false;
            //Debug.Log("suggestion");
            int loopCount = 1;

            for(int i = 0; i < loopCount; i++)
            {
                MetropolisHastings();
            }
            Debug.Log(loopCount.ToString() + "回");
            Debug.Log(mincost);
            guiSceneScript.suggestion = false;
        }

        //カメラを変える
        if(guiSceneScript.cameraCount % 2 == 0)
        {
            mainCamera.enabled = true;
            sceneViewCamera.enabled = false;
        }else if(guiSceneScript.cameraCount % 2 == 1)
        {
            mainCamera.enabled = false;
            sceneViewCamera.enabled = true;
            if (cubeList.Count != 0)
            {
                for (int i = 0; i < cubeList.Count; i++)
                {
                    Destroy(cubeList[i]);
                }
                cubeList.Clear();
            }
        }
    }

    public void roomTextSave(ref List<Vector4> room)
    {
        StreamWriter sw = new StreamWriter("Z:/Furniture Project/Unity/FloorPlan/Assets/FloorPlan/RoomData.txt", false);
        for(int i = 0; i < room.Count; i++)
        {
            sw.WriteLine("room[" + i.ToString() + "]:" + room[i].ToString());
        }
        sw.Flush();
        sw.Close();
    }
    
}
