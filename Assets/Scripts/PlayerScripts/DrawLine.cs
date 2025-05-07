using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour {

    [SerializeField] private Camera mainCamera;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private List<Vector3> pointList;
    [SerializeField] private List<Vector3> sampleList;
    [SerializeField] private RecognizerManager recognizerManager;
    [SerializeField] private float sampleInterval;
    public bool isAcceptable = true;

    [Header("Editor Only")]
    public LineAsset lineAsset;
    [SerializeField] private float drawCameraDistance;


    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        if (mainCamera != null) {
            mainCamera = Camera.main;
        }
        lineRenderer.positionCount = 0;
        recognizerManager = GetComponent<RecognizerManager>();
    }

    public IEnumerator DrawInput() {
        lineRenderer.positionCount = 0;
        if (pointList.Count != 0) {pointList.Clear();}
        if (sampleList.Count != 0) {sampleList.Clear();}
        isAcceptable = true;
        while (true) {
            //Get the mouse position hit
            Vector3 mouseScreenPos = Input.mousePosition;
            //Set them all on the z=3 plane
            mouseScreenPos.z = 3;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            if (sampleList.Count == 0) {
                sampleList.Add(mousePos);
            }
            else {
                //if the previous sample point's distance is larger than the interval, add sample point
                if (Vector3.Distance(sampleList[sampleList.Count-1], mousePos) > sampleInterval) {
                    sampleList.Add(mousePos);
                }
            }
            //Draw the visual line
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, mousePos);
            yield return null;
        }
    }

    public void EndDraw() {
        //if the sample is too short, it is not acceptable, to prevent spam
        if (sampleList.Count < 5) {
            isAcceptable = false;
            return;
        }
        //If the lineAsset equiped is empty, record the samples
        if (lineAsset != null && lineAsset.samplePoints.Count == 0) {
            lineAsset.Record(sampleList);
        }
        //Send samples to recognizer to detect shape
        recognizerManager.Recognize(sampleList);
    }
}