using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // プレイヤーのTransformを設定
    public Transform player;

    void Update()
    {
        // プレイヤーが設定されている場合に追従を実行
        if (player != null)
        {
            // プレイヤーの位置と同じ位置に設定
            transform.position = player.position;
        }
    }
}
