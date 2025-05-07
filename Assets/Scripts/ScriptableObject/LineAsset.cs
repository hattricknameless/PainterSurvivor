using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Line Asset", menuName = "Line Asset", order = 1)]
public class LineAsset : ScriptableObject {
    public string sampleName;
    public List<Vector2> samplePoints;

    public void Record(List<Vector3> inputs) {
        if (samplePoints.Count == 0) {
            samplePoints = inputs.Select(point => new Vector2(point.x, point.y)).ToList();
        }
        else {
            Debug.Log("Error, sample already loaded");
        }
    }
}
