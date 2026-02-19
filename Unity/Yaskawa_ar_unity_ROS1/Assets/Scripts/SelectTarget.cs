using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SelectTarget : MonoBehaviour
{
    // AR FoundationのRaycastを管理するマネージャー
    private ARRaycastManager arRaycastManager;

    // Raycastのヒット結果を格納するリスト
    private List<ARRaycastHit> hitResults = new List<ARRaycastHit>();


    void Start()
    {
        // AR Raycast Managerの取得
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    public void Update()
    {

    }

    public void SelectTargets()
    {
        print("Raycheck0");
        RaycastHit hit;
        print("Raycheck1");
        var ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        
        print("Raycheck2");
        if (Physics.Raycast(ray, out hit)) 
        {
            // ray処理
            print("Raycheck3");
        }
    }
}