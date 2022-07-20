using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public int i;
    public int j;
    public bool visited;

    public void SetCoordinates(int i, int j) {
        this.i = i;
        this.j = j;
    }

    public void SetVisited(bool visited) {
        this.visited = visited;
    }

    public int GetCoordinatesI() {
        return i;
    }

    public int GetCoordinatesJ() {
        return j;
    }

    public bool GetVisited() { return visited; }

}
