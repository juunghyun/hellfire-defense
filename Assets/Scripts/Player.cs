using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float minX = -4.2f;  // 최소 X값 (왼쪽)
    [SerializeField] private float maxX = 4.2f;   // 최대 X값 (오른쪽)
    
    private Animator anim;
    private bool isMousePressed = false; // 마우스가 눌린 상태인지 여부

    void Start()
    {
        anim = GetComponent<Animator>();
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

        // 마우스 왼쪽 버튼 클릭을 감지(넌클릭 상태에서 클릭댐)
        if (Input.GetMouseButtonDown(0) && !isMousePressed)  // 마우스 왼쪽 버튼을 클릭하면
        {
            anim.SetBool("Shoot", true); // Shoot 애니메이션 시작
            isMousePressed = true; // 마우스가 눌린 상태로 변경
        }

        // 마우스 왼쪽 버튼을 떼면
        if (Input.GetMouseButtonUp(0) && isMousePressed)
        {
            anim.SetBool("Shoot", false); // Shoot 애니메이션 종료
            isMousePressed = false; // 마우스가 떼어진 상태로 변경
        }
    }
}
