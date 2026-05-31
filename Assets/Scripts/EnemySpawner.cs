using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public Transform SpawnPoint;
    public float TimeBetweenEnemies = 0.8f;
    public float TimeBetweenWaves = 3.0f;

    void Start() => StartCoroutine(RunWaves());

    IEnumerator RunWaves()
    {
        while (true)
        {
            if (GameManager.Instance == null)
                yield break;

            int Wave = GameManager.Instance.WaveNumber;
            int Count = Wave + 4;
            GameManager.Instance.EnemiesLeft = Count;

            for (int i = 0; i < Count; i++)
            {
                if (GameManager.Instance == null)
                    yield break;

                SpawnEnemy(Wave);
                yield return new WaitForSeconds(TimeBetweenEnemies);
            }

            float Timeout = 60f;

            while (GameManager.Instance != null &&
                   GameManager.Instance.EnemiesLeft > 0 &&
                   Timeout > 0)
            {
                Timeout -= Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(TimeBetweenWaves);

            if (GameManager.Instance != null)
                GameManager.Instance.WaveNumber++;
        }
    }

    void SpawnEnemy(int Wave)
    {
        GameObject Go = Instantiate(
            EnemyPrefab, SpawnPoint.position, Quaternion.identity);
        Enemy E = Go.GetComponent<Enemy>();
        if (E == null) return;

        E.MaxHP = 30f * Mathf.Pow(1.15f, Wave - 1);
        E.Speed = Mathf.Min(2f + (Wave - 1) * 0.15f, 4f);
        E.Gold = 4 + Wave;
    }
}