using UnityEngine;
using System.Collections.Generic;

public class MissileTrail : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();

    public float pointSpacing = 0.05f;
    private Vector3 lastPoint;
    public bool isFrozen = false;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.widthMultiplier = 0.3f;
        lineRenderer.positionCount = 0;

        // ✅ 처음엔 점을 찍지 않고 위치만 기록
        lastPoint = transform.position;
    }

    void Update()
    {
        if (isFrozen) return;

        // ✅ 최소 두 점부터 궤적 생성
        if (points.Count == 0)
        {
            // 조건을 만족하면 첫 점 추가
            if (Vector3.Distance(transform.position, lastPoint) >= pointSpacing)
            {
                AddPoint(lastPoint);
                AddPoint(transform.position);
                lastPoint = transform.position;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, lastPoint) >= pointSpacing)
            {
                AddPoint(transform.position);
                lastPoint = transform.position;
            }
        }
    }

    void AddPoint(Vector3 point)
    {
        points.Add(point);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
}
