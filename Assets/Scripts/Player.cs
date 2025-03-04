using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float minX = -4.2f;  // 최소 X값 (왼쪽)
    [SerializeField] private float maxX = 4.2f;   // 최대 X값 (오른쪽)
    [SerializeField] private Gun gun; // 총기

    private Animator anim;
    private Quaternion originalRotation; // 원래 회전값

    void Start()
    {
        anim = GetComponent<Animator>();
        originalRotation = transform.rotation;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleShooting();
    }

    void HandleShooting()
    {
        if (gun != null) // 총이 설정되어 있을 때만 발사 시작
        {
            gun.StartFiring(); // 자동 발사 로직 호출
        }
    }

    void HandleRotation()
    {
        float speed = anim.GetFloat("Speed");  // 현재 속도 가져오기

        if (Mathf.Approximately(speed, 0f)) // 정지 상태 → firing_rifle
        {
            transform.rotation = Quaternion.Euler(originalRotation.eulerAngles.x,
            originalRotation.eulerAngles.y + 37.5f,
            originalRotation.eulerAngles.z);
        }
        else if (speed > 0) // 이동 중 → firing_rifle_run
        {
            transform.rotation = Quaternion.Euler(originalRotation.eulerAngles.x,
            originalRotation.eulerAngles.y + 12.6f,
            originalRotation.eulerAngles.z);
        }
        else
        {
            // 원래 회전으로 복구
            transform.rotation = Quaternion.Euler(originalRotation.eulerAngles.x,
            originalRotation.eulerAngles.y,
            originalRotation.eulerAngles.z);
        }
    }

    void HandleMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector3 moveTo = new Vector3(horizontalInput, 0f, 0f) * moveSpeed * Time.deltaTime;

        transform.position += moveTo;

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

        anim.SetFloat("Speed", Mathf.Abs(horizontalInput) * moveSpeed);
    }

    // 발이 지면에 닿을때마다 발생하는 이벤트
    public void OnFootstep()
    {
        Debug.Log("발소리 이벤트 호출됨!");
        // 여기에 발소리 추가 가능
        // AudioSource.PlayClipAtPoint(footstepSound, transform.position);
    }
}
