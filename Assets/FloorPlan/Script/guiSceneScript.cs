using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class guiSceneScript : MonoBehaviour {
    public Text text;
    public static bool lightFlag = false;
    public static bool furnitureFlag = false;
    public static bool suggestion = false;

    void Start()
    {
        text.text = "Setting Floor Plan ...";
    }

    void Update()
    {
        if (lightFlag)
        {
            text.text = "Setting Lighting positions ...";
        }
        if (furnitureFlag)
        {
            text.text = "Setting Furniture positions ...";
        }
        if (suggestion)
        {
            text.text = "Suggestion";
        }
    }


    public void OnLightButtonClick()
    {
        lightFlag = true;
        FloorPlan.floorFlag = false;
    }

    public void OnFurnitureButtonClick()
    {
        furnitureFlag = true;
        lightFlag = false;
        FloorPlan.floorFlag = false;
    }

    public void OnSuggestionButtonClick()
    {
        suggestion = true;
        furnitureFlag = false;
        lightFlag = false;
        FloorPlan.floorFlag = false;
    }
 
}
