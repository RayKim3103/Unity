using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MinimapViewport : MonoBehaviour
{
    public Camera mainCamera;
    public Camera minimapCamera;
    public RectTransform viewportRect;

    void Update()
    {
        // ���� ī�޶��� ����Ʈ ũ�⸦ ��������
        float aspectRatio = mainCamera.aspect;
        float height = mainCamera.orthographicSize * 2;
        float width = height * aspectRatio;

        // �̴ϸ� ī�޶��� ���� ��ǥ ��ȯ
        Vector3 minimapCenter = minimapCamera.transform.position;
        Vector3 bottomLeft = new Vector3(minimapCenter.x - minimapCamera.orthographicSize * aspectRatio,
                                         minimapCenter.y - minimapCamera.orthographicSize,
                                         minimapCenter.z);
        Vector3 topRight = new Vector3(minimapCenter.x + minimapCamera.orthographicSize * aspectRatio,
                                       minimapCenter.y + minimapCamera.orthographicSize,
                                       minimapCenter.z);

        // ���� ī�޶��� ���� ��ǥ ��ȯ
        Vector3 cameraBottomLeft = mainCamera.transform.position - new Vector3(width / 2, 0, height / 2);
        Vector3 cameraTopRight = mainCamera.transform.position + new Vector3(width / 2, 0, height / 2);

        // �̴ϸ� ��ǥ�踦 �������� ����Ʈ ���簢�� ��ġ �� ũ�� ����
        Vector2 viewportMin = new Vector2((cameraBottomLeft.x - bottomLeft.x) / (topRight.x - bottomLeft.x),
                                          (cameraBottomLeft.z - bottomLeft.z) / (topRight.z - bottomLeft.z));
        Vector2 viewportMax = new Vector2((cameraTopRight.x - bottomLeft.x) / (topRight.x - bottomLeft.x),
                                          (cameraTopRight.z - bottomLeft.z) / (topRight.z - bottomLeft.z));

        // ����Ʈ ���簢���� ��Ŀ �� ũ�� ����
        viewportRect.anchorMin = viewportMin;
        viewportRect.anchorMax = viewportMax;
    }
}
