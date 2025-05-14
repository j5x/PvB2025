using UnityEngine;

public class VfxComponent : MonoBehaviour
{
    [SerializeField] private Transform vfxSpawnPoint;
    
    public void PlayImpactVFX(GameObject vfxPrefab, Vector3 offset)
    {
        if (vfxPrefab == null || vfxSpawnPoint == null) return;

        GameObject vfxInstance = Instantiate(vfxPrefab);
        vfxInstance.transform.SetParent(vfxSpawnPoint, false);

        if (vfxInstance.TryGetComponent<RectTransform>(out RectTransform rect))
        {
            // Canvas-based
            rect.anchoredPosition = Vector2.zero + (Vector2)offset;
            rect.localRotation = Quaternion.identity;
        }
        else
        {
            // World-space
            vfxInstance.transform.localPosition = offset;
            vfxInstance.transform.localRotation = Quaternion.identity;
        }
    }

    public void PlayProjectileVFX(GameObject projectilePrefab, Vector3 offset, Transform target)
    {
        if (projectilePrefab == null || vfxSpawnPoint == null || target == null) return;

        GameObject projectile = Instantiate(projectilePrefab, vfxSpawnPoint.position + offset, Quaternion.identity);
        // Add projectile movement logic here if needed
    }
}