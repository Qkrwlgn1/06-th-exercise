using UnityEngine;

public class ObjectFollowMousePosition : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        //ȭ���� ���콺 ��ǥ�� �������� ���� ���� ���� ��ǥ�� ���Ѵ�.
        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        transform.position = mainCamera.ScreenToWorldPoint(position);
        //z ��ġ�� 0���� ����
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
