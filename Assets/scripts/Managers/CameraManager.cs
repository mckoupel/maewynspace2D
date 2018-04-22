using UnityEngine;

/// <summary>
/// Скрипт, отвечающий за камеру
/// </summary>
public class CameraManager : MonoBehaviour
{
    public SpaceGeneral spaceGeneral;
    public Camera camera;

    public float zoomSpeed = 1;
    public float zoomTarget = 10;

    public float minOrtho = 1.0f;
    public float maxOrtho = 10000.0f;

    public float smoothSpeed = 4.0f;

    private void Start()
    {
        spaceGeneral = GameObject.FindObjectOfType<SpaceGeneral>();
        camera = this.GetComponent<Camera>();
    }

    void Update()
    {
        // ускоряем скролл на определенном этапе
        if (camera.orthographicSize > 20)
        {
            smoothSpeed = 30;
            zoomSpeed = 300;
        }
        else
        {
            smoothSpeed = 10;
            zoomSpeed = 10;
        }

        // движемся в сторону zoomTarget
        camera.orthographicSize = Mathf.Clamp(Mathf.MoveTowards(Camera.main.orthographicSize, zoomTarget, smoothSpeed * Time.deltaTime), spaceGeneral.spaceDB.minCellCount.x / 2, spaceGeneral.spaceDB.maxCellCount.x / 2);
    }

    /// <summary>
    /// Принимает новое значение зума, к которому необходимо двигаться
    /// </summary>
    public void ChangeZoomTarget(float value)
    {
        zoomTarget = Camera.main.orthographicSize - value * zoomSpeed;
        zoomTarget = Mathf.Clamp(zoomTarget, minOrtho, maxOrtho);
    }

}
