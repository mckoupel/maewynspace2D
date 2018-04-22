using UnityEngine;

/// <summary>
/// Отлавливает нажатия клавиатуры и тача, выполняет небольшие действия над объектами
/// </summary>
public class InputManager : MonoBehaviour
{
    SpaceGeneral spaceGeneral;

    /// <summary>
    /// Скрипт, управляющий бэкграундом с космосом
    /// </summary>
    public BackgraundController backgroundSpace;
    /// <summary>
    /// Скрипт, управляющий бэкграундом со звездами
    /// </summary>
    public BackgraundController backgroundStars;

    /// <summary>
    /// Космический корабль
    /// </summary>
    public RectTransform ship;

    /// <summary>
    /// Базовое смещение бэкграунда за одну ячейку
    /// </summary>
    public float baseOffset = 0.005f;

   
    private float newDistance;
    private float initialDistance;

    private void Start()
    {
        spaceGeneral = GameObject.FindObjectOfType<SpaceGeneral>();
    }

    void Update ()
    {
        // изменение скейла бэкграундов
        backgroundStars.ScaleUV(Mathf.Clamp(0.1f + spaceGeneral.cameraManager.camera.orthographicSize / 100, 0f, 1.8f));
        backgroundSpace.ScaleUV(Mathf.Clamp(0.1f + spaceGeneral.cameraManager.camera.orthographicSize / 100, 0, 1.8f));

        #region mousescroll

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            spaceGeneral.cameraManager.ChangeZoomTarget(scroll);
        }

        #endregion

        #region touchscroll

        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);
            Vector2 prevTouchPosition0 = touch0.position - touch0.deltaPosition;
            Vector2 prevTouchPosition1 = touch1.position - touch1.deltaPosition;
            float touchDistance = (touch1.position - touch0.position).magnitude;
            float prevTouchDistance = (prevTouchPosition1 - prevTouchPosition1).magnitude;
            float touchChangeMultiplier = touchDistance / prevTouchDistance;

            spaceGeneral.cameraManager.ChangeZoomTarget(touchChangeMultiplier);
        }

         // if the second touch just started
        if (Input.touchCount == 2 && Input.GetTouch(1).phase == TouchPhase.Began)
        {
             Vector2 touch1 = Input.GetTouch(0).position;
             Vector2 touch2 = Input.GetTouch(1).position;

             // save the initial distance
             initialDistance = (touch1 - touch2).sqrMagnitude;

        }

        if (Input.touchCount >= 2 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved))
        {
             Vector2 touch1 = Input.GetTouch(0).position;
             Vector2 touch2 = Input.GetTouch(1).position;

             newDistance = (touch1 - touch2).sqrMagnitude;
             float changeInDistance = newDistance - initialDistance;
             float percentageChange = changeInDistance / initialDistance;

             spaceGeneral.cameraManager.ChangeZoomTarget(percentageChange);
        }


        #endregion

        #region moving

        // сдвиг фона, поворот корабля
        if (Input.GetKey(KeyCode.D) || (Input.touchCount == 1 && Input.GetTouch(0).position.x > Screen.width - 200))
        {
            spaceGeneral.GoToNeibCell(new Vector3Int(1, 0, 0));

            backgroundStars.MoveUV(new Vector2(baseOffset / 5, 0));
            backgroundSpace.MoveUV(new Vector2(baseOffset, 0));

            ship.localEulerAngles = new Vector3(0, 0, -90);
        }

        if (Input.GetKey(KeyCode.A) || (Input.touchCount == 1 && Input.GetTouch(0).position.x < 200))
        {
            spaceGeneral.GoToNeibCell(new Vector3Int(-1, 0, 0));

            backgroundStars.MoveUV(new Vector2(-baseOffset / 5, 0));
            backgroundSpace.MoveUV(new Vector2(-baseOffset, 0));

            ship.localEulerAngles = new Vector3(0, 0, 90);
        }

        if (Input.GetKey(KeyCode.W) || (Input.touchCount == 1 && Input.GetTouch(0).position.y > Screen.height - 200))
        {
            spaceGeneral.GoToNeibCell(new Vector3Int(0, 1, 0));

            backgroundStars.MoveUV(new Vector2(0, baseOffset / 5));
            backgroundSpace.MoveUV(new Vector2(0, baseOffset));

            ship.localEulerAngles = new Vector3(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.S) || (Input.touchCount == 1 && Input.GetTouch(0).position.y < 200))
        {
            spaceGeneral.GoToNeibCell(new Vector3Int(0, -1, 0));

            backgroundStars.MoveUV(new Vector2(0, -baseOffset / 5f));
            backgroundSpace.MoveUV(new Vector2(0, -baseOffset));

            ship.localEulerAngles = new Vector3(0, 0, 180);
        }

        #endregion


    }
}
