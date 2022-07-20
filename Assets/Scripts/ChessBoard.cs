using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChessBoard : MonoBehaviour {

    // Size of board
    public static int N = 50;
    public static int M = 50;

    // method to find path
    private enum Method { WARNSDORFF, RANDOM, REPITIENDO };
    private const Method method = Method.RANDOM;

    // initial cell
    private int START_I;
    private int START_J;

    // board colors
    private enum Colors { BLAUS, RANDOM, CALIDS, VERDS, GRIS, RESTRINGIT_RANDOM, VERMELLS }
    private const Colors color = Colors.CALIDS;

    public float MIN_H = 0f;
    public float MAX_H = 0f;
    public float MIN_S = 0f;
    public float MAX_S = 0f;
    public float MIN_V = 0f;
    public float MAX_V = 0f;

    public GameObject tilePrefab;
    public static float TILE_SIZE = 1f;

    private GameObject[,] tiles;
    [SerializeField] private Coord currentTile;
    private bool STOP = false;


    [SerializeField] private List<Transform> points;
    [SerializeField] private Line line;

    public class Coord : IComparable<Coord> {
        public Coord(int i, int j, int poss) {
            this.i = i;
            this.j = j;
            this.poss = poss;
        }

        public Coord(int i, int j) {
            this.i = i;
            this.j = j;
        }

        public int i { get; }
        public int j { get; }

        public int poss { get; set; }

        int IComparable<Coord>.CompareTo(Coord other) {
            if (other.poss > this.poss)
                return -1;
            else if (other.poss == this.poss)
                return (UnityEngine.Random.Range(0, 1) == 0) ?  1 : (-1);
            else
                return 1;
        }

    }

    // possible knight moves
    public int[] ci = new int[] { 1, 1, 2, 2, -1, -1, -2, -2 };
    public int[] cj = new int[] { 2, -2, 1, -1, 2, -2, 1, -1 };

    void Start() {

        TriarColors(color);
        GenerateTiles(TILE_SIZE, N, M);

        START_I = UnityEngine.Random.Range(0, N);
        START_J = UnityEngine.Random.Range(0, M);
        SetUpStart(START_I, START_J);

        STOP = false;
    }

    private void GenerateTiles(float tileSize, int n, int m) {

        tiles = new GameObject[N, M];

        for (int i = 0; i < N; i++)
            for (int j = 0; j < M; j++)
                tiles[i, j] = GenerateSingleTile(tileSize, i, j);

    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
    }

    private GameObject GenerateSingleTile(float tileSize, int i, int j) {

        GameObject tileObject = Instantiate(tilePrefab, new Vector2(i * tileSize, j * tileSize), Quaternion.identity);
        tileObject.transform.parent = transform;
        tileObject.name = "Tile (" + i + "," + j + ")";
        tileObject.GetComponent<Tile>().SetCoordinates(i, j);
        tileObject.GetComponent<Tile>().SetVisited(false);

        tileObject.GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV(MIN_H, MAX_H, MIN_S, MAX_S, MIN_V, MAX_V);
        return tileObject;
    }

    private void SetUpStart(int i, int j) {

        currentTile = new Coord(i, j);

        points.Add(tiles[i, j].transform);
        line.Draw(points);
        tiles[currentTile.i, currentTile.j].GetComponent<Tile>().SetVisited(true); ;

    }

    private void Update() {
        if (!STOP) AddNewPoint();
    }


    private void AddNewPoint() {

        Coord next;

        if (method == Method.WARNSDORFF) next = NextPointWarnsdorff();
        else if (method == Method.RANDOM) next = NextPointRandom();
        else next = NextPointRandom();

        if (next != null)
        {
            // add next to line points and send it to line
            points.Add(tiles[next.i, next.j].transform);
            line.Draw(points);                                         // next is the new current
            tiles[next.i, next.j].GetComponent<Tile>().SetVisited(true); // set the new current as visited
            //ChangeColor(next);
            this.currentTile = next;
        }
        else Finish();
    }

    private void Finish() {
        STOP = true;

        string fotoName = "AlGalope_" + method.ToString() + "_" + START_I + "_" + START_J + "_" + points.Count;

        if (CheckClosed())
        {
            points.Add(tiles[START_I, START_J].transform);
            line.Draw(points);
            fotoName += "_CLOSED";
        }

        Screenshoter.TakeScreenshot(fotoName);
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    private bool CheckClosed() {

        int i = currentTile.i;
        int j = currentTile.j;

        for(int k = 0; k < 8; ++k)  if (i + ci[k] == START_I && j + cj[k] == START_J) return true;

        return false;

    }

    // Returns the coordinates of a random next possible tile.
    private Coord NextPointRandom() {

        int i = currentTile.i;
        int j = currentTile.j;

        List<Coord> possibleNextPoints = new List<Coord>();

        for(int k = 0; k < 8; ++k)
            if (IsValid(i + ci[k], j + cj[k])) possibleNextPoints.Add(new Coord(i + ci[k], j + cj[k]));

        if (possibleNextPoints.Count == 0) return null;

        return possibleNextPoints[UnityEngine.Random.Range(0, possibleNextPoints.Count)];

    }

    // Checks whether a coord is inside the board and has not been visited yet
    private bool IsValid(int i, int j) {
        return i >= 0 && j >= 0 && i < N && j < M && !(tiles[i, j].GetComponent<Tile>().GetVisited());
    }

    private void ChangeColor(Coord c) {
        tiles[c.i, c.j].GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV(0.5f, 0.6f, 0f, 1f, 0.1f, 1f);
    }

    private Coord NextPointWarnsdorff() {

        int i = currentTile.i;
        int j = currentTile.j;

        List<Coord> possibleNextPoints = new List<Coord>();

        for (int k = 0; k < 8; ++k)
            if (IsValid(i + ci[k], j + cj[k])) possibleNextPoints.Add(new Coord(i + ci[k], j + cj[k], CountPossiblesFrom(i + ci[k], j + cj[k])));

        if (possibleNextPoints.Count == 0) return null;

        return possibleNextPoints.Min();
    }

    private int CountPossiblesFrom(int i, int j) {
        int cnt = 0;
        for (int k = 0; k < 8; ++k)
            if (IsValid(i + ci[k], j + cj[k])) cnt++;

        Debug.Log("Cell " + i + "," + j + " -> " + cnt);

        return cnt;
    }

    private void TriarColors(Colors color) {

        switch (color)
        {
            case Colors.BLAUS:
                MIN_H = 0.5f;
                MAX_H = 0.6f;
                MIN_S = 0f;
                MAX_S = 1f;
                MIN_V = 0.5f;
                MAX_V = 1f;
                break;

            case Colors.CALIDS:
                MIN_H = 0f;
                MAX_H = 0.2f;
                MIN_S = 0f;
                MAX_S = 1f;
                MIN_V = 0.5f;
                MAX_V = 1f;
                break;

            case Colors.RANDOM:
                MIN_H = 0f;
                MAX_H = 1f;
                MIN_S = 0f;
                MAX_S = 1f;
                MIN_V = 0.5f;
                MAX_V = 1f;
                break;

            case Colors.RESTRINGIT_RANDOM:
                float r = 0.01f*UnityEngine.Random.Range(0, 100);
                MIN_H = r;
                MAX_H = (r + 0.2f);
                MIN_S = 0f;
                MAX_S = 1f;
                MIN_V = 0.5f;
                MAX_V = 1f;
                break;

            case Colors.VERMELLS:
                MIN_H = 0f;
                MAX_H = 0f;
                MIN_S = 0f;
                MAX_S = 1f;
                MIN_V = 0.5f;
                MAX_V = 1f;
                break;
            case Colors.GRIS:
                MIN_H = 0f;
                MAX_H = 0f;
                MIN_S = 0f;
                MAX_S = 0f;
                MIN_V = 0.1f;
                MAX_V = 0.4f;
                break;
        }
    }
}

       
