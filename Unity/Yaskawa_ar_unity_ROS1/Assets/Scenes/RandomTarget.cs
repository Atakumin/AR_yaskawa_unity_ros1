using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTarget : MonoBehaviour
{
    // 生成するプレハブ格納用
    [SerializeField] GameObject Target;

    // プレハブ間の最小距離
    [SerializeField] float minDistance = 0.1f;

    // 既存のプレハブの位置を記録するリスト
    private List<Vector3> positions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        int numberOfTargets = 4; // 生成するターゲットの数

        for (int i = 0; i < numberOfTargets; i++)
        {
            Vector3 pos;
            bool isValidPosition;

            do
            {
                // プレハブの位置をランダムで設定
                float x = Random.Range(0.077f, 0.25f);
                float z = Random.Range(0.23f, 0.45f);
                pos = new Vector3(x, 0.225f, z);

                // 新しい位置が既存の位置と十分に離れているか確認
                isValidPosition = true;
                foreach (Vector3 existingPos in positions)
                {
                    if (Vector3.Distance(pos, existingPos) < minDistance)
                    {
                        isValidPosition = false;
                        break;
                    }
                }
            }
            while (!isValidPosition);

            // 有効な位置が見つかったら、リストに追加してプレハブを生成
            positions.Add(pos);
            Instantiate(Target, pos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
