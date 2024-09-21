using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawAttackRange : MonoBehaviour
{
    public float radius = 2f;  // 공격 범위 반경
    public int segments = 50;  // 원을 그릴 때 사용할 세그먼트 수
    public Material lineMaterial;  // 사용할 Material, Default-Particle로 설정
    public float lineWidth = 0.2f;  // 라인의 굵기

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = false;
        // LineRenderer의 Material을 Default-Particle로 설정
        lineRenderer.material = lineMaterial;
        // 라인의 굵기 설정
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        // 라인의 색상 및 알파 값 설정
        Color lineColor = new Color(1f, 0f, 0f, 150f / 255f);  // 빨간색, 알파 값은 200/255
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        // Debug.Log("LineRenderer Start: " + lineRenderer);
        DrawCircle();
    }

    void DrawCircle()
    {
        float angle = 0f;
        // Debug.Log("LineRenderer: " + lineRenderer);
        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            lineRenderer.SetPosition(i, new Vector3(x, y, 0f));

            angle += (360f / segments);
        }
    }

    // 범위가 변경될 때, 원의 크기를 업데이트할 수 있음
    public void UpdateCircle(float newRadius)
    {
        radius = newRadius;
        DrawCircle();
    }
}

