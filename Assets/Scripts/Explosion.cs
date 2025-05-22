using Unity.VisualScripting;
using UnityEngine;

public class MissileExplosion : MonoBehaviour
{
    public GameObject explosionEffect;
    public float explosionRadius = 20f;
    public LayerMask targetLayer;
    public GameObject imuObject;
    public GameObject seekerObject;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            // 움직임 관련 스크립트 비활성화
            MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
            foreach (var script in scripts)
            {
                if (script != this) // 자기 자신 제외
                    script.enabled = false;
            }
            // 타겟 멈춤
            TargetMover targetMover = other.GetComponent<TargetMover>();
            if (targetMover != null)
            {
                targetMover.enabled = false;
            }

            // 궤적 멈춤
            MissileTrail trail = GetComponent<MissileTrail>();
            if (trail != null)
                trail.isFrozen = true;
            // 폭발 효과
            if (imuObject != null)
            {
                Destroy(imuObject);
            }
            if (seekerObject != null)
            {
                Destroy(seekerObject);
            }
            Explode();
        }
    }


    void Explode()
    {
        if (explosionEffect != null)
        {
            Vector3 explosionPosition = transform.position + transform.forward * 10.0f;
 
            Instantiate(explosionEffect, explosionPosition, Quaternion.identity);
        }
    }
}
