using UnityEngine;
using System.Collections;


public class line : MonoBehaviour {
    public Transform[] transforms;
    LineRenderer lineRenderer;

    private Vector3 position, camposition, floorPosition;
    private Vector3 screenToWorldPointPosition;
    public Camera mainCamera;
    private Vector3[] mousePoint;
    int flag = 0;

    public Camera firstPersonalCamera;
    public Camera overheadCamera;

    public void ShowOverHeadView()
    {
        firstPersonalCamera.enabled = false;
        overheadCamera.enabled = true;
    }
    public void ShowFirstPersonView()
    {
        firstPersonalCamera.enabled = true;
        overheadCamera.enabled = false;
    }

    // Use this for initialization
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount(transforms.Length);
        camposition = mainCamera.transform.position;
        mousePoint = new Vector3[2];
        ShowFirstPersonView();    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            ShowOverHeadView();
        }else if (Input.GetKey(KeyCode.O))
        {
            ShowFirstPersonView();
        }

        if(Input.GetMouseButton(0))
        {
            position = Input.mousePosition;
            position.z = 10f;
            screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);

            float t = camposition.y / (camposition.y - screenToWorldPointPosition.y);

            floorPosition = t * screenToWorldPointPosition + (1 - t) * camposition;

            if(flag == 0)
            {
                mousePoint[flag] = floorPosition;
                flag = 1;
            }else if(flag == 1)
            {
                mousePoint[flag] = floorPosition;
            }
            Debug.Log(position);
            Debug.Log(screenToWorldPointPosition);
            Debug.Log(camposition);
            Debug.Log(floorPosition);
            Debug.Log("========");
        }
        


        //for (int i = 0; i < transforms.Length; i++)
        //{
        //    lineRenderer.SetPosition(i, transforms[i].position);
            
        //}

        for(int i=0;i< flag; i++)
        {
            lineRenderer.SetPosition(i, mousePoint[i]);
            if(flag == 1)
            {
                flag = 0;
            }
        }

    }
}
