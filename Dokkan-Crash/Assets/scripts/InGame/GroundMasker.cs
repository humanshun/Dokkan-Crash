using UnityEngine;

public class GroundMasker : MonoBehaviour
{
    public LayerMask groundLayer; // 地面として扱うレイヤーマスク
    public string maskedGroundLayerName = "MaskedGround"; // マスクで消された地面のレイヤー名
    public float maskRadius; // マスクの半径

    private void Start()
    {
        // 初期化処理
        ApplyMaskToGround();
    }

    // マスクを適用する処理
    private void ApplyMaskToGround()
    {
        // マスクで消された部分のレイヤーを取得
        int maskedGroundLayer = LayerMask.NameToLayer(maskedGroundLayerName);

        // レイヤーが正しく設定されているか確認
        if (maskedGroundLayer == -1)
        {
            Debug.LogError($"レイヤー名 '{maskedGroundLayerName}' が無効です。正しいレイヤー名を設定してください。");
            return;
        }

        // マスク範囲内のすべての地面オブジェクトを取得
        Collider2D[] groundColliders = Physics2D.OverlapCircleAll(transform.position, maskRadius, groundLayer);

        foreach (Collider2D collider in groundColliders)
        {
            // マスクで消された部分のレイヤーを変更
            collider.gameObject.layer = maskedGroundLayer;
        }
    }

    private void OnDrawGizmos()
    {
        // Scene ビューでこの範囲を表示するための処理
        Gizmos.color = Color.blue; // ギズモの色を青に設定
        Gizmos.DrawWireSphere(transform.position, maskRadius); // 範囲をワイヤーフレームの球として描画
    }
}
