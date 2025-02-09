using Mono.Cecil.Cil;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float minX = -4.2f;  // 최소 X값 (왼쪽)
    [SerializeField] private float maxX = 4.2f;   // 최대 X값 (오른쪽)
    [SerializeField] private GameObject muzzleFlashEffect; // 총구 화염 이펙트
    [SerializeField] private float fireRate = 0.1f; // 발사 간격 (초)

    private Animator anim;
    private Quaternion originalRotation; // 원래 회전값
    private bool isFiring = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        originalRotation = transform.rotation;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");  // ← / → 또는 A / D 입력 감지
        Vector3 moveTo = new Vector3(horizontalInput, 0f, 0f) * moveSpeed * Time.deltaTime;

        // 이동 적용
        transform.position += moveTo;

        // X축 위치를 minX와 maxX 범위 내로 제한
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

        // Animator의 Speed 값 변경 (애니메이션 자동 전환)
        float speedValue = Mathf.Abs(horizontalInput) * moveSpeed; 
        anim.SetFloat("Speed", speedValue);

        Vector3 position = transform.position;
        position.z = 13.43f; // Z값을 고정
        transform.position = position;

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

        // 자동 발사 로직
        if (!isFiring)
        {
            isFiring = true;
            InvokeRepeating("Fire", 0f, fireRate); // fireRate 간격으로 Fire 함수 호출
        }
    }

    void Fire()
    {
        // 총 발사 로직
        anim.SetTrigger("Shoot"); // Shoot 트리거 발동

        // 총구 화염 이펙트 재생
        if (muzzleFlashEffect != null)
        {
            muzzleFlashEffect.SetActive(true);
            // 잠시 후 이펙트 꺼짐
            Invoke("StopMuzzleFlash", 0.1f); // 이펙트가 켜진 후 일정 시간 후에 꺼짐
        }

        // 실제 총알 발사 로직 추가 (예: 총알 이동, 충돌 처리 등)
    }

    void StopMuzzleFlash()
    {
        muzzleFlashEffect.SetActive(false);
    }

    // 발사 멈추기
    public void StopFiring()
    {
        CancelInvoke("Fire");
        isFiring = false;
    }

    // 발이 지면에 닿을때마다 발생하는 이벤트
    public void OnFootstep()
    {
        Debug.Log("발소리 이벤트 호출됨!");
        // 여기에 발소리 추가 가능
        // AudioSource.PlayClipAtPoint(footstepSound, transform.position);
    }
}
