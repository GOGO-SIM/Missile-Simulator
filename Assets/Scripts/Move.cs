using UnityEngine;

public class MissileHoming : MonoBehaviour
{
    public float speed = 20f;               // 전진 속도
    public float rotateSpeed = 5f;          // 회전 속도
    private Transform target;

    void Start()
    {
        // "Target" 태그가 붙은 오브젝트를 찾음
        GameObject targetObj = GameObject.FindGameObjectWithTag("Target");
        if (targetObj != null)
        {
            target = targetObj.transform;
            Debug.Log(target.name + " found and assigned as target.");
        }
    }

    void Update()
    {
        if (target == null) return;

        // 1. 타겟 방향 계산
        Vector3 direction = (target.position - transform.position).normalized;

        // 2. 회전 방향 보간 (자연스럽게 회전)
        Quaternion toRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);

        // 3. 앞으로 전진
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
