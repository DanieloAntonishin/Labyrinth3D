using System.Collections;               // Проблема - поменять привязку камеры
using System.Collections.Generic;       // к другой точке (не к центру сферы, а к
using UnityEngine;                      // ее вершине)
                                        // Варианты решений: 
public class CamPoint : MonoBehaviour   // - изменить скрипт - противоречит SOLID
{                                       // - поместить "опорную" точку в сферу - 
    [SerializeField]                    //    будет вращаться со сферой
    private GameObject Sphere;          // *ссылка на сферу
    private Vector3 shift;              // *сдвиг точки и сферы
    void Start()                        // - поместить сферу и "точку" в один ГО -
    {                                   //    движение сферы не "потянет" ГО, а 
        shift = transform.position      //    его передвижение надо корректировать, т.к.
            - Sphere.transform.position;//    координаты сферы будут считаться от него
    }                                   //
                                        // - создать опорную точку (ГО) отдельно и 
    void Update()                       //    обеспечить его следование за сферой,
    {                                   //    камеру привязать к опорному ГО
        transform.position =            // 
            Sphere.transform.position   // сдвигаемся на позицию сферы +
            + shift;                    //  сдвиг рассчитанный на старте
    }                                   // 
}                                       // 
                                        // 