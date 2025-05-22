using UnityEngine;

public class MissileHoming : MonoBehaviour
{
    public float speed = 20f;          
    public float rotateSpeed = 5f;     
    private Transform target;

    void Start()
    {
        var tgt = GameObject.FindGameObjectWithTag("Target");
        if (tgt) target = tgt.transform;
    }

    void FixedUpdate()
    {
        if (target == null) return;

        // 1) 천천히 회전
        Vector3 dir     = (target.position - transform.position).normalized;
        Quaternion dstR = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, dstR, rotateSpeed * Time.fixedDeltaTime);

        // 2) 고정 속도로 전진
        Vector3 move = transform.forward * speed * Time.fixedDeltaTime;
        transform.position += move;
    }
}
