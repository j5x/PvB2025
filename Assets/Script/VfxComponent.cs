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

    public void PlayImpactVFX(GameObject prefab, bool forceScenePlayerImpactPoint = false)
    {
        if (prefab == null)
            return;

        Transform spawnPoint = vfxSpawnPoint;

        // If enemy is hitting player, use scene-level PlayerImpactPoint
        if (forceScenePlayerImpactPoint)
        {
            GameObject overridePoint = GameObject.FindWithTag("PlayerImpactPoint");
            if (overridePoint != null)
            {
                spawnPoint = overridePoint.transform;
            }
            else
            {
                Debug.LogWarning("PlayerImpactPoint tag not found in scene.");
            }
        }

        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
    
}