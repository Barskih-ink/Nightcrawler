using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowPlayerWithZoom : MonoBehaviour
{
    public Transform target;                 // �����
    public Vector3 offset = new Vector3(0f, 0f, -10f);  // �������� ������
    public float zoom = 5f;                  // ��������� (��� ������ � ��� ������)

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = zoom;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // ������������� ������ �� ������ � ��������
        transform.position = target.position + offset;

        // ��������� ������ ������ �� ������ ������������� ���������
        cam.orthographicSize = zoom;
    }
}
