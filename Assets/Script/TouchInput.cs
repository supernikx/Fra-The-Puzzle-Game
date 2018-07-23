using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchInput : MonoBehaviour
{
    private List<GameObject> TouchList = new List<GameObject>();
    private GameObject[] OldTouchList;
    private GraphicRaycaster raycaster;

    private void Start()
    {
        raycaster = GetComponent<GraphicRaycaster>();
    }

    void Update()
    {
        #region UnityEditor
#if UNITY_EDITOR
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            OldTouchList = new GameObject[TouchList.Count];
            TouchList.CopyTo(OldTouchList);
            TouchList.Clear();

            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);
            GameObject recipient = null;
            int resultindex = 0;
            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].gameObject.layer == LayerMask.NameToLayer("TouchInput"))
                {
                    recipient = results[i].gameObject;
                    resultindex = i;
                    break;
                }
            }

            if (recipient != null)
            {
                TouchList.Add(recipient);
                if (Input.GetMouseButtonDown(0))
                {
                    recipient.SendMessage("OnTouchDown", results[resultindex].screenPosition, SendMessageOptions.DontRequireReceiver);
                }
                if (Input.GetMouseButton(0))
                {
                    recipient.SendMessage("OnTouchStay", results[resultindex].screenPosition, SendMessageOptions.DontRequireReceiver);
                }
                if (Input.GetMouseButtonUp(0))
                {
                    recipient.SendMessage("OnTouchUp", results[resultindex].screenPosition, SendMessageOptions.DontRequireReceiver);
                }
            }

            foreach (GameObject obj in OldTouchList)
            {
                if (!TouchList.Contains(obj))
                {
                    obj.SendMessage("OnTouchExit", SendMessageOptions.DontRequireReceiver);
                }
            }
        }
#endif
        #endregion

        if (Input.touchCount > 0)
        {
            OldTouchList = new GameObject[TouchList.Count];
            TouchList.CopyTo(OldTouchList);
            TouchList.Clear();

            foreach (Touch touch in Input.touches)
            {
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                pointerData.position = touch.position;
                List<RaycastResult> results = new List<RaycastResult>();

                raycaster.Raycast(pointerData, results);
                GameObject recipient = null;
                int resultindex = 0;
                for (int i = 0; i < results.Count; i++)
                {
                    if (results[i].gameObject.layer == LayerMask.NameToLayer("TouchInput"))
                    {
                        recipient = results[i].gameObject;
                        resultindex = i;
                        break;
                    }
                }

                if (recipient != null)
                {
                    TouchList.Add(recipient);

                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            recipient.SendMessage("OnTouchDown", results[resultindex].screenPosition, SendMessageOptions.DontRequireReceiver);
                            break;
                        case TouchPhase.Moved:
                            recipient.SendMessage("OnTouchMove", results[resultindex].screenPosition, SendMessageOptions.DontRequireReceiver);
                            break;
                        case TouchPhase.Stationary:
                            recipient.SendMessage("OnTouchStay", results[resultindex].screenPosition, SendMessageOptions.DontRequireReceiver);
                            break;
                        case TouchPhase.Ended:
                            recipient.SendMessage("OnTouchUp", results[resultindex].screenPosition, SendMessageOptions.DontRequireReceiver);
                            break;
                        case TouchPhase.Canceled:
                            recipient.SendMessage("OnTouchExit", results[resultindex].screenPosition, SendMessageOptions.DontRequireReceiver);
                            break;
                    }
                }

                foreach (GameObject obj in OldTouchList)
                {
                    if (!TouchList.Contains(obj))
                    {
                        obj.SendMessage("OnTouchExit", SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }
    }
}
