using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallaxMul;
    [SerializeField] private GameObject cam;
    [SerializeField] private float repeatOffset;
    private Vector3 camPos;

    // Start is called before the first frame update
    void Start()
    {
        camPos = cam.transform.position;
    }

    void LateUpdate()
    {
        Vector3 camMovement = cam.transform.position - camPos;
        transform.position += camMovement * parallaxMul;
        camPos = cam.transform.position;

        if(Mathf.Abs(cam.transform.position.x - transform.position.x) >= repeatOffset)
        {
            float offsetX = (cam.transform.position - transform.position).x % repeatOffset;
            transform.position = new Vector3(cam.transform.position.x + offsetX, transform.position.y, transform.position.z);
        }
    }
}
