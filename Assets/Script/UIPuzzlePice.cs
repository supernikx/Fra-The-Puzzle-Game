using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPuzzlePice : MonoBehaviour, ITouchable
{
    public float BackSpeed;

    GameManager gm;
    public static GameObject ItemBeingDragged;
    private Vector3 StartPosition;

    private float StayCounter;
    private bool CanDrag;
    private float StayDelay = 0.5f;
    private bool Backing;

    void Start()
    {
        gm = GameManager.instance;
        CanDrag = false;
        Backing = false;
        StayCounter = 0;
    }

    public void OnTouchDown(Vector2 ScreenPosition)
    {
        if (!Backing)
        {
            StartPosition = transform.position;
            Debug.Log("Touch " + gameObject.name);
        }
    }

    public void OnTouchExit()
    {
        if (CanDrag && !Backing)
        {
            ItemBeingDragged = null;
            StayCounter = 0f;
            CanDrag = false;
            StartCoroutine(LerpBackToStart());
        }
    }

    public void OnTouchMove(Vector2 ScreenPosition)
    {
        #region UnityEditor
#if UNITY_EDITOR
        if (!Backing)
        {
            StayCounter += Time.deltaTime;
            if (StayCounter >= StayDelay && ItemBeingDragged == null)
            {
                CanDrag = true;
                ItemBeingDragged = gameObject;
                GameManager.instance.ui.EnableScroll(false);
            }
        }
#endif
        #endregion
        if (CanDrag && !Backing)
        {
            transform.position = ScreenPosition;
        }
    }

    public void OnTouchStay(Vector2 ScreenPosition)
    {
        if (!Backing)
        {
            StayCounter += Time.deltaTime;
            if (StayCounter >= StayDelay && ItemBeingDragged == null)
            {
                CanDrag = true;
                ItemBeingDragged = gameObject;
                GameManager.instance.ui.EnableScroll(false);
            }
        }
    }

    public void OnTouchUp(Vector2 ScreenPosition)
    {
        if (CanDrag && !Backing)
        {
            if (gm.CheckDistance(Camera.main.ScreenToWorldPoint(transform.position)))
            {
                gameObject.SetActive(false);
                gm.ui.EnableScroll(true);
            }
            else
            {
                StartCoroutine(LerpBackToStart());
            }
        }

        ItemBeingDragged = null;
        StayCounter = 0f;
        CanDrag = false;
    }

    private IEnumerator LerpBackToStart()
    {
        Backing = true;
        Vector2 ActualPosition = transform.position;
        while ((transform.position - StartPosition).sqrMagnitude > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, StartPosition, Time.deltaTime * BackSpeed);
            yield return null;
        }
        transform.position = StartPosition;
        Backing = false;
        gm.ui.EnableScroll(true);
    }
}
