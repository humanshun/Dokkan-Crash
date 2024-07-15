using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSensor : MonoBehaviour {

    // プレイヤーコントローラへの参照
    public PlayerController m_root;

    // 初期化処理
    void Start()
    {
        // このオブジェクトのルートにあるPlayerControllerを取得
        m_root = this.transform.root.GetComponent<PlayerController>();
    }

    // 衝突点を保存するための配列
    ContactPoint2D[] contacts = new ContactPoint2D[1];

    // 他のコライダーがトリガーゾーン内にいるときに呼び出される
    void OnTriggerStay2D(Collider2D other)
    {
        // 衝突したオブジェクトが"Ground"または"Block"タグを持っている場合
        if (other.CompareTag("Ground") || other.CompareTag("Block"))
        {
            // オブジェクトが"Ground"タグを持っている場合
            if (other.CompareTag("Ground"))
            {
                // 地面判定用のフラグを立てる
                m_root.Is_DownJump_GroundCheck = true;
            }
            else
            {
                // ブロックの場合はフラグを下げる
                m_root.Is_DownJump_GroundCheck = false;
            }

            // プレイヤーの垂直速度が0以下の場合
            if (m_root.m_rigidbody.velocity.y <= 0)
            {
                // 地面に接地しているフラグを立て、ジャンプ回数をリセット
                m_root.isGrounded = true;
                m_root.currentJumpCount = 0;
            }
        }
    }

    // 他のコライダーがトリガーゾーンから出たときに呼び出される
    void OnTriggerExit2D(Collider2D other)
    {
        // 地面に接地していないフラグを立てる
        m_root.isGrounded = false;
    }
}
