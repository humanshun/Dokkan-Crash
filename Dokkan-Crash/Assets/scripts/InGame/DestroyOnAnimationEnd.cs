using UnityEngine;

public class DestroyOnAnimationEnd : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        // アニメーターコンポーネントを取得
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // アニメーションが再生中かをチェックし、終了したらオブジェクトを破壊
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f &&
            !animator.IsInTransition(0))
        {
            Destroy(gameObject); // 自分自身を破壊
        }
    }
}
