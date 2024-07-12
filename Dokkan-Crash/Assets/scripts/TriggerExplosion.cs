using UnityEngine;

public class TriggerExplosion : MonoBehaviour
{
    public Explosion explosion; // Explosion クラスのインスタンス（おそらくスクリプトで定義されている）
    public Vector2 explosionPosition; // 爆発の位置

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            explosion.Explode(explosionPosition); // E キーが押されたら Explosion クラスの Explode メソッドを呼び出す
        }
    }
}
