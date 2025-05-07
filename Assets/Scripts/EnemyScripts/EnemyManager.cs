using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    private PainterMovement painter;
    public float spawnTimer;
    [SerializeField] private List<int> batchCount;
    private int batchPointer = 0;
    private bool ifDistributed = false;
    [SerializeField] private float spawnInterval;
    [SerializeField] private float minSummonDistance;
    [SerializeField] private float maxSummonDistance;
    [SerializeField] private GameObject enemyPrefab;

    [Header("UI References")]
    private int enemyCount;
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [SerializeField] private TextMeshProUGUI waveCountText;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private TextMeshProUGUI endgameText;
    [SerializeField] private ReturnMainMenu returnButton;

    private void Start() {
        painter = FindObjectOfType<PainterMovement>();
        StartCoroutine(SpawnCountDown());
    }

    private void Update() {
        GameObject[] enemyAssembly = GameObject.FindGameObjectsWithTag("Enemy");
        enemyCount = enemyAssembly.Count();
        UIUpdate();
        if (ifDistributed && enemyCount == 0) {
            endgameText.gameObject.SetActive(true);
            endgameText.text = "You Won";
            returnButton.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void UIUpdate() {
        enemyCountText.text = $"Enemy Count: {enemyCount}";
        waveCountText.text = batchPointer == 0 ? "" : $"Wave {batchPointer}";
        if (!ifDistributed) {
            float time = spawnTimer < 0 ? 0 : (float)Math.Round(spawnTimer, 2);
            countdownText.text = $"Next Wave In {time:F2}";
        }
        else {
            countdownText.text = "";
        }
    }

    private List<Vector3> GenerateSpawnPoints(int count) {
        List<Vector3> spawnPoints = new();
        for (int i = 0; i < count; i++) {
            spawnPoints.Add(GeneratePoint());
        }
        return spawnPoints;
    }

    private Vector3 GeneratePoint() {
        Vector3 spawnPoint;

        do {
            // Generate a random angle in radians
            float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);

            // Generate a random distance within the arena radius
            float distance = UnityEngine.Random.Range(0f, maxSummonDistance);

            // Convert polar to Cartesian coordinates
            float x = distance * Mathf.Cos(angle);
            float z = distance * Mathf.Sin(angle);

            // Form the potential spawn point
            spawnPoint = new Vector3(x, 0, z);

        } while (Vector3.Distance(spawnPoint, painter.transform.position) < minSummonDistance);  // Ensure outside exclusion zone
        return spawnPoint;
    }

    private IEnumerator SpawnCountDown() {
        spawnTimer = spawnInterval;
        while (true) {
            //if batches are fully distributed end the coroutine
            if (batchPointer == batchCount.Count) {
                ifDistributed = true;
                yield break;
            }
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0) {
                List<Vector3> points = GenerateSpawnPoints(batchCount[batchPointer++]);
                foreach (Vector3 point in points) {
                    Instantiate(enemyPrefab, point, enemyPrefab.transform.rotation);
                }
                spawnTimer = spawnInterval;
            }
            yield return null;
        }
    }
}
