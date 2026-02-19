using UnityEngine ;
using Unity.Robotics.ROSTCPConnector ;
using RosMessageTypes.Std ;

public class MyPublisher : MonoBehaviour
{
    ROSConnection ros ;
    float time ;

    //初期化時に呼ばれる
    void Start()
    {
        //ROSコネクションの取得
        ros = ROSConnection.GetOrCreateInstance();

        //パブリッシャーの登録
        ros.RegisterPublisher<StringMsg>("my_topic") ;
    }

    //フレーム毎に呼ばれる
    void Update()
    {
        // 処理を１秒毎に制限
        time += Time.deltaTime ;
        if (time < 1.0f) return ;
        time = 0.0f ;

        //メッセージのパブリッシュ
        StringMsg msg = new StringMsg("Hello Unity!") ;
        ros.Publish("my_topic", msg) ;
    }
}