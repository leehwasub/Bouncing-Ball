using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tilemap2D : MonoBehaviour
{
    [SerializeField]
    private GameObject tilePrefab; // 맵에 배치되는 타일 프리팹

    [SerializeField]
    private TMP_InputField inputWidth; // 맵의 width 크기를 얻어오는 InputField
    [SerializeField]
    private TMP_InputField inputHeight; // 맵의 height 크기를 얻어오는 InputField

    private MapData mapData; // 맵 데이터 저장에 사용되는 데이터 양식 클래스

    // 맵x,y 크기 프로퍼티
    public int Width { private set; get; } = 10;
    public int Height { private set; get; } = 10;

    // 맵에 배치되는 타일 정보 저장을 위한 List 프로퍼티
    public List<Tile> TileList { private set; get; }

    private void Awake()
    {
        // InputField에 표시되는 기본 값 설정
        inputWidth.text = Width.ToString();
        inputHeight.text = Height.ToString();

        mapData = new MapData();
        TileList = new List<Tile>();
    }

    public void GenerateTilemap()
    {
        //out또는 ref 키워드를 사용할때 프로퍼티 사용이 불가하기 때문에 지역 변수 선언
        int width, height;

        // InputField에 있는 width, height 문자열을 width, height 변수에 정수로 저장
        int.TryParse(inputWidth.text, out width);
        int.TryParse(inputHeight.text, out height);

        // 프로퍼티 Width, Height 값 설정
        Width = width;
        Height = height;

        for(int y = 0; y < Height; y++)
        {
            for(int x = 0; x < Width; x++)
            {
                // 생성되는 타일맵 중앙이 (0, 0, 0)인 위치
                Vector3 position = new Vector3((-Width * 0.5f + 0.5f) + x, (Height * 0.5f - 0.5f) - y, 0);

                SpawnTile(TileType.Empty, position);
            }
        }

        mapData.mapSize.x = Width;
        mapData.mapSize.y = Height;
        mapData.mapData = new int[TileList.Count];
    }

    private void SpawnTile(TileType tileType, Vector3 position)
    {
        GameObject clone = Instantiate(tilePrefab, position, Quaternion.identity);
        clone.name = "Tile"; // Tile 오브젝트의 이름을 "Tile"로 설정
        clone.transform.SetParent(transform); // Tilemap2D 오브젝트를 Tile 오브젝트의 부모로 설정

        Tile tile = clone.GetComponent<Tile>(); // 방금 생성한 타일(clone) 오브젝트의 Tile.Setup() 메소드 호출
        tile.Setup(tileType);

        TileList.Add(tile); // 모든 타일 정보는 tileList 리스트에 보관
    }

    public MapData GetMapData()
    {
        //맵에 배치된 모든 타일의 정보를 mapData.mapData 배열에 저장
        for(int i = 0; i < TileList.Count; i++)
        {
            if(TileList[i].TileType != TileType.Player)
            {
                mapData.mapData[i] = (int)TileList[i].TileType;
            }
            // 현재 위치의 타일이 플레이어면
            else
            {
                // 현재 위치의 타일은 빈 타일(Empty)로 설정
                mapData.mapData[i] = (int)TileType.Empty;

                // 현재 위치 정보를 mapData.playerPosition에 저장
                int x = (int)TileList[i].transform.position.x;
                int y = (int)TileList[i].transform.position.y;
                mapData.playerPosition = new Vector2Int(x, y);
            }
        }

        return mapData;
    }


}
