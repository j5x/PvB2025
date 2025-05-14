using UnityEngine;

public class DestroyAfterAnim : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfo.Length > 0)
        {
            float animLength = clipInfo[0].clip.length;
            Destroy(gameObject, animLength);
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: No animation playing on Animator. Destroying after fallback 1s.");
            Destroy(gameObject, 1f);
        }
    }
}