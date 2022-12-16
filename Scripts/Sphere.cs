using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderData;

public class Sphere : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Rigidbody rb;
    private AudioSource soundHitWall;
    private AudioSource soundHitGate;
    private AudioSource soundCollectPoint;
    private AudioSource soundFinishPoint;
  
    private Vector3 forceDirection;
    private const float FORCE_MAGNITUDE = 2;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        AudioSource[] sounds = GetComponents<AudioSource>(); // массив компонентов
        soundHitWall = sounds[0];
        soundHitGate = sounds[1];
        soundCollectPoint = sounds[2];
        soundFinishPoint = sounds[3];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           if(rb.velocity.y == 0) rb.AddForce(Vector3.up * 200);
        }
        float dx = Input.GetAxis("Horizontal");
        float dy = Input.GetAxis("Vertical");        // стрелки вверх, вниз управляют по Z
        // rb.AddForce(new Vector3 (dx, 0, dy) * FORCE_MAGNITUDE);    // в векторе силы Y -> Z
        
        // корректируем на поворот камеры: вперед - это куда смотрит камера 
        //rb.AddForce(cam.transform.forward * dy * FORCE_MAGNITUDE); // вращение при движении 
        forceDirection = cam.transform.forward;                                  // forward камеры наклонен вниз, усилия вдавливают/поднимают шар
        forceDirection.y = 0;                                                    // убираем вертикальную состовляющую вектора, но тогда его длинна сокращается
        forceDirection = forceDirection.normalized                               // длинна увеличивается. normalized - сохроняет направление и задает длинну вектору 1
            * dy * FORCE_MAGNITUDE;                                              // умножаем на ввод пользователя 
        forceDirection += cam.transform.right                                    // right камеры не наклонен по Y, поэтому
            * dx * FORCE_MAGNITUDE;                                              // коррекция не нужна
        rb.AddForce(forceDirection);                                             //
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "CheckPoint1")
        {
            other.gameObject.SetActive(false);
            GameStat.CheckImage1Checked(true);
            GameObject.Find("Gates1").gameObject.SetActive(false);
        }
        if(other.name == "CheckPoint2")
        {
            other.gameObject.SetActive(false);
            GameStat.CheckImage2Checked(true);
            GameObject.Find("Gates2").gameObject.SetActive(false);
        }
        if (other.name == "CheckPoint3")
        {
            Debug.Log("You found the exit, oh no it`s trap)");
            FinalGate.isActivated = true;
            other.gameObject.SetActive(false);
            GameStat.CheckImage3Checked(true);
            GameMenu.FinishContent.SetActive(true);
        }

        // Установки (настройки) звука берем из меню
        if (GameMenu.isSoundsEnabled)
        {
            AudioSource sound = other.gameObject.tag switch
            {
                "GateButton" => soundCollectPoint,
                "Finish" => soundFinishPoint,
                _ => null
            };
            if (sound is null) return;

            sound.volume = GameMenu.soundsVolume;
            sound.Play();
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        // Установки (настройки) звука берем из меню
        if (GameMenu.isSoundsEnabled)
        {
            AudioSource sound = other.gameObject.tag switch
            {
                "Wall" => soundHitWall,
                "Gate" => soundHitGate,
               
                _ => null
            };
            if (sound is null) return;

            sound.volume = GameMenu.soundsVolume;
            sound.Play();
        }
    }
}
