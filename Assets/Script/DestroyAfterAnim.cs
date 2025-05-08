using UnityEngine;

public class DestroyAfterAnim : MonoBehaviour
{
    private Animator animator;
    private float animationLength;

    void Awake()
    {
        animator = GetComponent<Animator>();
        // Assuming only 1 animation plays on trigger
        animationLength = animator.runtimeAnimatorController.animationClips[0].length;
    }

    void OnEnable()
    {
        Destroy(gameObject, animationLength);
    }
}
