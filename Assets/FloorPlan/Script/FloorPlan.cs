using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class FloorPlan : MonoBehaviour {
    public GameObject[] furniture;              //家具の読み込み
    private GameObject[] onFurniture;           //置いた家具
    private GameObject[] tempFurniture;         //swap用の家具のゲームオブジェクト
    private GameObject[] beforeFurniture;       //遷移前の置いた家具の位置を保存
    private Vector3[] mintrans, minrotation;    //mincostのときの位置と回転
    private Vector3[] beforeTrans, beforeRotation;//遷移前の位置と回転
    private Vector3[] tempTrans, tempRotation;  //いったん保存用の位置と回転

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
            float jou = 2.3f;
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
        //for (int i = 0; i < furniture.Length; i++)
        //{
        //    beforeTrans[i] = onFurniture[i].transform.position;
        //    beforeRotation[i] = onFurniture[i].transform.position;
        //}

        float p0, bcost = 0, p, alpha, acost = 0;

        //for (int i = 0; i < room.Count; i++)
        //{
        //    bcost += costFunction_v2(i);
        //}
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
            //for (int i = 0; i < furniture.Length; i++)
            //{
            //    tempTrans[i] = new Vector3(onFurniture[i].transform.position.x, onFurniture[i].transform.position.y, onFurniture[i].transform.position.z);
            //}
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
            //onFurniture = beforeFurniture;
            for (int i = 0; i < furniture.Length; i++)
            {
                onFurniture[i].transform.position = beforeTrans[i];
                onFurniture[i].transform.eulerAngles = beforeRotation[i];
            }

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
        //for (int i = 0; i < furniture.Length; i++)
        //{
        //    beforeTrans[i] = onFurniture[i].transform.position;
        //    beforeRotation[i] = onFurniture[i].transform.position;
        //}

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

        //既存手法の場合(0,3)，複数部屋の場合(0,4)
        int move = Random.Range(0, 4);
        //move = 3;
        //GaussianMove
        if (move == 0 && inroom.Count != 0)
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
        else if (move == 1 && inroom.Count != 0)
        {
            int fid = inroom[Random.Range(0, inroom.Count)];
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
        //RandomSwap(部屋内のスワップ)
        else if (move == 2 && inroom.Count > 1)
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
        //部屋間で1個だけ一気に移動する遷移．
        else if(move == 3 && inroom.Count != 0) 
        {
            int fid = inroom[Random.Range(0, inroom.Count)];
            
            int changeRoom;
            do
            {
                changeRoom = Random.Range(0, room.Count);
            } while (changeRoom == roomNo);

            Collider[] col;
            int limit = 0;
            Vector3 bTrans = onFurniture[fid].transform.position;
            do
            {
                if (limit++ > 100)
                {
                    onFurniture[fid].transform.position = bTrans;
                    break;
                }
                onFurniture[fid].transform.position =
                 new Vector3(Random.Range(room[changeRoom].x, room[changeRoom].z), 0.02f, Random.Range(room[changeRoom].y, room[changeRoom].w));
                BoxCollider box = onFurniture[fid].GetComponent<BoxCollider>();
                col = Physics.OverlapBox(box.bounds.center, box.bounds.size / 2.0f, onFurniture[fid].transform.rotation);
            } while (col.Length > 1);

        }
        //inroom.Clear();

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
            //for (int i = 0; i < furniture.Length; i++)
            //{
            //    onFurniture[i].transform.position = beforeTrans[i];
            //    onFurniture[i].transform.eulerAngles = beforeRotation[i];
            //}
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

    void MetropolisHastings_v3()
    {
        //前の状態を保存
        beforeFurniture = onFurniture;
        //for (int i = 0; i < furniture.Length; i++)
        //{
        //    beforeTrans[i] = new Vector3(onFurniture[i].transform.position.x, onFurniture[i].transform.position.y, onFurniture[i].transform.position.z);
        //    beforeRotation[i] = new Vector3(onFurniture[i].transform.position.x, onFurniture[i].transform.position.y, onFurniture[i].transform.position.z);
        //}

        float p0, bcost = 0, p, alpha, acost = 0;

        for (int i = 0; i < room.Count; i++)
        {
            bcost += costFunction_v2(i);
        }
        p0 = densityFunction(bcost);

        int move = Random.Range(0, 4);
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
        //RandomSwap(部屋内スワップ)
        else if (move == 2)
        {
            Vector3 swaptemp = new Vector3();
            Collider[] col1, col2;
            int onefID, twofID, limit = 0;
            tempFurniture = onFurniture;
            for (int i = 0; i < furniture.Length; i++)
            {
                tempTrans[i] = new Vector3(onFurniture[i].transform.position.x, onFurniture[i].transform.position.y, onFurniture[i].transform.position.z);
            }
            
            int roomID = Random.Range(0, room.Count);

            List<int> inroom;
            inroom = new List<int>();

            for (int i = 0; i < furniture.Length; i++)
            {
                if (onFurniture[i].transform.position.x > room[roomID].x && onFurniture[i].transform.position.x < room[roomID].z)
                {
                    if (onFurniture[i].transform.position.z < room[roomID].y && onFurniture[i].transform.position.z > room[roomID].w)
                    {
                        inroom.Add(i);
                    }
                    else if (onFurniture[i].transform.position.z > room[roomID].y && onFurniture[i].transform.position.z < room[roomID].w)
                    {
                        inroom.Add(i);
                    }
                }
            }

            if(inroom.Count > 1)
            {
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
                        twofID = inroom[Random.Range(0,inroom.Count)];
                    }
                    swaptemp = tempFurniture[onefID].transform.position;
                    tempFurniture[onefID].transform.position = tempFurniture[twofID].transform.position;
                    tempFurniture[twofID].transform.position = swaptemp;
                    BoxCollider box1 = tempFurniture[onefID].GetComponent<BoxCollider>();
                    BoxCollider box2 = tempFurniture[twofID].GetComponent<BoxCollider>();
                    col1 = Physics.OverlapBox(box1.bounds.center, box1.bounds.size / 2.0f, tempFurniture[onefID].transform.rotation);
                    col2 = Physics.OverlapBox(box2.bounds.center, box2.bounds.size / 2.0f, tempFurniture[twofID].transform.rotation);
                    tempFurniture = onFurniture;
                    //for (int i = 0; i < furniture.Length; i++)
                    //{
                    //    tempFurniture[i].transform.position = new Vector3(tempTrans[i].x, tempTrans[i].y, tempTrans[i].z);
                    //}
                } while (col1.Length > 1 || col2.Length > 1);
                if(onefID != twofID)
                {
                    swaptemp = new Vector3(onFurniture[onefID].transform.position.x, onFurniture[onefID].transform.position.y, onFurniture[onefID].transform.position.z);
                    onFurniture[onefID].transform.position = new Vector3(onFurniture[twofID].transform.position.x, onFurniture[twofID].transform.position.y, onFurniture[twofID].transform.position.z);
                    onFurniture[twofID].transform.position = new Vector3(swaptemp.x, swaptemp.y, swaptemp.z);
                }
                else
                {
                    for (int i = 0; i < furniture.Length; i++)
                    {
                        onFurniture[i].transform.position = new Vector3(tempTrans[i].x, tempTrans[i].y, tempTrans[i].z);
                    }
                }
            }
            
        }
        //1個一気に移動
        else if (move == 3)
        {
            int fid = Random.Range(0, furniture.Length);
            //家具がどの部屋にあるのか調べるルーチン
            int rid = new int();
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
            }

            int changeRoom;
            do
            {
                changeRoom = Random.Range(0, room.Count);
            } while (changeRoom == rid);

            Collider[] col;
            int limit = 0;
            Vector3 bTrans = onFurniture[fid].transform.position;
            do
            {
                if (limit++ > 100)
                {
                    onFurniture[fid].transform.position = bTrans;
                    break;
                }
                onFurniture[fid].transform.position =
                 new Vector3(Random.Range(room[changeRoom].x, room[changeRoom].z), 0.02f, Random.Range(room[changeRoom].y, room[changeRoom].w));
                BoxCollider box = onFurniture[fid].GetComponent<BoxCollider>();
                col = Physics.OverlapBox(box.bounds.center, box.bounds.size / 2.0f, onFurniture[fid].transform.rotation);
            } while (col.Length > 1);

        }


        for (int i = 0; i < room.Count; i++)
        {
            acost += costFunction_v2(i);
        }
        p = densityFunction(acost);
        alpha = p / p0;
        if (alpha > 1) alpha = 1.0f;
        float t = Random.Range(0.0f, 1.0f);

        //状態を遷移しない
        if (alpha <= t)
        {
            //前の状態に戻す．
            onFurniture = beforeFurniture;
            //for (int i = 0; i < furniture.Length; i++)
            //{
            //    onFurniture[i].transform.position = new Vector3(beforeTrans[i].x, beforeTrans[i].y, beforeTrans[i].z);
            //    onFurniture[i].transform.eulerAngles = new Vector3(beforeRotation[i].x, beforeTrans[i].y, beforeTrans[i].z);
            //}

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
                }else if((onFurniture[inroom[i]].gameObject.tag == "DiningTable" && onFurniture[inroom[j]].gameObject.tag == "CoffeeTable")
                    ||(onFurniture[inroom[i]].gameObject.tag == "CoffeeTable" && onFurniture[inroom[j]].gameObject.tag == "DiningTable"))
                {
                    p = 1.0f; mind = 2.6f; maxd = 3.5f;
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
                } else if ((onFurniture[inroom[i]].gameObject.tag == "CoffeeTable" && onFurniture[inroom[j]].gameObject.tag == "TV")
                    ||(onFurniture[inroom[i]].gameObject.tag == "TV" && onFurniture[inroom[j]].gameObject.tag == "CoffeeTable"))
                {
                    q = 1.0f;mind = 1.1f; maxd = 1.9f;
                    cosfg = Vector3.Dot(f, d); cosgf = Vector3.Dot(g, -d);
                } else if ((onFurniture[inroom[i]].gameObject.tag == "CoffeeTable" && onFurniture[inroom[j]].gameObject.tag == "Armchair")
                 || (onFurniture[inroom[i]].gameObject.tag == "Armchair" && onFurniture[inroom[j]].gameObject.tag == "CoffeeTable"))
                {
                    q = 1.0f; mind = 1.1f; maxd = 1.6f;
                    cosfg = Vector3.Dot(f, d); cosgf = Vector3.Dot(g, -d);
                }
                else if (onFurniture[inroom[i]].gameObject.tag == "DiningChair" && onFurniture[inroom[j]].gameObject.tag == "DiningChair")
                {
                    q = 1.0f; mind = 2.4f; maxd = 3.6f;
                    cosfg = Vector3.Dot(f, d); cosgf = Vector3.Dot(g, -d);
                } else if ((onFurniture[inroom[i]].gameObject.tag == "DiningChair" && onFurniture[inroom[j]].gameObject.tag == "TV")
                     || (onFurniture[inroom[i]].gameObject.tag == "TV" && onFurniture[inroom[j]].gameObject.tag == "DiningChair"))
                {
                    q = 1.0f; mind = 1.7f; maxd = 2.3f;
                    cosfg = Vector3.Dot(f, d); cosgf = Vector3.Dot(g, -d);
                } else if ((onFurniture[inroom[i]].gameObject.tag == "DiningTable" && onFurniture[inroom[j]].gameObject.tag == "DiningChair")
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
        float sumArea = 0.0f;
        mvb = 0.0f;
        Vector3 centroid, rCenter;
        rCenter = new Vector3((room[roomID].x + room[roomID].z) / 2.0f, 0.0f, (room[roomID].y + room[roomID].w) / 2.0f);
        centroid = new Vector3(0.0f, 0.0f, 0.0f);
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
            float oneSize = onFurniture[inroom[i]].GetComponent<BoxCollider>().bounds.size.x * onFurniture[inroom[i]].GetComponent<BoxCollider>().bounds.size.z;
            sumArea += oneSize;
            centroid += oneSize * onFurniture[inroom[i]].transform.position;
        }
        centroid /= sumArea;

        mvb = (centroid - rCenter).magnitude;

    }

    //中心を向くようにする
    void calcEmphasis(ref float mef,ref float msy,int roomID)
    {
        mef = 0.0f; msy = 0.0f;
        Vector3 d, dc, rcenter;
        rcenter = new Vector3((room[roomID].x + room[roomID].z) / 2.0f, 0.0f, (room[roomID].y + room[roomID].w) / 2.0f);
        List<int> inroom;
        inroom = new List<int>();

        for (int i = 0; i < furniture.Length; i++)
        {
            if (onFurniture[i].transform.position.x > room[roomID].x && onFurniture[i].transform.position.x < room[roomID].z)
            {
                if (onFurniture[i].transform.position.z < room[roomID].y && onFurniture[i].transform.position.z > room[roomID].w)
                {
                    inroom.Add(i);
                }
                else if (onFurniture[i].transform.position.z > room[roomID].y && onFurniture[i].transform.position.z < room[roomID].w)
                {
                    inroom.Add(i);
                }
            }
        }

        for (int i = 0; i < inroom.Count; i++)
        {
            if(onFurniture[inroom[i]].gameObject.tag == "Armchair" || onFurniture[inroom[i]].gameObject.tag == "Sofa" 
                || onFurniture[inroom[i]].gameObject.tag == "DiningChair" || onFurniture[inroom[i]].gameObject.tag == "TV")
            {
                float theta = onFurniture[inroom[i]].transform.eulerAngles.y;
                d = new Vector3(Mathf.Sin(theta * Mathf.PI / 180.0f), 0.0f, Mathf.Cos(theta * Mathf.PI / 180.0f));
                dc = rcenter - onFurniture[inroom[i]].transform.position;
                dc /= dc.magnitude;
                mef -= Vector3.Dot(d, dc);
            }
        }

    }

    //歩ける空間を確保する
    void calcCirculation(ref float mci, int roomID)
    {
        mci = 0.0f;
        float cfree, roomArea, sumArea = 0.0f;
        List<int> inroom;
        inroom = new List<int>();

        for (int i = 0; i < furniture.Length; i++)
        {
            if (onFurniture[i].transform.position.x > room[roomID].x && onFurniture[i].transform.position.x < room[roomID].z)
            {
                if (onFurniture[i].transform.position.z < room[roomID].y && onFurniture[i].transform.position.z > room[roomID].w)
                {
                    inroom.Add(i);
                }
                else if (onFurniture[i].transform.position.z > room[roomID].y && onFurniture[i].transform.position.z < room[roomID].w)
                {
                    inroom.Add(i);
                }
            }
        }

        for (int i = 0; i < inroom.Count; i++)
        {
            sumArea += onFurniture[inroom[i]].GetComponent<BoxCollider>().bounds.size.x * onFurniture[inroom[i]].GetComponent<BoxCollider>().bounds.size.z;
        }

        roomArea = Mathf.Abs(room[roomID].x - room[roomID].z) * Mathf.Abs(room[roomID].y - room[roomID].w);

        cfree = (roomArea - sumArea) / roomArea;

        mci -= t(cfree, 0.8f, 0.9f, 3);

    }


    float costFunction_v2(int roomNo)
    {
        float cost = 0;

        float mci = 0, wci = 2.0f;                          //Circulation
        float mpd = 0, mpa = 0, wpd = 1.0f, wpa = 1.0f;     //Pairwise Relationship
        float mcd = 0, mca = 0, wcd = 1.0f, wca = 1.0f;     //Conversation
        float mvb = 0, wvb = 1.5f;                          //Balance 
        float mfa = 0, mwa = 0, wfa = 2.5f, wwa = 2.5f;     //Alignment 
        float mef = 0, msy = 0, wef = 4.0f, wsy = 1.0f;     //Emphasis

       
        mpd = 0.0f; mpa = 0.0f; mcd = 0.0f; mca = 0.0f; mvb = 0.0f; mfa = 0.0f; mwa = 0.0f;
        calcPairRelation(ref mpd, ref mpa, roomNo);  //mpaはまだできてない．．．
        calcConversation(ref mcd, ref mca, roomNo);
        calcVisualBalance(ref mvb, roomNo);
        calcAlignment(ref mfa, ref mwa, roomNo);
        calcCirculation(ref mci, roomNo);
        calcEmphasis(ref mef, ref msy, roomNo);

        //cost += wci * mci / 1.0f + wpd * mpd / 10.0f + wcd * mcd / 8.0f + wca * mca / 14.0f + wvb * mvb / 5.0f + wfa * mfa / 12.0f + wwa * mwa / 4.0f + wef * mef / 3.0f;
        cost += mef;
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
        beforeTrans = new Vector3[furniture.Length];
        beforeRotation = new Vector3[furniture.Length];
        tempTrans = new Vector3[furniture.Length];
        tempRotation = new Vector3[furniture.Length];

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
            //int loopCount = 50000;

            //for (int i = 0; i < room.Count; i++)
            //{
            //    mincost = 9999;
            //    for (int j = 0; j < loopCount; j++)
            //    {
            //        MetropolisHastings_v2(i);
            //        //for (int k = 0; k < furniture.Length; k++)
            //        //{
            //        //    onFurniture[k].transform.position = new Vector3(mintrans[k].x, mintrans[k].y, mintrans[k].z);
            //        //    onFurniture[k].transform.eulerAngles = new Vector3(minrotation[k].x, minrotation[k].y, minrotation[k].z);
            //        //}
            //    }

            //    for (int k = 0; k < furniture.Length; k++)
            //    {
            //        onFurniture[k].transform.position = new Vector3(mintrans[k].x, mintrans[k].y, mintrans[k].z);
            //        onFurniture[k].transform.eulerAngles = new Vector3(minrotation[k].x, minrotation[k].y, minrotation[k].z);
            //    }
            //}

            //int loopCount = 5000;
            int loopCount = 1;
            for (int i = 0; i < loopCount; i++)
            {
                MetropolisHastings_v3();

                for(int j = 0; j < furniture.Length; j++)
                {
                    onFurniture[j].transform.position = new Vector3(mintrans[j].x, mintrans[j].y, mintrans[j].z);
                    onFurniture[j].transform.eulerAngles = new Vector3(minrotation[j].x, minrotation[j].y, minrotation[j].z);
                }
            }


            Debug.Log(loopCount.ToString() + "回");
            Debug.Log(mincost);
            //状態を遷移させる
            //mincost = 9999;
            guiSceneScript.suggestion = false;
        }

        //整列させるよ
        if (guiSceneScript.allign)
        {
            for (int i = 0; i < furniture.Length; i++)
            {
                if (Mathf.Floor((onFurniture[i].transform.eulerAngles.y + 45.0f) / 90.0f) == 0)
                {
                    onFurniture[i].transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                } else if (Mathf.Floor((onFurniture[i].transform.eulerAngles.y + 45.0f) / 90.0f) == 1) 
                {
                    onFurniture[i].transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                }else if(Mathf.Floor((onFurniture[i].transform.eulerAngles.y + 45.0f) / 90.0f) == 2)
                {
                    onFurniture[i].transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                }else if(Mathf.Floor((onFurniture[i].transform.eulerAngles.y + 45.0f) / 90.0f) == 3)
                {
                    onFurniture[i].transform.eulerAngles = new Vector3(0.0f, 270.0f, 0.0f);
                }else if(Mathf.Floor((onFurniture[i].transform.eulerAngles.y + 45.0f) / 90.0f) == 4)
                {
                    onFurniture[i].transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                }
            }

            for (int i = 0; i < furniture.Length; i++)
            {
                if(onFurniture[i].gameObject.tag == "TV")
                {
                    int i_tv = new int();
                    for (int j = 0; j < room.Count; j++)
                    {
                        if (onFurniture[i].transform.position.x > room[j].x && onFurniture[i].transform.position.x < room[j].z)
                        {
                            if (onFurniture[i].transform.position.z < room[j].y && onFurniture[i].transform.position.z > room[j].w)
                            {
                                i_tv = j; break;
                            }
                            else if (onFurniture[i].transform.position.z > room[j].y && onFurniture[i].transform.position.z < room[j].w)
                            {
                                i_tv = j; break;
                            }
                        }
                    }

                    if(Mathf.Floor(onFurniture[i].transform.eulerAngles.y) == 90)
                    {
                        onFurniture[i].transform.position = new Vector3(room[i_tv].x + onFurniture[i].GetComponent<BoxCollider>().bounds.size.x / 2.0f, onFurniture[i].transform.position.y, onFurniture[i].transform.position.z);
                    }else if(Mathf.Floor(onFurniture[i].transform.eulerAngles.y) == 180)
                    {
                        onFurniture[i].transform.position = new Vector3(onFurniture[i].transform.position.x, onFurniture[i].transform.position.y, room[i_tv].y - onFurniture[i].GetComponent<BoxCollider>().bounds.size.z / 2.0f);
                    }else if(Mathf.Floor(onFurniture[i].transform.eulerAngles.y) == 270)
                    {
                        onFurniture[i].transform.position = new Vector3(room[i_tv].z - onFurniture[i].GetComponent<BoxCollider>().bounds.size.x / 2.0f, onFurniture[i].transform.position.y, onFurniture[i].transform.position.z);
                    }else if(Mathf.Floor(onFurniture[i].transform.eulerAngles.y) == 0)
                    {
                        onFurniture[i].transform.position = new Vector3(onFurniture[i].transform.position.x, onFurniture[i].transform.position.y, room[i_tv].w + onFurniture[i].GetComponent<BoxCollider>().bounds.size.z / 2.0f);
                    }
                }
            }


            guiSceneScript.allign = false;
        }


        //カメラを変える
        if(guiSceneScript.cameraCount % 2 == 0)
        {
            mainCamera.enabled = true;
            sceneViewCamera.enabled = false;
            sphere.gameObject.SetActive(true);
        }else if(guiSceneScript.cameraCount % 2 == 1)
        {
            sphere.gameObject.SetActive(false);
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

        //---------------------------------------距離感の確認---------------------------------------------
        if (Input.GetKey(KeyCode.UpArrow))
        {
            onFurniture[3].transform.position += new Vector3(0.0f, 0.0f, 0.05f);
            Debug.Log((onFurniture[3].transform.position - onFurniture[11].transform.position).magnitude);
        }else if (Input.GetKey(KeyCode.DownArrow))
        {
            onFurniture[3].transform.position += new Vector3(0.0f, 0.0f, -0.05f);
            Debug.Log((onFurniture[3].transform.position - onFurniture[11].transform.position).magnitude);
        }else if (Input.GetKey(KeyCode.RightArrow))
        {
            onFurniture[3].transform.eulerAngles += new Vector3(0.0f, 1.0f, 0.0f);
            Debug.Log(onFurniture[3].transform.eulerAngles.y);
        }
        //--------------------------------------------------------------------------------------------------




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
