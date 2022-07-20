using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {

    private LineRenderer lr;
    private List<Transform> points;

    // Start is called before the first frame update
    void Start() {
        lr = GetComponent<LineRenderer>();
        lr.startWidth = 0.08f;
        lr.endWidth = 0.08f;
    }

    public void Draw(List<Transform> points) {

        lr.positionCount = points.Count;
        this.points = points;

        for (int i = 0; i < points.Count; i++)
        {
            lr.SetPosition(i, points[i].position);
        }
    }

}