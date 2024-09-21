using UnityEngine;

public class TowerTrigger : MonoBehaviour
{
    public TowerPlacementManager placementManager;
    private CheckTowerCollision checkTowerCollision;


    private void Awake() 
    {
        checkTowerCollision = placementManager.towerPrefab.GetComponent<CheckTowerCollision>();
    }

    void OnMouseDown()
    {
        Debug.Log("Object Clicked: " + gameObject.name);

        // 버튼 중복 클릭 방지
        if(placementManager.isPlacing != true)
        {
            // 아래 2코드, 순서 중요 -> isPlaced에 따라 CheckTowerCollision의 코드 동작 바뀜
            checkTowerCollision.isPlaced = false;
            placementManager.StartPlacingTower();
        }
        else
        {
            placementManager.DestroyPlacingTower();
        }

    }


}