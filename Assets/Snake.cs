using System.Collections.Generic; // List�� ����ϱ� ���� �߰��ؾ� �մϴ�.
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 direction = Vector2.right;
    public float moveSpeed = 0.5f;

    // ���� ������Ʈ���� ��Ƶ� ����Ʈ�Դϴ�.
    private List<Transform> tail = new List<Transform>();

    // ���� �������� Inspector â���� �޾ƿ� �����Դϴ�.
    public Transform tailPrefab;

    void Start()
    {
        InvokeRepeating("Move", moveSpeed, moveSpeed);
    }

    void Update()
    {
        // ���� ��ȯ ��, �ݴ� �������� �ٷ� ���� ���ϰ� �ϴ� ���� �߰�
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
        // ������ �Ӹ��� ������� �ϴ� ����
        // 1. �Ӹ��� �����̱� �� ���� ��ġ�� ����
        Vector3 lastPosition = transform.position;

        // 2. �Ӹ��� ���� ��ġ�� �̵�
        transform.position = new Vector3(
            Mathf.Round(transform.position.x) + direction.x,
            Mathf.Round(transform.position.y) + direction.y,
            0.0f
        );

        // 3. ������ ������� ��
        if (tail.Count > 0)
        {
            // ������ ���� ������ �Ӹ��� �ִ� �ڸ��� �̵���Ű��,
            tail[tail.Count - 1].position = lastPosition;

            // ����Ʈ�� �� �տ� �����Ͽ� ������ ����ϴ�.
            tail.Insert(0, tail[tail.Count - 1]);
            tail.RemoveAt(tail.Count - 1);
        }
    }

    // Is Trigger�� üũ�� Collider�� �ε����� �� ȣ��Ǵ� �Լ��Դϴ�.
    void OnTriggerEnter2D(Collider2D other)
    {
        // �ε��� ��밡 "Food" ���
        if (other.tag == "Food")
        {
            // 1. ���� ��ġ�� �����ϰ� ����
            // (ȭ�� ũ�⿡ ���� -8~8 ���̷� ���� ����)
            other.transform.position = new Vector2(
                Random.Range(-8, 8),
                Random.Range(-4, 4)
            );

            // 2. ���� ������ ����
            Transform newSegment = Instantiate(this.tailPrefab);

            // 3. ������ ������ ��ġ�� �ϴ� ���� �Ӹ� ��ġ�� ����
            newSegment.position = this.transform.position;

            // 4. ���� ����Ʈ�� �߰�
            // ����Ʈ�� �� �տ� �߰��ؾ� ������ �ʽ��ϴ�.
            tail.Insert(0, newSegment);
        }
    }
}