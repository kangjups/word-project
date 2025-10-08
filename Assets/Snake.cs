using System.Collections.Generic; // List를 사용하기 위해 추가해야 합니다.
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 direction = Vector2.right;
    public float moveSpeed = 0.5f;

    // 꼬리 오브젝트들을 담아둘 리스트입니다.
    private List<Transform> tail = new List<Transform>();

    // 꼬리 프리팹을 Inspector 창에서 받아올 변수입니다.
    public Transform tailPrefab;

    void Start()
    {
        InvokeRepeating("Move", moveSpeed, moveSpeed);
    }

    void Update()
    {
        // 방향 전환 시, 반대 방향으로 바로 꺾지 못하게 하는 로직 추가
        if (direction.x != 0f)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                direction = Vector2.up;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                direction = Vector2.down;
            }
        }
        else if (direction.y != 0f)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                direction = Vector2.left;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                direction = Vector2.right;
            }
        }
    }

    void Move()
    {
        // 꼬리가 머리를 따라오게 하는 로직
        // 1. 머리가 움직이기 전 현재 위치를 저장
        Vector3 lastPosition = transform.position;

        // 2. 머리를 다음 위치로 이동
        transform.position = new Vector3(
            Mathf.Round(transform.position.x) + direction.x,
            Mathf.Round(transform.position.y) + direction.y,
            0.0f
        );

        // 3. 꼬리가 따라오게 함
        if (tail.Count > 0)
        {
            // 마지막 꼬리 조각을 머리가 있던 자리로 이동시키고,
            tail[tail.Count - 1].position = lastPosition;

            // 리스트의 맨 앞에 삽입하여 순서를 맞춥니다.
            tail.Insert(0, tail[tail.Count - 1]);
            tail.RemoveAt(tail.Count - 1);
        }
    }

    // Is Trigger가 체크된 Collider와 부딪혔을 때 호출되는 함수입니다.
    void OnTriggerEnter2D(Collider2D other)
    {
        // 부딪힌 상대가 "Food" 라면
        if (other.tag == "Food")
        {
            // 1. 음식 위치를 랜덤하게 변경
            // (화면 크기에 맞춰 -8~8 사이로 임의 지정)
            other.transform.position = new Vector2(
                Random.Range(-8, 8),
                Random.Range(-4, 4)
            );

            // 2. 꼬리 조각을 생성
            Transform newSegment = Instantiate(this.tailPrefab);

            // 3. 생성된 꼬리의 위치는 일단 지금 머리 위치로 설정
            newSegment.position = this.transform.position;

            // 4. 꼬리 리스트에 추가
            // 리스트의 맨 앞에 추가해야 꼬이지 않습니다.
            tail.Insert(0, newSegment);
        }
    }
}