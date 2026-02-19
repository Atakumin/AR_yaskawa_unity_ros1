using System.Collections ;
using System.Collections.Generic ;
using UnityEngine ;
using Unity.Robotics.ROSTCPConnector ;
using RosMessageTypes.Std ;

public class MySubscriber : MonoBehaviour
{
    ROSConnection ros ;

    //初期化時に呼ばれる
    void Start()
    {
        //ROSコネクションの取得
        ros = ROSConnection.GetOrCreateInstance() ;

        //サブスクライバーの登録
        ros.Subscribe<StringMsg>("my_topic" , OnSubscribe) ;
    }

    //サブスクライブ時に呼ばれる
    void OnSubscribe(StringMsg msg)
    {
        Debug.Log("Subscribe :" + msg.data) ;
    }
}