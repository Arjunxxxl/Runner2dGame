using UnityEngine;

public class ParallaxBG : MonoBehaviour
{
    [SerializeField] private float parallaxMul;
    [SerializeField] private GameObject cam;
    [SerializeField] private float repeatOffset;
    private Vector3 startPos;
    [SerializeField] private bool followInY;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        float dist = cam.transform.position.x * parallaxMul;
        transform.position = new Vector3(startPos.x + dist,
            followInY ? transform.position.y : startPos.y, 
            transform.position.z);
    }
}