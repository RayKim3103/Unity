using System.Collections;
using UnityEngine;

public class PoisonTowerShoot : TowerShoot
{
    public float damageInterval = 0.2f;  // 데미지를 줄 간격
    public float duration = 2f;  // 데미지를 주는 총 시간 (지속 시간)
    float elapsed = 0f;
    float coolDown = 0f;

    // 목표에 도달했을 때 호출되는 메소드를 재정의 (override)
    protected override void OnHitTarget()
    {
        // 목표 적의 EnemyMovement 컴포넌트를 가져옴
        EnemyMovement enemy = target.GetComponent<EnemyMovement>();

        coolDown += Time.deltaTime;
        // 목표 적이 EnemyMovement 컴포넌트를 가지고 있다면
        if (elapsed < duration && enemy != null && coolDown > damageInterval)
        {
            enemy.GetComponent<AttackEnemy>().TakeDamage(damage); 
            Debug.Log("Poisoned / Enemy Hp: " + enemy.GetComponent<AttackEnemy>().hp + "coolDown: " + coolDown);
            elapsed += damageInterval;
            coolDown = 0f;
        }
        else if (enemy == null || elapsed > duration)
        {
            // 투사체 제거
            Destroy(gameObject);
            Debug.Log("Poison Projectile Destroyed");
        }
    }
}