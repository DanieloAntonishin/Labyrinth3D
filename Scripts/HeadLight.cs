using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeadLight : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    private Light lights;
    void Start()
    {
        lights = GetComponent<Light>();
    }

    void Update()
    {  
        transform.forward = cam.transform.forward + Vector3.up * 0.5f;
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && lights.range <= 10)
        {
            lights.range += Time.deltaTime*2; 
        }
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) && lights.range >= 0)
        {
            lights.range -= Time.deltaTime*2;
        }
    }
}
/* Освещение:
 * - направленный свет: не зависит от положения источника, создает
 *    наклонно падающий свет для всей сцены. Можно использовать как
 *    дневной свет либо как глобальные осветители.
 * - конусный свет (SpotLight): напоминает фонарик
 * - точечный свет: сферическая светимость.
 *     Используется с другими объектами, т.к. сам не имеет формы
 *     При этом для того чтобы реализовать светимость самого объекта
 *     используется эффект эмиссии материала объекта (сама эмиссия не
 *     подсвечивает другие объекты, поэтому комбинируется с другими
 *     источниками света).
 * Поверхности (Mesh) прозрачные изнутри, поэтому источники света можно
 *  помещать внутрь других объектов, если они будут снаружи, то будут
 *  перекрывать свет
 * Все виды света могут отбрасывать или не отбрасывать тени, тени
 *  могуть быть твердыми (с четкими контурами) и мягкими (с размытыми контурами)
 *  А также режимы освещения делятся на 
 *   realtime (пересчет с каждым кадром)
 *   baked (кешированный, рассчитанный заранее)
 *   mixed (смешанный)
 *   
 * Skybox: изменение неба
 * - находим а) cubemap - разветка куба с 6 видами б) отдельно 6 картинок
 * - импортируем в Assets - Textures
 * - меняем свойства (картинке) TextureType - default; TextureShape - Cube,
 *    сохраняем изменения (Apply) - смотрим на результат: должен получиться
 *    равномерный шар без видимых швов
 * - создаем материал, меняем в нем Shader - Skybox/Cubemap
 *    в свойствах материала выбираем текстуру, созданную на пред. шаге
 * - устанавливаем материал в свойства Skybox
 *    - menu Window - Rendering - Lighting
 *    - (tab) Environment - Skybox Material - выбираем наш материал
 *    
 *  Д.З. Реализовать вращение фонарика персонажа в направлении поворота
 *  камеры (по горизонтали, ** по вертикали, зажатие Shift усиливает свет)
 */