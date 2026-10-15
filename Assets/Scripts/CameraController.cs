using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private Camera cam;

    private InputAction moveAction;
    private Vector2 moveValue;

    [SerializeField]
    private float moveSpeed;

    private InputAction zoomAction;
    private Vector2 zoomValue;

    [SerializeField]
    private float zoomSpeed;
    [SerializeField]
    private float zoomModifier;

    public static CameraController instance;

    void Awake()
    {
        instance = this;
        cam = Camera.main;

        // Safety: if the Inspector values were lost when the project was moved
        if (moveSpeed <= 0f) moveSpeed = 10f;
        if (zoomSpeed <= 0f) zoomSpeed = 5f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        zoomAction = InputSystem.actions.FindAction("Zoom");

        if (moveAction == null)
            Debug.LogWarning("CameraController: Input Action 'Move' not found - using WASD/arrow fallback. " +
                "Check Project Settings > Input System Package > Project-wide Actions.");
        if (zoomAction == null)
            Debug.LogWarning("CameraController: Input Action 'Zoom' not found - only Z/X keys will zoom.");
    }

    // Update is called once per frame
    void Update()
    {
        MoveByKB();
        ZoomInOut();
    }

    public void MoveByKB()
    {
        if (moveAction != null)
        {
            moveValue = moveAction.ReadValue<Vector2>();
        }
        else if (Keyboard.current != null)
        {
            moveValue = Vector2.zero;
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveValue.y += 1;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveValue.y -= 1;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveValue.x += 1;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveValue.x -= 1;
        }

        Vector3 dir = new Vector3(moveValue.x, moveValue.y, 0f);

        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    public void ZoomInOut()
    {
        zoomModifier = 0f;

        if (zoomAction != null)
        {
            zoomValue = zoomAction.ReadValue<Vector2>();
            zoomModifier = zoomValue.y * 0.05f;
        }

        if (Keyboard.current != null)
        {
            if (Keyboard.current.zKey.isPressed)
                zoomModifier = -0.005f;
            if (Keyboard.current.xKey.isPressed)
                zoomModifier = 0.005f;
        }

        cam.orthographicSize += zoomModifier * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 4, 10);
    }

    public void MoveCamera(Vector3 pos)
    {
        transform.position = new Vector3(pos.x, pos.y, -10f);
    }
}