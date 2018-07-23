using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPuzzlePice : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public float BackSpeed;

    GameManager gm;
    public static GameObject ItemBeingDragged;
    public Vector3 StartPosition;

    private float StayCounter;
    private bool CanDrag;
    private float StayDelay = 0.5f;
    private bool Backing;
    private Transform DefaultParent;

    void Start()
    {
        gm = GameManager.instance;
        CanDrag = false;
        Backing = false;
        StayCounter = 0;
    }

    public void OnTouchStay(Vector2 ScreenPosition)
    {
        if (!Backing)
        {
            StayCounter += Time.deltaTime;
            if (StayCounter >= StayDelay && ItemBeingDragged == null)
            {
                StartPosition = transform.position;
                CanDrag = true;
            }
        }
    }

    private IEnumerator LerpBackToStart()
    {
        gm.ui.UpDownInventory();
        Backing = true;
        float AnimationCounterDuration = 0f;
        while ((transform.position - StartPosition).sqrMagnitude > 0.01f || AnimationCounterDuration < 0.35f)
        {
            transform.position = Vector2.MoveTowards(transform.position, StartPosition, Time.deltaTime * BackSpeed);
            AnimationCounterDuration += Time.deltaTime;
            yield return null;
        }
        transform.position = StartPosition;
        Backing = false;
        gm.ui.EnableScroll(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (CanDrag && !Backing)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (CanDrag)
        {
            ItemBeingDragged = gameObject;
            gm.ui.EnableScroll(false);
            gm.ui.UpDownInventory();
        }
    }

    private void OnMouseDrag()
    {
        if (!Backing)
        {
            StayCounter += Time.deltaTime;
            if (StayCounter >= StayDelay && ItemBeingDragged == null)
            {
                StartPosition = transform.position;
                CanDrag = true;
            }
        }
    }
}
