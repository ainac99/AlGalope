using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMan : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        Camera cam = GetComponent<Camera>();

        float x = 0.5f * (ChessBoard.N - ChessBoard.TILE_SIZE);
        float y = 0.5f * (ChessBoard.M - ChessBoard.TILE_SIZE);
        this.transform.position = new Vector3(x, y, -10);

        cam.orthographicSize = ChessBoard.N / 2.0f + 0.2f;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
