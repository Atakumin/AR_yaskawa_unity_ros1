using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI; // UIを扱うために追加

public class Put : MonoBehaviour
{
    // 配置するオブジェクトのリスト
    [SerializeField] private List<GameObject> objectsToPlace;

    // 現在選択されているオブジェクトのインデックス
    private int selectedIndex = 0;

    // ARRaycastManagerのインスタンス
    private ARRaycastManager raycastManager;

    // Raycastの結果を格納するリスト
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // 生成されたオブジェクトを追跡するためのリスト
    private List<GameObject> instantiatedObjects = new List<GameObject>();

    // 最初のフレームの前に呼び出される
    void Start()
    {
        // ARRaycastManagerを取得
        raycastManager = GetComponent<ARRaycastManager>();
    }

    // 毎フレーム呼び出される
    void Update()
    {
        // 画面がタッチされたかチェック
        if (Input.touchCount > 0)
        {
            // タッチ情報を格納
            Touch touch = Input.GetTouch(0);

            // タッチのフェーズがBeganまたはMovedの場合
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
            {
                // タッチした位置からRayを飛ばして、Planeにヒットした情報をhitsに格納する
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    // 物体を生成し、その参照をリストに保持する
                    GameObject newObject = Instantiate(objectsToPlace[selectedIndex], hits[0].pose.position, Quaternion.identity);
                    instantiatedObjects.Add(newObject);
                }
            }
        }
    }

    // UIから呼び出される、選択されたオブジェクトを設定するメソッド
    public void SetSelectedIndex(int index)
    {
        if (index >= 0 && index < objectsToPlace.Count)
        {
            selectedIndex = index;
        }
    }
}
