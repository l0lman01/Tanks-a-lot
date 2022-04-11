using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CameraSO cameraSO;

    public float SpeedCamera = 5f;
    public float BorderThickness = 10f;
    public Vector2 Limit;
    public float ScrollSpeed = 50f;
    public float minScroll = 1f;
    public float maxScroll = 10f;


    private void Awake()
    {
        cameraSO.camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;

        Vector3 pos = transform.position;

        if (Input.GetKey("z") || Input.mousePosition.y >= Screen.height - BorderThickness)
        {
            pos.y += SpeedCamera * Time.deltaTime;
        }
        if (Input.GetKey("q") || Input.mousePosition.x <= BorderThickness)
        {
            pos.x -= SpeedCamera * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= BorderThickness)
        {
            pos.y -= SpeedCamera * Time.deltaTime;

        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - BorderThickness)
        {
            pos.x += SpeedCamera * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize -= scroll * ScrollSpeed * 100f * Time.deltaTime;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minScroll, maxScroll);

        pos.x = Mathf.Clamp(pos.x, -Limit.x, Limit.x);
        pos.y = Mathf.Clamp(pos.y, -Limit.y, Limit.y);

        transform.position = pos;
    }
}
