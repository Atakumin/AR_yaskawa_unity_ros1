using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTargets : MonoBehaviour
{
    // 生成するプレハブ格納用
    public GameObject PrefabTarget;
    public int numberOfTargets = 4; // 生成するプレハブの数
    private int TargetCount = 0; // 生成したプレハブのカウント

    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        // 30フレーム毎にシーンにプレハブを生成
        if(Time.frameCount % 30 == 0 && TargetCount < numberOfTargets)
        {
            for (int i = 0; i < numberOfTargets; i++)
            {
                // プレハブの位置をランダムで設定
                float x = Random.Range(0.0f, 0.24f);
                float z = Random.Range(0.24f, 0.45f);
                Vector3 pos = new Vector3(x, 0.23f, z);

                // プレハブを生成
                GameObject newTarget = Instantiate(PrefabTarget, pos, Quaternion.identity);
                
                // プレハブに名前を付ける
                newTarget.name = "Target_" + TargetCount;

                // カウントを増加
                TargetCount++;

                // 指定した数のキューブが生成されたらループを終了
                if (TargetCount >= numberOfTargets)
                {
                    break;
                }
            }
        }    
    }
}

