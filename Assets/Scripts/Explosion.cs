using UnityEngine;

public class MissileExplosion : MonoBehaviour
{
    public GameObject explosionEffect; // 이펙트 프리팹
    public float explosionRadius = 3f;
    public LayerMask targetLayer;

    void OnTriggerEnter(Collider other)
    {
        // 타겟과 부딪혔는지 확인 (Tag 사용)
        if (other.CompareTag("Target"))
        {
            Explode();
            Destroy(gameObject); // 미사일 파괴
        }
    }

    void Explode()
    {
        // 이펙트 생성
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // 옵션: 주변에 데미지 주기 등
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, targetLayer);
        foreach (Collider hit in hits)
        {
            Debug.Log("폭발 피해 대상: " + hit.name);
            // 여기서 데미지 처리 코드 삽입 가능
        }
    }
}
