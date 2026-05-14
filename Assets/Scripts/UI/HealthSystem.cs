using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class HealthSystem : MonoBehaviour
{
    public static HealthSystem Instance { get; private set; }

    [Header("Health")]
    [SerializeField] private int maxHearts = 3;
    private int currentHearts;

    [Header("Gems")]
    private int gemsCollected = 0;
    private int totalGems = 0;

    [Header("UI References")]
    [SerializeField] private Transform heartsContainer;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Text gemCountText;

    [Header("Respawn")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnDelay = 1f;
    [SerializeField] private GameObject player;

    private List<Image> heartImages = new List<Image>();
    private bool isDead = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        currentHearts = maxHearts;
        totalGems = FindObjectsByType<Gem>(FindObjectsSortMode.None).Length;
        UpdateGemUI();
        BuildHeartUI();
    }

    void BuildHeartUI()
    {
        if (heartsContainer == null) return;
        foreach (Transform child in heartsContainer)
            Destroy(child.gameObject);

        heartImages.Clear();
        for (int i = 0; i < maxHearts; i++)
        {
            if (heartPrefab == null) break;
            var heart = Instantiate(heartPrefab, heartsContainer);
            var img = heart.GetComponent<Image>();
            if (img != null) heartImages.Add(img);
        }
    }

    public void TakeDamage(int amount = 1)
    {
        if (isDead) return;
        currentHearts -= amount;
        currentHearts = Mathf.Max(currentHearts, 0);
        UpdateHeartUI();

        if (currentHearts <= 0)
            StartCoroutine(Die());
    }

    public void AddGem(int amount = 1)
    {
        gemsCollected += amount;
        UpdateGemUI();
    }

    private void UpdateHeartUI()
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (heartImages[i] != null)
                heartImages[i].color = i < currentHearts ? Color.red : new Color(0.3f, 0.3f, 0.3f, 0.5f);
        }
    }

    private void UpdateGemUI()
    {
        if (gemCountText != null)
            gemCountText.text = $"{gemsCollected} / {totalGems}";
    }

    private IEnumerator Die()
    {
        isDead = true;
        if (player != null) player.SetActive(false);
        yield return new WaitForSeconds(respawnDelay);
        Respawn();
    }

    private void Respawn()
    {
        currentHearts = maxHearts;
        isDead = false;
        UpdateHeartUI();

        if (player != null)
        {
            if (respawnPoint != null)
                player.transform.position = respawnPoint.position;
            player.SetActive(true);
        }
    }
}
