using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PutRobotarm : MonoBehaviour
{
    // 平面に生成するオブジェクト
    [SerializeField] GameObject Sia5;

    // ARRaycastManager
    ARRaycastManager raycastManager;

    // RaycastとPlaneが衝突した情報を格納
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // 生成されたオブジェクトを追跡するための変数
    private GameObject instantiatedObject;

    // Startは最初のフレームの前に呼び出される
    void Start()
    {
        // ARRaycastManagerを取得する
        raycastManager = GetComponent<ARRaycastManager>();
    }

    // Updateは毎フレーム呼び出される
    void Update()
    {
        // 画面がタッチされたかチェック
        if (Input.touchCount > 0)
        {
            // タッチ情報を格納
            Touch touch = Input.GetTouch(0);

            // タッチした位置からRayを飛ばして、Planeにヒットした情報をhitsに格納する
            if (raycastManager.Raycast(touch.position, hits, TrackableType.Planes))
            {
                // すでにオブジェクトが生成されている場合は削除する
                if (instantiatedObject != null)
                {
                    Destroy(instantiatedObject);
                }

                // 物体を生成し、その参照を保持する
                instantiatedObject = Instantiate(Sia5, hits[0].pose.position, Quaternion.identity);
            }
        }
    }
}




