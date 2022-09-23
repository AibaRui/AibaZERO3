using UnityEngine;

/// <summary>
/// �Ə� (Crosshair) �𐧌䂷��R���|�[�l���g
/// �}�E�X�J�[�\���̈ʒu�ɏƏ����ړ�����
/// </summary>
public class CrosshairController : MonoBehaviour
{
    private GameObject player;
    private Camera mainCamera;

    private Vector3 currentPosition = Vector3.zero;

    void Start()
    {


    }
    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (player)
        {
            var distance = Vector3.Distance(player.transform.position, mainCamera.transform.position);
            var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);

            currentPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            this.transform.position = currentPosition;
        }

        if(!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

    }





}







