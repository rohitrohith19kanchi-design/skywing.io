using UnityEngine;

public class RunnerWorldController : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] private float startingSpeed = 5f;
    [SerializeField] private float maximumSpeed = 12f;
    [SerializeField] private float speedIncreaseRate = 0.15f;

    [Header("Cleanup")]
    [SerializeField] private float destroyX = -15f;

    public float CurrentSpeed { get; private set; }

    private bool isRunning;

    private void Awake()
    {
        CurrentSpeed = startingSpeed;
        isRunning = false;
    }

    private void Update()
    {
        if (!isRunning)
            return;

        CurrentSpeed = Mathf.Min(
            CurrentSpeed + speedIncreaseRate * Time.deltaTime,
            maximumSpeed
        );

        GameObject[] obstacles =
            GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obstacle in obstacles)
        {
            obstacle.transform.position +=
                Vector3.left * CurrentSpeed * Time.deltaTime;

            if (obstacle.transform.position.x < destroyX)
                Destroy(obstacle);
        }
    }

    public void StartWorld()
    {
        isRunning = true;
    }

    public void StopWorld()
    {
        isRunning = false;
    }
}