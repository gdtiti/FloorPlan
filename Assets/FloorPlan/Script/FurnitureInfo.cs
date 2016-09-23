using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;

public class FurnitureInfo : MonoBehaviour {
    private string inputFileName = "initFurniture.txt";
    private string comment = "";
    private string space = "";
    public static int numFurniture;
    public static int numSetFurniture;
    public static List<string> furniturePath;
    public static List<List<int>> f_set;
    public static List<List<float>> pfg;
    public static List<List<float>> mfg;
    public static List<List<float>> Mfg;

    public static List<List<float>> qfg;
    public static List<List<float>> mc;
    public static List<List<float>> Mc;

    public static float wci, wpd, wpa, wcd, wca, wvb, wfa, wwa, wef, wsy;

    public static string windowPath, doorPath;
    public static string twallPath, tfloorPath;

    public static float wall_height;

    // Use this for initialization
    void Start () {
        furniturePath = new List<string>();
        f_set = new List<List<int>>();
        pfg = new List<List<float>>();
        mfg = new List<List<float>>();
        Mfg = new List<List<float>>();

        qfg = new List<List<float>>();
        mc = new List<List<float>>();
        Mc = new List<List<float>>();

        ReadFile();
    }
	
	// Update is called once per frame
	void Update () {

	}

    void ReadFile()
    {
        FileInfo fi = new FileInfo(Application.dataPath + "/FloorPlan/Configuration/" + inputFileName);
        try
        {
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
            {
                //家具の読み込み
                comment = sr.ReadLine();
                numFurniture = Int32.Parse(sr.ReadLine());
                for (int i = 0; i < numFurniture; i++)
                {
                    furniturePath.Add(sr.ReadLine());
                }
                space = sr.ReadLine();

                //家具セットの読み込み
                comment = sr.ReadLine();
                numSetFurniture = Int32.Parse(sr.ReadLine());
                for (int i = 0; i < numSetFurniture; i++)
                {
                    var line = sr.ReadLine();
                    var values = line.Split('\t');
                    List<int> addData = new List<int>();
                    for (int j = 0; j < Int32.Parse(values[0]); j++)
                    {
                        addData.Add(Int32.Parse(values[j + 1]));
                    }
                    f_set.Add(addData);
                }
                space = sr.ReadLine();

                //Pairwise relationship(pfg)の読み込み
                comment = sr.ReadLine();
                for (int i = 0; i < numFurniture; i++)
                {
                    var line = sr.ReadLine();
                    var values = line.Split('\t');
                    List<float> addData = new List<float>();
                    for (int j = 0; j < numFurniture; j++)
                    {
                        addData.Add(float.Parse(values[j]));
                    }
                    pfg.Add(addData);
                }
                space = sr.ReadLine();

                //Pairwise relationship(mfg)の読み込み
                comment = sr.ReadLine();
                for (int i = 0; i < numFurniture; i++)
                {
                    var line = sr.ReadLine();
                    var values = line.Split('\t');
                    List<float> addData = new List<float>();
                    for (int j = 0; j < numFurniture; j++)
                    {
                        addData.Add(float.Parse(values[j]));
                    }
                    mfg.Add(addData);
                }
                space = sr.ReadLine();

                //Pairwise relationship(Mfg)の読み込み
                comment = sr.ReadLine();
                for (int i = 0; i < numFurniture; i++)
                {
                    var line = sr.ReadLine();
                    var values = line.Split('\t');
                    List<float> addData = new List<float>();
                    for (int j = 0; j < numFurniture; j++)
                    {
                        addData.Add(float.Parse(values[j]));
                    }
                    Mfg.Add(addData);
                }
                space = sr.ReadLine();

                //Conversation(qfg)
                comment = sr.ReadLine();
                for (int i = 0; i < numFurniture; i++)
                {
                    var line = sr.ReadLine();
                    var values = line.Split('\t');
                    List<float> addData = new List<float>();
                    for (int j = 0; j < numFurniture; j++)
                    {
                        addData.Add(float.Parse(values[j]));
                    }
                    qfg.Add(addData);
                }
                space = sr.ReadLine();

                //Conversation(mc)
                comment = sr.ReadLine();
                for (int i = 0; i < numFurniture; i++)
                {
                    var line = sr.ReadLine();
                    var values = line.Split('\t');
                    List<float> addData = new List<float>();
                    for (int j = 0; j < numFurniture; j++)
                    {
                        addData.Add(float.Parse(values[j]));
                    }
                    mc.Add(addData);
                }
                space = sr.ReadLine();

                //Conversation(Mc)
                comment = sr.ReadLine();
                for (int i = 0; i < numFurniture; i++)
                {
                    var line = sr.ReadLine();
                    var values = line.Split('\t');
                    List<float> addData = new List<float>();
                    for (int j = 0; j < numFurniture; j++)
                    {
                        addData.Add(float.Parse(values[j]));
                    }
                    Mc.Add(addData);
                }
                space = sr.ReadLine();

                //重みの読み込み
                comment = sr.ReadLine();
                for (int i = 0; i < 1; i++)
                {
                    var line = sr.ReadLine();
                    var values = line.Split('\t');
                    wci = float.Parse(values[0]);   wpd = float.Parse(values[1]);   wpa = float.Parse(values[2]);
                    wcd = float.Parse(values[3]);   wca = float.Parse(values[4]);   wvb = float.Parse(values[5]);
                    wfa = float.Parse(values[6]);   wwa = float.Parse(values[7]);   wef = float.Parse(values[8]);
                    wsy = float.Parse(values[9]); 
                }
                space = sr.ReadLine();

                //窓のモデル読み込み
                comment = sr.ReadLine();
                windowPath = sr.ReadLine();
                space = sr.ReadLine();

                //ドアのモデル読み込み
                comment = sr.ReadLine();
                doorPath = sr.ReadLine();
                space = sr.ReadLine();

                //床のテクスチャ読み込み
                comment = sr.ReadLine();
                tfloorPath = sr.ReadLine();
                space = sr.ReadLine();

                //壁のテクスチャ読み込み
                comment = sr.ReadLine();
                twallPath = sr.ReadLine();
                space = sr.ReadLine();

                //壁の高さ
                comment = sr.ReadLine();
                wall_height = float.Parse(sr.ReadLine());
                space = sr.ReadLine();
            }
        }
        catch (Exception e)
        {
            Debug.Log("File Read Error");
            Debug.Log(e.Message);
        }
    }

}
