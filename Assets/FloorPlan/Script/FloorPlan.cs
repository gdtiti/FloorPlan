using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class FloorPlan : MonoBehaviour {
    public GameObject[] furniture;              //家具の読み込み
    private GameObject[] onFurniture;           //置いた家具
    private GameObject[] tempFurniture;         //swap用の家具のゲームオブジェクト
    private GameObject[] beforeFurniture;       //遷移前の置いた家具の位置を保存
    private Vector3[] mintrans, minrotation;    //mincostのときの値を保存

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

    private int floorFixing = 0;


    void createFloor()
    {
        position = Input.mousePosition;
        position.z = mainCamera.GetComponent<Transform>().position.y;
        screenToWorldPointPosition = mainCamera.ScreenToWorldPoint(position);
        sphere.transform.position = screenToWorldPointPosition;
        float wheight = 4.3f;
        if (Input.GetMouseButtonDown(0))
        {
            if (cubeList.Count == 2)
            {
                GameObject[] wall1 = new GameObject[4];
                GameObject[] bwall1 = new GameObject[4];
                GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Quad);
                GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Quad);
                //floor
                floor.GetComponent<Renderer>().material.color = new Color(255f/255f, 214f/255f, 180f/255f, 125f/255f);
                floor.transform.position = new Vector3((cubeList[0].transform.position.x + cubeList[1].transform.position.x) / 2.0f, 0.01f, (cubeList[0].transform.position.z + cubeList[1].transform.position.z) / 2.0f);
                floor.transform.eulerAngles = new Vector3(90f, 0f, 0f);
                floor.transform.localScale = new Vector3(Mathf.Abs(cubeList[0].transform.position.x - cubeList[1].transform.position.x), Mathf.Abs(cubeList[0].transform.position.z - cubeList[1].transform.position.z), 1f);
                floor.GetComponent<Renderer>().material.mainTexture = tfloor;
                //ceiling
                ceiling.GetComponent<Renderer>().material.color = new Color(255f/255f, 255f/255f, 255f/255f);
                ceiling.transform.position = floor.transform.position;
                ceiling.transform.position += new Vector3(0.0f, 2.5f, 0.0f);
                ceiling.transform.eulerAngles = new Vector3(270f, 0f, 0f);
                ceiling.transform.localScale = floor.transform.localScale;
                ceiling.GetComponent<Renderer>().material.mainTexture = twall;

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
                wall1[0].transform.localScale = new Vector3(Mathf.Abs(cubeList[0].transform.position.x - cubeList[1].transform.position.x), wheight, 1f);
                bwall1[0].transform.localScale = wall1[0].transform.localScale;

                //right
                wall1[1].transform.position = new Vector3(cubeList[1].transform.position.x-margin, 0.5f, 
                    (cubeList[0].transform.position.z+cubeList[1].transform.position.z)/2.0f);
                bwall1[1].transform.position = wall1[1].transform.position;
                wall1[1].transform.eulerAngles = new Vector3(0, 90f, 0);
                bwall1[1].transform.eulerAngles = new Vector3(0, 270f, 0);
                wall1[1].transform.localScale = new Vector3(Mathf.Abs(cubeList[0].transform.position.z - cubeList[1].transform.position.z), wheight, 1f);
                bwall1[1].transform.localScale = wall1[1].transform.localScale;

                //bottom
                wall1[2].transform.position = new Vector3((cubeList[0].transform.position.x + cubeList[1].transform.position.x) / 2.0f,
                                                    0.5f, cubeList[1].transform.position.z+margin);
                bwall1[2].transform.position = wall1[2].transform.position;
                wall1[2].transform.eulerAngles = new Vector3(0, 0, 0);
                bwall1[2].transform.eulerAngles = new Vector3(0, 180f, 0);
                wall1[2].transform.localScale = new Vector3(Mathf.Abs(cubeList[0].transform.position.x - cubeList[1].transform.position.x), wheight, 1f);
                bwall1[2].transform.localScale = wall1[2].transform.localScale;

                //left
                wall1[3].transform.position = new Vector3(cubeList[0].transform.position.x+margin, 0.5f,
                    (cubeList[0].transform.position.z+cubeList[1].transform.position.z)/2.0f);
                bwall1[3].transform.position = wall1[3].transform.position;
                wall1[3].transform.eulerAngles = new Vector3(0, 90f, 0);
                bwall1[3].transform.eulerAngles = new Vector3(0, 270f, 0);
                wall1[3].transform.localScale = new Vector3(Mathf.Abs(cubeList[0].transform.position.z - cubeList[1].transform.position.z), wheight, 1f);
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

                ////------------------------------------------------------------------------------------------------------
                ////結果を固定することにする
                ////room[0]=(-0.9,3.3,3.2,-2.9),room[1]=(-5.1,5.4,-0.9,1.3),room[2]=(-4.1,1.3,-0.9,-2.9)
                if (floorFixing == 0) { cube1.transform.position = new Vector3(-1.0f, 0.1f, 3.5f); floorFixing++; }
                else if (floorFixing == 1) { cube1.transform.position = new Vector3(3.4f, 0.1f, -3.1f); floorFixing++; }
                else if (floorFixing == 2) { cube1.transform.position = new Vector3(-5.3f, 0.1f, 5.7f); floorFixing++; }
                else if (floorFixing == 3) { cube1.transform.position = new Vector3(-1.0f, 0.1f, 1.3f); floorFixing++; }
                else if (floorFixing == 4) { cube1.transform.position = new Vector3(-4.2f, 0.1f, 1.3f); floorFixing++; }
                else if (floorFixing == 5) { cube1.transform.position = new Vector3(-1.0f, 0.1f, -3.1f); floorFixing++; }
                ////------------------------------------------------------------------------------------------------------
                if (cubePoint.Count >= 2)
                    {
                        float threshold = 0.3f;
                        for (int i = 0; i < cubePoint.Count; i++)
                        {
                            //Debug.Log(Mathf.Abs(cube1.transform.position.x - cubePoint[i].x));
                            if (Mathf.Abs(cube1.transform.position.x - cubePoint[i].x) < threshold)
                            {
                                cube1.transform.position = new Vector3(cubePoint[i].x, cube1.transform.position.y, cube1.transform.position.z);
                            }
                            if (Mathf.Abs(cube1.transform.position.z - cubePoint[i].z) < threshold)
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
            lighting.transform.position = new Vector3(screenToWorldPointPosition.x, 2.3f, screenToWorldPointPosition.z);
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
            onFurniture[i].transform.eulerAngles =
             new Vector3(0.0f, Random.Range(-180.0f, 180f), 0.0f);
            //int angle = Random.Range(0,4);
            //onFurniture[i].transform.eulerAngles =
            //    new Vector3(0.0f, 90.0f * angle, 0.0f);
            BoxCollider box = onFurniture[i].GetComponent<BoxCollider>();
            col = Physics.OverlapBox(box.bounds.center, box.bounds.size / 2.0f, onFurniture[i].transform.rotation);
        } while (col.Length > 1);
    }

    void MetropolisHastings()
    {
        //前の状態を保存
        beforeFurniture = onFurniture;

        float p0, bcost, p, alpha, acost;

        bcost = costFunction();
        p0 = densityFunction(bcost);

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
                //int angle = Random.Range(0, 4);
                //rdiff = new Vector3(0.0f, 90.0f * angle, 0.0f);
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

        acost = costFunction();
        p = densityFunction(acost);
        alpha = p / p0;
        if (alpha > 1) alpha = 1.0f;
        float t = Random.Range(0.0f, 1.0f);

        //状態を遷移しない
        if (alpha <= t)
        {
            //前の状態に戻す．
            onFurniture = beforeFurniture;
        }
        //状態を遷移する
        else
        {
            //すでに状態を遷移しているから何もしない
        }

        //mincostが更新されたときちゃんと変更しないといけないよね
        if(acost < bcost)
        {
            bcost = acost;
        }
        if(mincost > bcost)
        {
            mincost = bcost;

            for (int i = 0; i < furniture.Length; i++)
            {
                mintrans[i] = new Vector3(onFurniture[i].transform.position.x, onFurniture[i].transform.position.y, onFurniture[i].transform.position.z);
                minrotation[i] = new Vector3(onFurniture[i].transform.eulerAngles.x, onFurniture[i].transform.eulerAngles.y, onFurniture[i].transform.eulerAngles.z);
            }
        }

    }

    void MetropolisHastings_v2(int roomNo)
    {
        //前の状態を保存
        beforeFurniture = onFurniture;

        float p0, bcost, p, alpha, acost;

        bcost = costFunction_v2(roomNo);
        p0 = densityFunction(bcost);

        List<int> inroom;
        inroom = new List<int>();

        for(int i = 0; i < furniture.Length; i++)
        {
           if (onFurniture[i].transform.position.x > room[roomNo].x && onFurniture[i].transform.position.x < room[roomNo].z)
           {
              if(onFurniture[i].transform.position.z < room[roomNo].y && onFurniture[i].transform.position.z > room[roomNo].w)
              {
                  inroom.Add(i);
              }else if(onFurniture[i].transform.position.z > room[roomNo].y && onFurniture[i].transform.position.z < room[roomNo].w)
              {
                  inroom.Add(i);
              }
          }
        }


        int move = Random.Range(0, 3);
        //GaussianMove
        if (move == 0)
        {
            int fid = inroom[Random.Range(0, inroom.Count)];

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
            int fid = inroom[Random.Range(0, inroom.Count)];
            Collider[] col1; Vector3 rdiff;
            float mu = 0.0f, sigma = 10.0f;
            int limit = 0;
            tempFurniture = onFurniture;
            do
            {
                rdiff = new Vector3(0.0f, rand_normal(mu, sigma), 0.0f);
                //int angle = Random.Range(0, 4);
                //rdiff = new Vector3(0.0f, 90.0f * angle, 0.0f);
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
        //RandomSwap(部屋内のスワップ)
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
                onefID = inroom[Random.Range(0, inroom.Count)];
                twofID = onefID;
                while (twofID == onefID)
                {
                    twofID = inroom[Random.Range(0, inroom.Count)];
                }
                ttemp = tempFurniture[onefID].transform.position;
                tempFurniture[onefID].transform.position = tempFurniture[twofID].transform.position;
                tempFurniture[twofID].transform.position = ttemp;
                BoxCollider box1 = tempFurniture[onefID].GetComponent<BoxCollider>();
                BoxCollider box2 = tempFurniture[twofID].GetComponent<BoxCollider>();
                col1 = Physics.OverlapBox(box1.bounds.center, box1.bounds.size / 2.0f, tempFurniture[onefID].transform.rotation);
                col2 = Physics.OverlapBox(box2.bounds.center, box2.bounds.size / 2.0f, tempFurniture[twofID].transform.rotation);
                tempFurniture = onFurniture;
            } while (col1.Length > 1 || col2.Length > 1);
            ttemp = onFurniture[onefID].transform.position;
            onFurniture[onefID].transform.position = onFurniture[twofID].transform.position;
            onFurniture[twofID].transform.position = ttemp;
        }

        acost = costFunction_v2(roomNo);
        p = densityFunction(acost);
        alpha = p / p0;
        if (alpha > 1) alpha = 1.0f;
        float t = Random.Range(0.0f, 1.0f);

        //状態を遷移しない
        if (alpha <= t)
        {
            //前の状態に戻す．
            onFurniture = beforeFurniture;
        }
        //状態を遷移する
        else
        {
            //すでに状態を遷移しているから何もしない
        }

        //mincostが更新されたときちゃんと変更しないといけないよね
        if (acost < bcost)
        {
            bcost = acost;
        }
        if (mincost > bcost)
        {
            mincost = bcost;

            for (int i = 0; i < furniture.Length; i++)
            {
                mintrans[i] = new Vector3(onFurniture[i].transform.position.x, onFurniture[i].transform.position.y, onFurniture[i].transform.position.z);
                minrotation[i] = new Vector3(onFurniture[i].transform.eulerAngles.x, onFurniture[i].transform.eulerAngles.y, onFurniture[i].transform.eulerAngles.z);
            }
        }

    }


    float rand_normal(float mu, float sigma)
    {
        float z = Mathf.Sqrt(-2.0f*Mathf.Log(Random.Range(0.0f,1.0f)))*Mathf.Sin(2.0f*Mathf.PI*Random.Range(0.0f,1.0f));
        return mu + sigma * z;
    }

    //家具のペア同士の評価(部屋別)
    void calcPairRelation(ref float mpd, ref float mpa, int roomID)
    {
        float p = 0, distance = 0, mind = 0, maxd = 0, cosfg = 0;
        mpd = 0; mpa = 0;
        List<int> inroom;
        inroom = new List<int>();

        for(int i = 0; i < furniture.Length; i++)
        {
            if (onFurniture[i].transform.position.x > room[roomID].x && onFurniture[i].transform.position.x < room[roomID].z)
            {
                if(onFurniture[i].transform.position.z < room[roomID].y && onFurniture[i].transform.position.z > room[roomID].w)
                {
                    inroom.Add(i);
                }else if(onFurniture[i].transform.position.z > room[roomID].y && onFurniture[i].transform.position.z < room[roomID].w)
                {
                    inroom.Add(i);
                }
            }
        }

        for(int i = 0; i < inroom.Count; i++)
        {
            for(int j = i + 1; j < inroom.Count; j++)
            {
                Vector3 d = onFurniture[inroom[i]].transform.position - onFurniture[inroom[j]].transform.position;
                distance = d.magnitude;

                if((onFurniture[inroom[i]].gameObject.tag == "Armchair" && onFurniture[inroom[j]].gameObject.tag == "CoffeeTable")
                    ||(onFurniture[inroom[i]].gameObject.tag == "CoffeeTable" && onFurniture[inroom[j]].gameObject.tag == "Armchair"))
                {
                    p = 1.0f; mind = 1.1f; maxd = 1.6f;
                }else if((onFurniture[inroom[i]].gameObject.tag == "Armchair" && onFurniture[inroom[j]].gameObject.tag == "Sofa")
                    ||(onFurniture[inroom[i]].gameObject.tag == "Sofa" && onFurniture[inroom[j]].gameObject.tag == "Armchair"))
                {
                    p = 1.0f; mind = 1.6f; maxd = 2.3f;
                }else if((onFurniture[inroom[i]].gameObject.tag == "CoffeeTable" && onFurniture[inroom[j]].gameObject.tag == "Sofa")
                    ||(onFurniture[inroom[i]].gameObject.tag == "Sofa" && onFurniture[inroom[j]].gameObject.tag == "CoffeeTable"))
                {
                    p = 1.0f; mind = 1.1f; maxd = 1.9f;
                }else if(onFurniture[inroom[i]].gameObject.tag == "Armchair" && onFurniture[inroom[j]].gameObject.tag == "Armchair")
                {
                    p = 1.0f; mind = 1.7f; maxd = 2.3f;
                }else if((onFurniture[inroom[i]].gameObject.tag == "Armchair" && onFurniture[inroom[j]].gameObject.tag == "TV")
                    || (onFurniture[inroom[i]].gameObject.tag == "TV" && onFurniture[inroom[j]].gameObject.tag == "Armchair"))
                {
                    p = 1.0f; mind = 1.6f; maxd = 2.3f;
                }else if((onFurniture[inroom[i]].gameObject.tag == "CoffeeTable" && onFurniture[inroom[j]].gameObject.tag == "TV")
                    || (onFurniture[inroom[i]].gameObject.tag == "TV" && onFurniture[inroom[j]].gameObject.tag == "CoffeeTable"))
                {
                    p = 1.0f; mind = 1.1f; maxd = 1.9f;
                }else if((onFurniture[inroom[i]].gameObject.tag == "Sofa" && onFurniture[inroom[j]].gameObject.tag == "TV")
                    || (onFurniture[inroom[i]].gameObject.tag == "TV" && onFurniture[inroom[j]].gameObject.tag == "Sofa"))
                {
                    p = 1.0f; mind = 2.2f; maxd = 3.2f;
                }else if(onFurniture[inroom[i]].gameObject.tag == "CoffeeTable" && onFurniture[inroom[j]].gameObject.tag == "CoffeeTable")
                {
                    p = 1.0f; mind = 2.4f; maxd = 3.2f;
                }else if((onFurniture[inroom[i]].gameObject.tag == "DiningTable" && onFurniture[inroom[j]].gameObject.tag == "DiningChair")
                    || (onFurniture[inroom[i]].gameObject.tag == "DiningChair" && onFurniture[inroom[j]].gameObject.tag == "DiningTable"))
                {
                    p = 1.0f; mind = 1.2f; maxd = 1.8f;
                }
                else
                {
                    p = 0.0f; mind = 0.0f; maxd = 0.0f;
                }
                mpd -= p * t(distance, mind, maxd, 2);
            }
        }

    }

    //会話をするための評価関数(部屋別)
    void calcConversation(ref float mcd, ref float mca, int roomID)
    {
        float q = 0.0f, cosfg = 0.0f, cosgf = 0.0f, distance = 0.0f, maxd = 0.0f, mind = 0.0f;
        mcd = 0.0f; mca = 0.0f;
        List<int> inroom;
        inroom = new List<int>();

        for(int i = 0; i < furniture.Length; i++)
        {
            if (onFurniture[i].transform.position.x > room[roomID].x && onFurniture[i].transform.position.x < room[roomID].z)
            {
                if(onFurniture[i].transform.position.z < room[roomID].y && onFurniture[i].transform.position.z > room[roomID].w)
                {
                    inroom.Add(i);
                }else if(onFurniture[i].transform.position.z > room[roomID].y && onFurniture[i].transform.position.z < room[roomID].w)
                {
                    inroom.Add(i);
                }
            }
        }

        for (int i = 0; i < inroom.Count; i++)
        {
            for (int j = i + 1; j < inroom.Count; j++)
            {
                Vector3 d = onFurniture[inroom[j]].transform.position - onFurniture[inroom[i]].transform.position;
                distance = d.magnitude;
                d.x /= distance; d.y = 0.0f; d.z /= distance;
                float thetaf = onFurniture[inroom[i]].transform.eulerAngles.y;
                float thetag = onFurniture[inroom[j]].transform.eulerAngles.y;

                Vector3 f = new Vector3(Mathf.Sin(thetaf * Mathf.PI / 180.0f), 0.0f, Mathf.Cos(thetaf * Mathf.PI / 180.0f));
                Vector3 g = new Vector3(Mathf.Sin(thetag * Mathf.PI / 180.0f), 0.0f, Mathf.Cos(thetag * Mathf.PI / 180.0f));

                if ((onFurniture[inroom[i]].gameObject.tag == "Armchair" && onFurniture[inroom[j]].gameObject.tag == "Sofa")
                    || (onFurniture[inroom[i]].gameObject.tag == "Sofa" && onFurniture[inroom[j]].gameObject.tag == "Armchair"))
                {
                    q = 1.0f; mind = 1.7f; maxd = 2.3f;
                    cosfg = Vector3.Dot(f, d); cosgf = Vector3.Dot(g, -d);
                }
                else if (onFurniture[inroom[i]].gameObject.tag == "Armchair" && onFurniture[inroom[j]].gameObject.tag == "Armchair")
                {
                    q = 1.0f; mind = 2.4f; maxd = 3.2f;
                    cosfg = Vector3.Dot(f, d); cosgf = Vector3.Dot(g, -d);
                }
                else if ((onFurniture[inroom[i]].gameObject.tag == "Armchair" && onFurniture[inroom[j]].gameObject.tag == "TV")
                   || (onFurniture[inroom[i]].gameObject.tag == "TV" && onFurniture[inroom[j]].gameObject.tag == "Armchair"))
                {
                    q = 1.0f; mind = 1.7f; maxd = 2.3f;
                    cosfg = Vector3.Dot(f, d); cosgf = Vector3.Dot(g, -d);
                }
                else if ((onFurniture[inroom[i]].gameObject.tag == "Sofa" && onFurniture[inroom[j]].gameObject.tag == "TV")
                   || (onFurniture[inroom[i]].gameObject.tag == "TV" && onFurniture[inroom[j]].gameObject.tag == "Sofa"))
                {
                    q = 1.0f; mind = 2.4f; maxd = 3.2f;
                    cosfg = Vector3.Dot(f, d); cosgf = Vector3.Dot(g, -d);
                }else if((onFurniture[inroom[i]].gameObject.tag == "CoffeeTable" && onFurniture[inroom[j]].gameObject.tag == "Armchair")
                    || (onFurniture[inroom[i]].gameObject.tag == "Armchair" && onFurniture[inroom[j]].gameObject.tag == "CoffeeTable"))
                {
                    q = 1.0f; mind = 1.1f; maxd = 1.6f;
                    cosfg = Vector3.Dot(f, d); cosgf = Vector3.Dot(g, -d);
                }
                else if (onFurniture[inroom[i]].gameObject.tag == "DiningChair" && onFurniture[inroom[j]].gameObject.tag == "DiningChair")
                {
                    q = 1.0f; mind = 2.4f; maxd = 3.6f;
                    cosfg = Vector3.Dot(f, d); cosgf = Vector3.Dot(g, -d);
                }else if((onFurniture[inroom[i]].gameObject.tag == "DiningChair" && onFurniture[inroom[j]].gameObject.tag == "TV")
                    ||(onFurniture[inroom[i]].gameObject.tag == "TV" && onFurniture[inroom[j]].gameObject.tag == "DiningChair"))
                {
                    q = 1.0f; mind = 1.7f;maxd = 2.3f;
                    cosfg = Vector3.Dot(f, d); cosgf = Vector3.Dot(g, -d);
                }else if((onFurniture[inroom[i]].gameObject.tag == "DiningTable" && onFurniture[inroom[j]].gameObject.tag == "DiningChair")
                    || (onFurniture[inroom[i]].gameObject.tag == "DiningChair" && onFurniture[inroom[j]].gameObject.tag == "DiningTable"))
                {
                    q = 1.0f; mind = 1.8f; maxd = 2.3f;
                    cosfg = Vector3.Dot(f, d); cosgf = Vector3.Dot(g, -d);
                }
                else
                {
                    q = 0.0f; mind = 0.0f; maxd = 0.0f; cosfg = 0.0f; cosgf = 0.0f;
                }
                mcd -= q * t(distance, mind, maxd, 2);
                mca -= q * (cosfg + 1) * (cosgf + 1);
            }
        }

    }

    //整列させる．mfaのグループごとまだできてない．mwaもまだできてない．
    void calcAlignment(ref float mfa, ref float mwa, int roomID)
    {
        mfa = 0.0f; mwa = 0.0f;

        List<int> inroom;
        inroom = new List<int>();

        for(int i = 0; i < furniture.Length; i++)
        {
            if (onFurniture[i].transform.position.x > room[roomID].x && onFurniture[i].transform.position.x < room[roomID].z)
            {
                if(onFurniture[i].transform.position.z < room[roomID].y && onFurniture[i].transform.position.z > room[roomID].w)
                {
                    inroom.Add(i);
                }else if(onFurniture[i].transform.position.z > room[roomID].y && onFurniture[i].transform.position.z < room[roomID].w)
                {
                    inroom.Add(i);
                }
            }
        }

        //mfaの計算
        for (int i = 0; i < inroom.Count; i++)
        {
            for(int j = i + 1; j < inroom.Count; j++)
            {
                mfa -= Mathf.Cos(4 * (-onFurniture[inroom[i]].transform.eulerAngles.y + onFurniture[inroom[j]].transform.eulerAngles.y) * Mathf.PI / 180.0f);
            }
        }

        //mwaの計算まだちゃんとできてない．．．
        for (int i = 0; i < inroom.Count; i++)
        {
            mwa -= Mathf.Cos(4 * (onFurniture[inroom[i]].transform.eulerAngles.y));
        }

    }

    //重心のバランスの計算
    void calcVisualBalance(ref float mvb, int roomID)
    {
        float sumArea = 0.0f, oneArea;
        mvb = 0.0f;
        Vector3 centroid, roomCenter;
        List<int> inroom;
        inroom = new List<int>();

        for(int i = 0; i < furniture.Length; i++)
        {
            if (onFurniture[i].transform.position.x > room[roomID].x && onFurniture[i].transform.position.x < room[roomID].z)
            {
                if(onFurniture[i].transform.position.z < room[roomID].y && onFurniture[i].transform.position.z > room[roomID].w)
                {
                    inroom.Add(i);
                }else if(onFurniture[i].transform.position.z > room[roomID].y && onFurniture[i].transform.position.z < room[roomID].w)
                {
                    inroom.Add(i);
                }
            }
        }





    }

    float costFunction_v2(int roomNo)
    {
        float cost = 0;

        float mpd = 0, mpa = 0, wpd = 6.5f, wpa = 4.0f;     //Pairwise Relationship
        float mcd = 0, mca = 0, wcd = 4.5f, wca = 3.0f;     //Conversation
        float mvb = 0, wvb = 3.0f;                          //Balance 15
        float mfa = 0, mwa = 0, wfa = 3.5f, wwa = 3.5f;     //Alignment 2.5 2.5

       
        mpd = 0.0f; mpa = 0.0f; mcd = 0.0f; mca = 0.0f; mvb = 0.0f; mfa = 0.0f; mwa = 0.0f;
        calcPairRelation(ref mpd, ref mpa, roomNo);  //mpaはまだできてない．．．
        calcConversation(ref mcd, ref mca, roomNo);
        calcVisualBalance(ref mvb, roomNo);
        calcAlignment(ref mfa, ref mwa, roomNo);

        cost += wpd * mpd + wpa * mpa + wcd * mcd + wca * mca + wvb * mvb + wfa * mfa + wwa * mwa;
        return cost;
    }


    float costFunction()
    {
        float cost_all = 0;
        float[] cost = new float[room.Count];

        float mpd = 0, mpa = 0, wpd = 6.5f, wpa = 4.0f;     //Pairwise Relationship
        float mcd = 0, mca = 0, wcd = 4.5f, wca = 3.0f;     //Conversation
        float mvb = 0, wvb = 3.0f;                          //Balance 15
        float mfa = 0, mwa = 0, wfa = 3.5f, wwa = 3.5f;     //Alignment 2.5 2.5

        for(int i = 0; i < room.Count; i++)
        {
            mpd = 0.0f; mpa = 0.0f; mcd = 0.0f; mca = 0.0f; mvb = 0.0f; mfa = 0.0f; mwa = 0.0f;
            calcPairRelation(ref mpd, ref mpa, i);  //mpaはまだできてない．．．
            calcConversation(ref mcd, ref mca, i);
            calcVisualBalance(ref mvb, i);
            calcAlignment(ref mfa, ref mwa, i);

            cost[i] = wpd * mpd + wpa * mpa + wcd * mcd + wca * mca + wvb * mvb + wfa * mfa + wwa * mwa;
            cost_all += cost[i];
        }

        return cost_all;
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
        sphere.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        cubeList = new List<GameObject>();
        cubePoint = new List<Vector3>();
        room = new List<Vector4>();
        onFurniture = new GameObject[furniture.Length];
        tempFurniture = new GameObject[furniture.Length];
        mintrans = new Vector3[furniture.Length];
        minrotation = new Vector3[furniture.Length];

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
            int loopCount = 5000;

            for (int i = 0; i < room.Count; i++)
            {
                mincost = 9999;
                for (int j = 0; j < loopCount; j++)
                {
                    MetropolisHastings_v2(i);
                }
                for (int k = 0; k < furniture.Length; k++)
                {
                    onFurniture[k].transform.position = new Vector3(mintrans[k].x, mintrans[k].y, mintrans[k].z);
                    onFurniture[k].transform.eulerAngles = new Vector3(minrotation[k].x, minrotation[k].y, minrotation[k].z);
                }
            }

            ////部屋間をスワップする(確率1/2でする．)
            //int rswap = Random.Range(0, 2);
            //if (rswap == 1)
            //{
            //    Vector3 ttemp;
            //    Collider[] col1, col2;
            //    int onefID, twofID, limit = 0;
            //    tempFurniture = onFurniture;
            //    do
            //    {
            //        if (limit++ > 100)
            //        {
            //            onefID = twofID = 0;
            //            break;
            //        }
            //        onefID = Random.Range(0, furniture.Length);
            //        twofID = onefID;
            //        while (twofID == onefID)
            //        {
            //            twofID = Random.Range(0, furniture.Length);
            //        }
            //        ttemp = tempFurniture[onefID].transform.position;
            //        tempFurniture[onefID].transform.position = tempFurniture[twofID].transform.position;
            //        tempFurniture[twofID].transform.position = ttemp;
            //        BoxCollider box1 = tempFurniture[onefID].GetComponent<BoxCollider>();
            //        BoxCollider box2 = tempFurniture[twofID].GetComponent<BoxCollider>();
            //        col1 = Physics.OverlapBox(box1.bounds.center, box1.bounds.size / 2.0f, tempFurniture[onefID].transform.rotation);
            //        col2 = Physics.OverlapBox(box2.bounds.center, box2.bounds.size / 2.0f, tempFurniture[twofID].transform.rotation);
            //        tempFurniture = onFurniture;
            //    } while (col1.Length > 1 || col2.Length > 1);
            //    ttemp = onFurniture[onefID].transform.position;
            //    onFurniture[onefID].transform.position = onFurniture[twofID].transform.position;
            //    onFurniture[twofID].transform.position = ttemp;
            //}


            //for(int i = 0; i < loopCount; i++)
            //{
            //    MetropolisHastings();
            //}
            //for (int i = 0; i < furniture.Length; i++)
            //{
            //    onFurniture[i].transform.position = new Vector3(mintrans[i].x, mintrans[i].y, mintrans[i].z);
            //    onFurniture[i].transform.eulerAngles = new Vector3(minrotation[i].x, minrotation[i].y, minrotation[i].z);
            //}
            Debug.Log(loopCount.ToString() + "回");
            Debug.Log(mincost);
            //状態を遷移させる
            mincost = 9999;
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
