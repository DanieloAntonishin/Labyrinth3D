using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private GameObject Sphere;      // ссылка на шарик
    private Vector3 cam_sphere;     // вектор камера-шарик
    private float mouseX;           // накопитель сдвигов мыши по X
    private float mouseY;           // mouse - Y
    private float zoom;             // приближение - отдаление камеры 

    private const float MAX_X_ANGLE = 70;     // предельные углы поворота
    private const float MIN_X_ANGLE = 30;     // камеры вокруг оси Х
    private const float SENSITIVITY_X = 2;    // чувствительность Y к мыши
    private const float SENSITIVITY_Y = 2;    // чувствительность X к мыши
    private const float MAX_ZOOM = 1.5f;      // предельные углы приближения
    private const float MIN_ZOOM = 0.0f;      // камеры к шарику
    private const float SENSI_ZOOM = 2;       // чувствительность к колесу мыши

    public float add = 0;


    void Start()
    {
        cam_sphere = this.transform.position - Sphere.transform.position;
        mouseY = this.transform.eulerAngles.x;
        zoom = 1;
    }

    private void Update()
    {
        // накопление данных о движении мыши
        mouseY -= SENSITIVITY_X * Input.GetAxis("Mouse Y") * Time.timeScale;
        if (mouseY < MIN_X_ANGLE) mouseY = MIN_X_ANGLE;
        if (mouseY > MAX_X_ANGLE) mouseY = MAX_X_ANGLE;
        mouseX += SENSITIVITY_Y * Input.GetAxis("Mouse X") * Time.timeScale;

        // поворот колеса мыши (scrolling)
        if (Input.mouseScrollDelta != Vector2.zero)
        {
            zoom -= Input.mouseScrollDelta.y / SENSI_ZOOM * Time.timeScale;
            if (zoom < MIN_ZOOM) zoom = MIN_ZOOM;
            if (zoom > MAX_ZOOM) zoom = MAX_ZOOM;
           
        }

    }

    void LateUpdate()
    {
        // пересчитываем позицию камеры относительно шарика
        // поворачиваем камеру

            this.transform.position = Sphere.transform.position +
            Quaternion.Euler(0, mouseX, 0) * cam_sphere * zoom;
            this.transform.eulerAngles = new Vector3(mouseY, mouseX, 0);
        

        //if(Input.mouseScrollDelta != Vector2.zero)
        //    Debug.Log(Input.mouseScrollDelta);
    }
}