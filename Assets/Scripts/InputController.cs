using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    private TileType currentType = TileType.Empty; // 마우스 클릭된 위치의 타일을 currentType 속성으로 변경
    private Tile playerTile = null;

    [SerializeField]
    private CameraController cameraController; // 카메라 위치, 줌 제어를 위한 CameraController
    private Vector2 previousMousePosition; // 직전 프레임의 마우스 위치
    private Vector2 currentMousePosition; // 현재 프레임의 마우스 위치

    private void Update()
    {
        //마우스가 UI위에 있을때는 Update()내용이 실행되지 않는다.
        if (EventSystem.current.IsPointerOverGameObject()) return;

        UpdateCamera();

        RaycastHit hit;
        if (Input.GetMouseButton(0))
        {
            //카메라부터 현재 마우스 위치로 뻗어나가는 광선 생성
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            // 광선에 부딪힌 오브젝트가 존재하면 hit에 저장
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //hit 오브젝트의 Tile 컴포넌트 정보를 불러와 tile 변수에 저장
                // 이때 hit 오브젝트에 Tile 컴포넌트가 없으면 null 변환
                Tile tile = hit.transform.GetComponent<Tile>();
                if(tile != null)
                {
                    // 플레이어 타일은 맵에 1개만 배치할 수 있기 때문에
                    // 이전에 배치된 플레이어 타일이 있으면 Empty 속성으로 설정
                    if(currentType == TileType.Player)
                    {
                        if(playerTile != null)
                        {
                            playerTile.TileType = TileType.Empty;
                        }
                        playerTile = tile;
                    }

                    //부딪힌 오브젝트를 tileType 속성으로 변경(타일, 아이템, 플레이어 캐릭터)
                    tile.TileType = currentType;
                }
            }
        }
    }

    private void UpdateCamera()
    {
        // 키보드를 이용한 카메라 이동
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        cameraController.SetPosition(x, y);

        // 마우스 휠 버튼을 이용한 카메라 이동
        if (Input.GetMouseButtonDown(2))
        {
            currentMousePosition = previousMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(2))
        {
            currentMousePosition = Input.mousePosition;
            if(previousMousePosition != currentMousePosition)
            {
                Vector2 move = (previousMousePosition - currentMousePosition) * 0.5f;
                cameraController.SetPosition(move.x, move.y);
            }
        }
        previousMousePosition = currentMousePosition;

        // 마우스 휠을 이용한 카메라 Zoom In/Out
        float distance = Input.GetAxisRaw("Mouse ScrollWheel");
        cameraController.SetOrthgraphicSize(-distance);
    }

    /// <summary>
    /// 타일, 아이템, 플레이어 캐릭터 버튼을 눌러 tileType을 변경
    /// </summary>
    public void SetTileType(int tileType)
    {
        currentType = (TileType)tileType;
    }

}
