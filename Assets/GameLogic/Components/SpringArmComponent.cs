using UnityEngine;

public class SpringArmComponent : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float maxAngle = 90.0f;
    [SerializeField] private float rotationSpeed = 120.0f;
    [SerializeField] private float armLenght = 1.0f;
    private Transform armEnd;
    private Vector3 offset;
    private float yawRotation = 0.0f;
    private float pitchRotation = 0.0f;

    void Start()
    {
        if (!((transform.childCount > 0) && (armEnd = transform.GetChild(0))))
        {
            Debug.LogWarning("Spring arm don't have end");
            return;
        }

        if (target)
        {
            offset = transform.position - target.transform.position;
        }
        else
        {
            offset = transform.position;
            Debug.LogWarning("Spring arm target is not selected");
        }
    }

    void Update()
    {
        Move();
        UpdateArm();
    }

    private void Move()
    {
        if (!target) return;
        
        transform.position = offset + target.transform.position;
    }

    public void Rotate(Vector2 rotationDirection)
    {
        if (!target) return;

        float yawDirection = rotationDirection.x * Time.deltaTime * rotationSpeed;
        float pitchDirection = -rotationDirection.y * Time.deltaTime * rotationSpeed;

        yawRotation += yawDirection;
        pitchRotation = Mathf.Clamp(pitchRotation + pitchDirection, -maxAngle, maxAngle);

        transform.localRotation = Quaternion.Euler(pitchRotation, yawRotation, 0.0f);
    }

    public void SetTarget(GameObject newTarget)
    {
        if (!newTarget) Debug.LogWarning("Spring arm target is null");

        target = newTarget;
    }

    void UpdateArm()
    {
        if (!armEnd) return;

        Vector3 endDirection = (armEnd.position - transform.position).normalized;
        Vector3 endLocation = transform.position + endDirection * armLenght;

        Ray ray = new(transform.position, endDirection);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, armLenght)) endLocation = hitInfo.point;

        armEnd.position = endLocation;
    }
}
