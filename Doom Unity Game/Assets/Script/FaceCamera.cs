using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    void Update()
    {
        Vector3 camPos = Camera.main.transform.position;        
        Vector3 lookDir = camPos - transform.position;
       
        lookDir.y = -90;
       
        transform.rotation = Quaternion.LookRotation(lookDir);
    }
}
