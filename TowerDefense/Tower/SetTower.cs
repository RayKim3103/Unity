using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTower : MonoBehaviour
{
    public Button[] buttonsPosition;
    public Button[] buttonsPrefabs;
    public GameObject[] totalTowerPrefabs;
    
    // [SerializeField] // 디버깅을 위해서 직렬화 
    private GameObject[] towerPrefabs = new GameObject[3];
    public GameObject[] points;
    public int buttonsPositionCount = 0;
    public GameObject towerPrefabSelectPanel; // 다른 특정 패널을 띄울 때 사용할 패널
    
    /* TowerPlace 게임 오브젝트에 스크립트 할당 */
    /* 이 스크립트는 각각 배열의 index가 서로 연관되어 있어야합니다 */
    // buttonsPosition 과 points
    // buttonsPrefabs 과 towerPrefabs
    void Start()
    {
        // 메인 메뉴에서 선택한 tower를 반영하기 위해 towerPrefabs를 재설정
        int count = Mathf.Min(GameManager.Instance.purchasedTowers.Count, towerPrefabs.Length);
        for (int i = 0; i < count; i++)
        {
            int towerIndex = (int)GameManager.Instance.purchasedTowers[i];
            if (towerIndex < totalTowerPrefabs.Length)
            {
                towerPrefabs[i] = totalTowerPrefabs[towerIndex];
            }
        }

        // 각 위치 버튼에 대해 클릭 이벤트를 추가합니다.
        for (int i = 0; i < buttonsPosition.Length; i++)
        {
            int index = i; // 클로저 문제를 해결하기 위해 인덱스 값을 저장합니다.
            buttonsPosition[i].onClick.AddListener(() => OnButtonClickTowerPosition(index));
        }

        // 패널의 각 프리팹 버튼에 대해 클릭 이벤트를 추가합니다.
        for (int i = 0; i < buttonsPrefabs.Length; i++)
        {
            int index = i; // 클로저 문제를 해결하기 위해 인덱스 값을 저장합니다.
            buttonsPrefabs[i].onClick.AddListener(() => OnButtonClickTowerPrefab(index));
        }
    }

    public void OnButtonClickTowerPosition(int index)
    {
        // 버튼이 클릭되었을 때 호출되는 함수입니다.
        buttonsPositionCount = index; // 버튼의 인덱스를 buttonCount에 저장합니다.
        if(points[index].GetComponentInChildren<Tower>() != null)
        {
            Debug.Log("이미 포탑이 설치된 지역입니다!");
            return;
        }

        // 다른 특정 패널을 활성화합니다.
        if (towerPrefabSelectPanel != null)
        {
            towerPrefabSelectPanel.SetActive(true);
        }
    }

    public void OnButtonClickTowerPrefab(int prefabIndex)
    {
        // 프리팹 버튼이 클릭되었을 때 호출되는 함수
        int count = Mathf.Min(GameManager.Instance.purchasedTowers.Count, towerPrefabs.Length);
        if (buttonsPositionCount >= 0 && buttonsPositionCount < points.Length && prefabIndex >= 0 && prefabIndex < count)
        {
            SpawnTower(prefabIndex);
        }
        // 선택한 타워가 없을 때 Basic Tower를 기본제공
        else if (count == 0 && prefabIndex == 0)
        {
            towerPrefabs[0] = totalTowerPrefabs[1];
            SpawnTower(prefabIndex);
        }
    }

    private void SpawnTower(int prefabIndex)
    {
        // points 배열의 특정 위치에 해당 프리팹을 생성합니다.
        Vector3 spawnPosition = points[buttonsPositionCount].transform.position;
        GameObject placedTower = Instantiate(towerPrefabs[prefabIndex], spawnPosition, Quaternion.identity);

        // placedTower의 부모를 points 배열의 해당 인덱스에 해당하는 게임 오브젝트로 설정합니다.
        placedTower.transform.parent = points[buttonsPositionCount].transform;

        // 설치된 타워의 Tower 스크립트를 가져와서 활성화
        Tower towerComponent = placedTower.GetComponent<Tower>();
        if (towerComponent != null)
        {
            towerComponent.ActivateTower();
        }

        // 패널을 비활성화합니다.
        if (towerPrefabSelectPanel != null)
        {
            towerPrefabSelectPanel.SetActive(false);
            InGameUIManager.instance.CloseTowerSelectButtons();
            InGameUIManager.instance.CloseTowerSelectPanel();
        }
    }
}
