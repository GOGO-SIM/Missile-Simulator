using UnityEngine;

public class MissileExplosion : MonoBehaviour
{
    public GameObject explosionEffect;
    public float explosionRadius = 3f;
    public LayerMask targetLayer;

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

            // 궤적 멈춤
            MissileTrail trail = GetComponent<MissileTrail>();
            if (trail != null)
                trail.isFrozen = true;
            // 폭발 효과
            Explode();
        }
    }


    void Explode()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
    }
}
