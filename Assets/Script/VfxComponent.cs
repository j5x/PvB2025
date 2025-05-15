using UnityEngine;

public class VfxComponent : MonoBehaviour
{
    [SerializeField] private Transform vfxSpawnPoint;
    
    public void SetVfxSpawnPoint(Transform point)
    {
        vfxSpawnPoint = point;
    }
    
    public void PlayAttackVFX(GameObject prefab)
    {
        if (prefab != null && vfxSpawnPoint != null)
            Instantiate(prefab, vfxSpawnPoint.position, Quaternion.identity);
    }

    public void PlayImpactVFX(GameObject prefab)
    {
        if (prefab != null)
            Instantiate(prefab, transform.position, Quaternion.identity);
    }

    public void PlayProjectileVFX(GameObject projectilePrefab, Vector3 offset, Transform target)
    {
        if (projectilePrefab == null || vfxSpawnPoint == null || target == null) return;

        GameObject projectile = Instantiate(projectilePrefab, vfxSpawnPoint.position + offset, Quaternion.identity);
        // Add projectile movement logic here if needed
    }
}