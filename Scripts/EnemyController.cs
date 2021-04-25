using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Animator animator;

    public bool isActionOngoing = false;
    public GameObject hitVFX;

    public UnitBattleStats unitStats;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(unitStats.hpCurrent <= 0)
        {
            if(isActionOngoing)
            {
                return;
            }
            isActionOngoing = true;
            animator.SetTrigger("dead");
            Destroy(gameObject,3f);
        }
    }

    public void TakeHit(float hpDamage, float spDamage)
    {
        animator.SetTrigger("hit");
        var effect = Instantiate(hitVFX, transform);
        Destroy(effect, 1f);
        unitStats.hpCurrent -= hpDamage;
        unitStats.hpCurrent -= spDamage;
    }
}
