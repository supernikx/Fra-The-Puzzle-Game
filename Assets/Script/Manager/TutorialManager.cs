using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    Transform RightPoint;
    [SerializeField]
    Transform LeftPoint;
    [SerializeField]
    Image FirstImage;
    [SerializeField]
    Image SecondImage;
    [SerializeField]
    Image CenterImage;
    [SerializeField]
    List<Sprite> TutorialImages;
    int CurrentPageIndex;

    [SerializeField]
    float SwipeResistence;
    [SerializeField]
    float SwipeSpeed;
    Vector3 TouchInitialPosition;
    bool Moving;

    private void OnEnable()
    {       
        CurrentPageIndex = 0;
        CenterImage.sprite = TutorialImages[CurrentPageIndex];
        FirstImage.transform.position = RightPoint.position;
        SecondImage.transform.position = RightPoint.position;
        Moving = false;
    }

    /// <summary>
    /// Coroutine che si occupa di eseguire lo swipe verso Destra
    /// </summary>
    /// <returns></returns>
    IEnumerator OnRightSwipe()
    {
        if (CurrentPageIndex - 1 < 0)
            yield return null;
        else
        {
            Moving = true;
            CurrentPageIndex--;
            FirstImage.sprite = CenterImage.sprite;
            FirstImage.transform.position = CenterImage.transform.position;
            SecondImage.sprite = TutorialImages[CurrentPageIndex];
            SecondImage.transform.position = LeftPoint.transform.position;
            while (Vector2.Distance(FirstImage.transform.position, RightPoint.position) > 0.01f)
            {
                FirstImage.transform.position = Vector2.MoveTowards(FirstImage.transform.position, RightPoint.position, Time.deltaTime * SwipeSpeed);
                SecondImage.transform.position = Vector2.MoveTowards(SecondImage.transform.position, CenterImage.transform.position, Time.deltaTime * SwipeSpeed);
                yield return null;
            }

            CenterImage.sprite = TutorialImages[CurrentPageIndex];
            FirstImage.transform.position = RightPoint.position;
            SecondImage.transform.position = RightPoint.position;
            Moving = false;
        }
    }

    /// <summary>
    /// Coroutine che si occupa di eseguire lo swipe verso sinistra
    /// </summary>
    /// <returns></returns>
    IEnumerator OnLeftSwpe()
    {
        if (CurrentPageIndex + 1 > TutorialImages.Count - 1)
            yield return null;
        else
        {
            Moving = true;
            CurrentPageIndex++;
            FirstImage.sprite = CenterImage.sprite;
            FirstImage.transform.position = CenterImage.transform.position;
            SecondImage.sprite = TutorialImages[CurrentPageIndex];
            SecondImage.transform.position = RightPoint.transform.position;
            while (Vector2.Distance(FirstImage.transform.position, LeftPoint.position) > 0.01f)
            {
                FirstImage.transform.position = Vector2.MoveTowards(FirstImage.transform.position, LeftPoint.position, Time.deltaTime * SwipeSpeed);
                SecondImage.transform.position = Vector2.MoveTowards(SecondImage.transform.position, CenterImage.transform.position, Time.deltaTime * SwipeSpeed);
                yield return null;
            }

            CenterImage.sprite = TutorialImages[CurrentPageIndex];
            FirstImage.transform.position = LeftPoint.position;
            SecondImage.transform.position = LeftPoint.position;
            Moving = false;
        }
    }

    #region Input
    /// <summary>
    /// Funzione che registra la posizione in cui è iniziato il drag
    /// </summary>
    public void BeginDrag()
    {
        if (!Moving)
            TouchInitialPosition = Input.mousePosition;
    }

    /// <summary>
    /// Funzione che controlla se è stato eseguito un drag e la direzione verso cui è stato eseguito
    /// </summary>
    public void EndDrag()
    {
        if (!Moving)
        {
            Vector2 deltaSwipe = TouchInitialPosition - Input.mousePosition;
            if (Mathf.Abs(deltaSwipe.x) > SwipeResistence)
            {
                if (deltaSwipe.x < 0)
                {
                    StartCoroutine(OnRightSwipe());                    
                }
                else
                {
                    StartCoroutine(OnLeftSwpe());
                }                
            }
        }
    }
    #endregion
}
