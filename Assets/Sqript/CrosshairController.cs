using UnityEngine;

/// <summary>
/// 照準 (Crosshair) を制御するコンポーネント
/// マウスカーソルの位置に照準を移動する
/// </summary>
public class CrosshairController : MonoBehaviour
{
    private Transform player;
    private Camera mainCamera;

    private Vector3 currentPosition = Vector3.zero;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        mainCamera = Camera.main;
    }

    void Update()
    {
       
            var distance = Vector3.Distance(player.transform.position, mainCamera.transform.position);
            var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);

            currentPosition = mainCamera.ScreenToWorldPoint(mousePosition);
             this.transform.position = currentPosition;
    }





}







