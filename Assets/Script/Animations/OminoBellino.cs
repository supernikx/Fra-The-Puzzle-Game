using System.Collections;
using UnityEngine;

public class OminoBellino : MonoBehaviour {

    public Animator animator;
    public float animSpeedUpDuration = 1.5f;

    float timer = 0;
    bool isTouched = false;

    public void OnOminoTouched()
    {
        print("Toccato l'omino");
        if (animator && !isTouched)
        {
            animator.SetTrigger("SpeedUp");
            isTouched = true;
            StartCoroutine(TimeCounter());
        }
    }

    IEnumerator TimeCounter()
    {
        print("Partita speedUp");
        while (timer < animSpeedUpDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        animator.SetTrigger("SpeedDown");
        isTouched = false;
    }
}
