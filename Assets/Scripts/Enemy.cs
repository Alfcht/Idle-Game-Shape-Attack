using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Enemy : MonoBehaviour
{
    public float MaxHP = 30f;
    public float Speed = 2f;
    public int Gold = 5;

    public Sprite[] EnemyShapes;

    public Slider HpBar;

    public float TowerContactDamage = 10f;

    public float TowerXThreshold = -8f;

    private float CurrentHP;
    private bool Poisoned;
    private bool IsDead;

    private Rigidbody2D Rb;
    private SpriteRenderer SpriteRenderer;
    private Color BaseColor;

    public TextMeshProUGUI DamageText;

    public Coroutine DamageRoutine;

    void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();

        if (DamageText == null)
            DamageText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        CurrentHP = MaxHP;
        UpdateHPBar();

        if (SpriteRenderer != null)
        {
            BaseColor = SpriteRenderer.color;

            if (EnemyShapes != null && EnemyShapes.Length > 0)
            {
                int RandomIndex = Random.Range(0, EnemyShapes.Length);
                SpriteRenderer.sprite = EnemyShapes[RandomIndex];
            }
        }
    }

    void FixedUpdate()
    {
        if (IsDead) return;

        Vector2 Step = Vector2.left * Speed * Time.fixedDeltaTime;

        if (Rb != null)
        {
            Rb.MovePosition(Rb.position + Step);
        }
        else
        {
            transform.Translate(Step);
        }

        if (transform.position.x < TowerXThreshold)
        {
            ReachTowerWithoutTrigger();
        }
    }

    public void TakeDamage(float Amount, bool IsCrit)
    {
        if (IsDead) return;

        CurrentHP -= Amount;
        UpdateHPBar();
        ShowDamageNumber(Amount, IsCrit);

        if (CurrentHP <= 0f)
            Die();
    }

    public void ApplyPoison(float TickDamage)
    {
        if (Poisoned || IsDead) return;
        StartCoroutine(PoisonRoutine(TickDamage));
    }

    IEnumerator PoisonRoutine(float TickDamage)
    {
        Poisoned = true;

        if (SpriteRenderer != null)
            SpriteRenderer.color = new Color(0.31f, 0.86f, 0.31f);

        float Elapsed = 0f;

        while (Elapsed < 3f && !IsDead)
        {
            yield return new WaitForSeconds(0.5f);
            TakeDamage(TickDamage, false);
            Elapsed += 0.5f;
        }

        if (!IsDead && SpriteRenderer != null)
            SpriteRenderer.color = BaseColor;

        Poisoned = false;
    }

    void OnTriggerEnter2D(Collider2D Col)
    {
        if (IsDead) return;
        if (!Col.CompareTag("TowerCore")) return;

        TryDamageTowerAndDespawn(Col.transform);
    }

    void OnCollisionEnter2D(Collision2D Collision)
    {
        if (IsDead) return;
        if (!Collision.collider.CompareTag("TowerCore")) return;

        TryDamageTowerAndDespawn(Collision.collider.transform);
    }

    void TryDamageTowerAndDespawn(Transform HitTransform)
    {
        Tower Tower = HitTransform.GetComponentInParent<Tower>();

        if (Tower == null)
            Tower = HitTransform.GetComponent<Tower>();

        if (Tower != null)
            Tower.TakeDamage(TowerContactDamage);

        DespawnAfterReachingTower();
    }

    void UpdateHPBar()
    {
        if (HpBar != null)
            HpBar.value = Mathf.Clamp01(CurrentHP / MaxHP);
    }

    void Die()
    {
        if (IsDead) return;

        IsDead = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddGold(Gold);
            GameManager.Instance.EnemiesLeft = Mathf.Max(0, GameManager.Instance.EnemiesLeft - 1);
        }

        if (AudioManager.Instance != null)
        AudioManager.Instance.PlayEnemyDeath();

        Destroy(gameObject);
    }

    void ReachTowerWithoutTrigger()
    {
        if (IsDead) return;

        Tower Tower = FindObjectOfType<Tower>();

        if (Tower != null)
            Tower.TakeDamage(TowerContactDamage);

        DespawnAfterReachingTower();
    }

    void DespawnAfterReachingTower()
    {
        if (IsDead) return;

        IsDead = true;

        if (GameManager.Instance != null)
            GameManager.Instance.EnemiesLeft = Mathf.Max(0, GameManager.Instance.EnemiesLeft - 1);

        Destroy(gameObject);
    }

    void ShowDamageNumber(float Amount, bool IsCrit)
    {
        if (DamageText == null) return;

        if (DamageRoutine != null)
            StopCoroutine(DamageRoutine);

        DamageRoutine = StartCoroutine(DamageNumberRoutine(Amount, IsCrit));
    }

    IEnumerator DamageNumberRoutine(float Amount, bool IsCrit)
    {
        DamageText.gameObject.SetActive(true);
        DamageText.text = IsCrit ? $"{Amount:0}" : $"{Amount:0}";
        DamageText.color = IsCrit ? Color.red : Color.white;
        DamageText.fontSize = IsCrit ? 35f : 25f;

        Vector3 StartPos = DamageText.transform.localPosition;
        Vector3 EndPos = StartPos + Vector3.up * 1.5f;

        float Duration = 0.5f;
        float T = 0f;

        while (T < Duration)
        {
            T += Time.deltaTime;
            float P = Mathf.Clamp01(T / Duration);
            DamageText.transform.localPosition = Vector3.Lerp(StartPos, EndPos, P);

            Color C = DamageText.color;
            C.a = 1f - P;
            DamageText.color = C;

            yield return null;
        }

        DamageText.transform.localPosition = StartPos;
        DamageText.gameObject.SetActive(false);
        DamageRoutine = null;
    }
}