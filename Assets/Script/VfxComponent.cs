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
        if (prefab == null)
            return;

        Vector3 spawnPosition = vfxSpawnPoint != null ? vfxSpawnPoint.position : transform.position;
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
}