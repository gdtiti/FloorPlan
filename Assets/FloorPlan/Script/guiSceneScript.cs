using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class guiSceneScript : MonoBehaviour {
    public Text text;
    public static bool lightFlag = false;
    public static bool furnitureFlag = false;
    public static bool suggestion = false;
    public static int cameraCount = 0;          //Camera変更のため
    public static bool allign = false;
    public static bool doorFlag = false;

    void Start()
    {
        //text.text = "Setting Floor Plan ...";
        //mainCamera = true;
        //sceneCamera = false;
    }

    void Update()
    {
        //if (lightFlag)
        //{
        //    text.text = "Setting Lighting positions ...";
        //}
        //if (furnitureFlag)
        //{
        //    text.text = "Setting Furniture positions ...";
        //}
        //if (suggestion)
        //{
        //    text.text = "Suggestion";
        //}
    }


    public void OnLightButtonClick()
    {
        lightFlag = true;
        furnitureFlag = false;
        suggestion = false;
        FloorPlan.floorFlag = false;
        doorFlag = false;
    }

    public void OnDoorButtonClick()
    {
        doorFlag = true;
        lightFlag = false;
        suggestion = false;
        FloorPlan.floorFlag = false;
    }

    public void OnFurnitureButtonClick()
    {
        furnitureFlag = true;
        lightFlag = false;
        suggestion = false;
        FloorPlan.floorFlag = false;
        doorFlag = false;
    }

    public void OnSuggestionButtonClick()
    {
        suggestion = true;
        furnitureFlag = false;
        lightFlag = false;
        FloorPlan.floorFlag = false;
        doorFlag = false;
    }

    //カメラ変更ボタンのクリック回数を調べる．
    public void OnChangeCameraButtonClick()
    {
        cameraCount++;
    }

    public void OnAllignButton()
    {
        allign = true;
    }
 
}
