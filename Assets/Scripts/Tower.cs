using UnityEngine;
using System.Collections.Generic;
using System.Collections;   

public class Tower : MonoBehaviour
{
    public Sprite[] ShapeSprites;
    public GameObject BulletPrefab;

    float AttackDamage => GameManager.Instance != null ? 10f + ((GameManager.Instance.LvlDamage - 1) * 2f) : 10f;
    float AttackSpeed => GameManager.Instance != null ? 1f + ((GameManager.Instance.LvlSpeed - 1) * 0.2f) : 1f;
    float MaxHP => GameManager.Instance != null ? 100f + ((GameManager.Instance.LvlHP - 1) * 25f) : 100f;
    float CritChance => GameManager.Instance != null ? 5f + ((GameManager.Instance.LvlCritChance - 1) * 3f) : 5f;
    float CritMultiplier => GameManager.Instance != null ? 1.5f + ((GameManager.Instance.LvlCritDamage - 1) * 0.1f) : 1.5f;
    float AttrPower => GameManager.Instance != null ? 5f + ((GameManager.Instance.LvlAttribute - 1) * 1.5f) : 5f;

    public float GetMaxHP() => MaxHP;

    private float CurrentHP;
    private float AttackTimer;
    private List<Enemy> EnemiesInRange = new List<Enemy>();
    private SpriteRenderer Sr;

    private Animator aniMater;

    void Start()
    {
        Sr = GetComponent<SpriteRenderer>();
        CurrentHP = MaxHP;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.TowerHP = CurrentHP;
            GameManager.Instance.TowerMaxHP = CurrentHP;
        }

        if (GameManager.Instance != null)
        {
            int Idx = GameManager.Instance.ChosenShape;
            if (Idx < ShapeSprites.Length)
                Sr.sprite = ShapeSprites[Idx];
            Sr.color = GameManager.Instance.ChosenColor;
        }

        aniMater = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        AttackTimer += Time.deltaTime;

        if (AttackTimer >= 1f / AttackSpeed)
        {
            AttackTimer = 0f;
            Enemy Target = GetNearestEnemy();
            if (Target != null)
                FireBullet(Target);
        }
    }

    void FireBullet(Enemy Target)
    {
        if (BulletPrefab == null) return;

        GameObject Go = Instantiate(BulletPrefab, transform.position, Quaternion.identity);

        if (AudioManager.Instance != null)
        AudioManager.Instance.PlayBulletLaunch();

        Bullet B = Go.GetComponent<Bullet>();
        if (B == null) return;

        bool IsCrit = Random.Range(0f, 100f) < CritChance;
        B.Damage = IsCrit ? AttackDamage * CritMultiplier : AttackDamage;
        B.IsCrit = IsCrit;
        B.Direction = (Target.transform.position - transform.position).normalized;
        B.Attribute = GameManager.Instance.ChosenAttribute;
        B.AttrPower = AttrPower;

        SpriteRenderer Bsr = Go.GetComponent<SpriteRenderer>();
        if (Bsr != null)
            Bsr.color = GameManager.Instance.ChosenColor;

        if (aniMater != null)
        {
            aniMater.SetBool("isAttacking", true);
            StartCoroutine(ResetAttackAnim());
        }

        if (BulletPrefab == null)
            return;
    }

    Enemy GetNearestEnemy()
    {
        EnemiesInRange.RemoveAll(E => E == null);
        Enemy Nearest = null;
        float MinDist = float.MaxValue;

        foreach (var E in EnemiesInRange)
        {
            float D = Vector2.Distance(transform.position, E.transform.position);
            if (D < MinDist)
            {
                MinDist = D;
                Nearest = E;
            }
        }

        return Nearest;
    }

    public void TakeDamage(float Amount)
    {
        CurrentHP -= Amount;

        if (GameManager.Instance != null)
            GameManager.Instance.TowerHP = Mathf.Max(0f, CurrentHP);

        if (CurrentHP <= 0f)
            GameOver();
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        if (GameManager.Instance != null)
            GameManager.Instance.GoToMenu();
    }

    void OnTriggerEnter2D(Collider2D Col)
    {
        Enemy E = Col.GetComponent<Enemy>();
        if (E != null && !EnemiesInRange.Contains(E))
            EnemiesInRange.Add(E);
    }

    void OnTriggerExit2D(Collider2D Col)
    {
        Enemy E = Col.GetComponent<Enemy>();
        if (E != null)
            EnemiesInRange.Remove(E);
    }

    IEnumerator ResetAttackAnim()
    {
        yield return null;
            if (aniMater != null)
                aniMater.SetBool("isAttacking", false);
    }
}