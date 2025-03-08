using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject muzzleFlashEffect; // 총구 화염 이펙트
    [SerializeField] private GameObject bulletFireEffect; // 총알 화염 이펙트
    [SerializeField] private float fireRate = 0.1f; // 발사 간격 (초)
    [SerializeField] private Transform muzzleFlashPoint; // 총구 위치
    [SerializeField] private GameObject bulletPrefab; // 총알 Prefab
    private bool isFiring = false; // 발사 중 여부
    private Animator anim;

    void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    public void StartFiring()
    {
        if (!isFiring) // 이미 발사 중이 아니면 시작
        {
            isFiring = true;
            InvokeRepeating("Fire", 0f, fireRate); // fireRate 간격으로 Fire 함수 호출
        }
    }

    void Fire()
    {
        // 총 발사 애니메이션
        // anim.SetTrigger("Shoot"); // Shoot 트리거 발동

        // 총구 화염 이펙트 재생
        if (muzzleFlashEffect != null)
        {
            muzzleFlashEffect.SetActive(true);
            // 잠시 후 이펙트 끄기
            Invoke("StopMuzzleFlash", 0.1f); // 이펙트가 켜진 후 일정 시간 후에 꺼짐
        }

        // 실제 총알 발사 로직 추가
        //TODO : 수명 대신 오브젝트 풀링으로 변경 요망
        if (bulletPrefab != null && muzzleFlashPoint != null)
        {
            // 총알 생성
            GameObject bullet = Instantiate(bulletPrefab, muzzleFlashPoint.position, muzzleFlashPoint.rotation);

            // 총알이 총구의 방향으로 발사되도록 회전 설정
            bullet.transform.forward = muzzleFlashPoint.forward;  // 총구의 방향으로 총알이 발사되도록 설정

            // 총알의 x 회전값을 -90으로 설정
            Vector3 bulletRotation = bullet.transform.rotation.eulerAngles;
            bulletRotation.x = -90f;
            bullet.transform.rotation = Quaternion.Euler(bulletRotation);

            // Rigidbody를 사용하여 총알을 발사 (Rigidbody는 Prefab에 미리 추가되어 있어야 함)
            //TODO : 수명 대신 오브젝트 풀링으로 변경 요망
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                // 총구의 방향으로 총알을 발사 (반대 방향으로 나가도록 수정)
                bulletRb.linearVelocity = -muzzleFlashPoint.forward * 20f; // 발사 속도 조정
            }

            // 총알 수명 설정 (3초 뒤에 사라짐)
            //TODO : 수명 대신 오브젝트 풀링으로 변경 요망
            Destroy(bullet, 3f); // 3초 후 총알을 삭제
        }
    }

    void StopMuzzleFlash()
    {
        muzzleFlashEffect.SetActive(false); // 총구 화염 이펙트 끄기
    }

    // 발사 멈추기
    public void StopFiring()
    {
        CancelInvoke("Fire"); // Fire 반복 호출 취소
        isFiring = false;
    }
}
