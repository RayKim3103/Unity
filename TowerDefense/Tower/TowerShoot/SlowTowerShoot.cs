using UnityEngine;

public class SlowTowerShoot : TowerShoot
{
    public float slowFactor = 0.5f; // 속도를 절반으로 줄임
    public float slowDuration = 3f; // 슬로우 지속 시간

    // 목표에 도달했을 때 호출되는 메소드를 재정의 (override)
    protected override void OnHitTarget()
    {
        // 기본 기능을 호출하여 데미지를 입힘
        base.OnHitTarget();

        // 목표 적의 EnemyMovement 컴포넌트를 가져옴
        EnemyMovement enemy = target.GetComponent<EnemyMovement>();

        // 목표 적이 EnemyMovement 컴포넌트를 가지고 있다면
        if (enemy != null)
        {
            // 적의 속도를 느리게 함
            enemy.ReduceSpeedTemporarily(slowFactor, slowDuration);
        }

        // 투사체 제거
        Destroy(gameObject);
    }
}