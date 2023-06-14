using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossClass : MonoBehaviour
{
    [SerializeField]
    private float bossLife = 4f;
    [SerializeField]
    private float playerDamage = 2;
    Animator bossAnimator;

    [SerializeField]
    private float detectionRange;
    [SerializeField]
    private float currentDistance;

    private void Awake()
    {
        bossAnimator = transform.parent.GetComponent<Animator>();
    }

    public void RecibirDaño()    // ABSTRACTION
    {
        bossLife -= playerDamage;
        if (bossLife <= 0)
        {
            StartCoroutine(AnimateDeath());
        }
    }

    private IEnumerator AnimateDeath()  // ABSTRACTION
    {
        bossAnimator.SetBool("bossIsDead", true);
        yield return new WaitForSeconds(0);      
    }


}
