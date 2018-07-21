using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITouchable {

    void OnTouchDown(Vector2 ScreenPosition);
    void OnTouchUp(Vector2 ScreenPosition);
    void OnTouchStay(Vector2 ScreenPosition);
    void OnTouchMove(Vector2 ScreenPosition);
    void OnTouchExit();

}
