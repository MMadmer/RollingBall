using UnityEngine;
using UnityEngine.SceneManagement;
using GlobalFunc;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1500.0f;
    public SpringArmComponent springArm;
    private PlayerInput input;
    private Rigidbody rb;
    private Collider mainCollider;

    private void Awake()
    {
        input = new PlayerInput();

        input.BaseControl.Restart.performed += context => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        if (!rb) Debug.LogWarning("Player has no rigid body");

        mainCollider = GetComponent<Collider>();
        if (!mainCollider) Debug.LogWarning("Player has no collider");
    }

    private void Update()
    {
        Move(input.BaseControl.Move.ReadValue<Vector2>());
        CameraControl();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
    
    private void Move(Vector2 direction)
    {
        if (!rb || !GlobalFunctions.OnGround(mainCollider, 0.0f)) return;

        Vector3 moveDirection = new(direction.x, 0.0f, direction.y);
        if (springArm) moveDirection = Quaternion.AngleAxis(springArm.transform.eulerAngles.y, Vector3.up) * moveDirection;

        rb.AddForce(moveDirection * (moveSpeed * Time.deltaTime));
    }

    private void CameraControl()
    {
        if (!springArm) return;

        springArm.Rotate(input.BaseControl.Camera.ReadValue<Vector2>());
    }
}
