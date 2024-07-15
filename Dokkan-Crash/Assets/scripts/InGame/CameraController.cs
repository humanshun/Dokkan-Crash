using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    // グローバルアクセス用の静的インスタンス
    public static CameraController Instance;

    // カメラが追従するターゲットオブジェクト
    public GameObject Target;
    
    // カメラの追従スムーズ値
    public int Smoothvalue = 2;
    
    // カメラのY軸位置調整
    public float PosY = 1;

    // コルーチンの参照
    public Coroutine my_co;

    // 初期化処理
    void Start()
    {
        // 特に初期化は不要
    }

    // 毎フレーム更新処理
    void Update()
    {
        // ターゲットの位置を取得し、カメラの新しい位置を計算
        Vector3 Targetpos = new Vector3(Target.transform.position.x, Target.transform.position.y + PosY, -100);
        
        // カメラの位置をスムーズにターゲット位置に移動
        transform.position = Vector3.Lerp(transform.position, Targetpos, Time.deltaTime * Smoothvalue);
    }
}
