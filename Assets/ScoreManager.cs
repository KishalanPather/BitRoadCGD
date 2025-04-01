using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
public static ScoreManager Instance;
    public Transform vehicleTransform;
    public VehicleHandler vehicleHandler;
    public int Score;
    public Text scoreText;
    public Text speedText;
    public Text fuelText;
    public Slider fuelSlider;
    public Image fuelFillImage;
    public Text boostInfoText;

    [Header("Score Multipliers")]
    [SerializeField] private float distanceMultiplier = 1f;
    [SerializeField] private float coinMultiplier = 1f;
    [SerializeField] private float fuelEfficiencyMultiplier = 100f;
    [SerializeField] private float avoidanceMultiplier = 50f;

    [Header("Score Components")]
    private float distanceScore = 0f;
    private float coinScore = 0f;
    private float fuelEfficiencyScore = 0f;
    private float obstacleAvoidanceScore = 0f;
    private float totalScore = 0f;

    [Header("Fuel Colours")]
    public Color fullFuelColour = Color.green;
    public Color midFuelColour = Color.yellow;
    public Color lowFuelColour = Color.red;
    // For tracking distance traveled
    private Vector3 lastPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (vehicleTransform == null)
        {
            Debug.LogError("Vehicle transform not assigned in ScoreManager.");
        }
        lastPosition = vehicleTransform.position; 
    }

      private void Update()
    {
        if (!GameManager.Instance.IsGameActive()) return;
        UpdateDistanceScore();
        UpdateFuelEfficiencyScore();
        CalculateTotalScore();
        UpdateHUD();
    }

    /// <summary>
    /// Calculates the incremental distance traveled since the last frame.
    /// </summary>
    void UpdateDistanceScore()
    {
        float distanceDelta = Vector3.Distance(vehicleTransform.position, lastPosition);
        distanceScore = distanceDelta * distanceMultiplier;
        lastPosition = vehicleTransform.position;
    }

    /// <summary>
    /// Updates the fuel efficiency score based on current vehicle data.
    /// Assumes VehicleHandler exposes a method GetFuelEfficiency() returning a value between 0 and 1.
    /// </summary>
    void UpdateFuelEfficiencyScore()
    {
        if (vehicleHandler != null)
        {
            //float efficiency = vehicleHandler.GetFuelEfficiency();
           //fuelEfficiencyScore = efficiency * fuelEfficiencyMultiplier;

            float fuelPercentage = vehicleHandler.GetCurrentFuel() / 100f;
            fuelEfficiencyScore = fuelPercentage * fuelEfficiencyMultiplier;

            fuelSlider.value = fuelPercentage;

            // Change color based on fuel percentage
            if (fuelFillImage != null)
            {
                if (fuelPercentage > 0.55f)
                {
                    fuelFillImage.color = Color.Lerp(midFuelColour, fullFuelColour, (fuelPercentage - 0.5f) * 2);
                }
                else
                {
                    fuelFillImage.color = Color.Lerp(lowFuelColour, midFuelColour, fuelPercentage * 2);
                }
            }
        }
    }

    /// <summary>
    /// Combines the score components to produce the total score.
    /// </summary>
    void CalculateTotalScore()
    {
        float baseScore = distanceScore + coinScore + obstacleAvoidanceScore;
        if (vehicleHandler != null)
        {
            totalScore += baseScore * vehicleHandler.GetScoreMultiplier();
        }
        else
        {
            totalScore += baseScore;
        }
    }

    /// <summary>
    /// Updates the on-screen HUD with the current score.
    /// </summary>
    void UpdateHUD()
    {
        if (scoreText != null)
        {
            scoreText.text = Mathf.FloorToInt(totalScore).ToString();
        }

        if (speedText != null && vehicleHandler != null)
        {
            speedText.text = Mathf.FloorToInt(vehicleHandler.GetCurrentSpeed()).ToString() + " KMH";
        }

        if (boostInfoText != null && vehicleHandler != null)
        {
            float multiplier = vehicleHandler.GetScoreMultiplier();
            if (multiplier > 1f)
            {
                boostInfoText.text = multiplier.ToString() + "x";
            }
            else
            {
                boostInfoText.text = "";
            }
        }
    }

    /// <summary>
    /// Call this method when a coin is collected.
    /// </summary>
    /// <param name="baseValue">The base point value of the coin.</param>
    public void AddCoinScore(int baseValue)
    {
        coinScore += baseValue * coinMultiplier;
    }

    /// <summary>
    /// Call this method when an obstacle is successfully avoided.
    /// </summary>
    public void AddObstacleAvoidanceScore()
    {
        obstacleAvoidanceScore += avoidanceMultiplier;
    }

    /// <summary>
    /// Public accessor for the total score.
    /// </summary>
    public float GetTotalScore()
    {
        return totalScore;
    }
}
