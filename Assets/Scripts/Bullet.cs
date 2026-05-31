using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public float Damage;
    [HideInInspector] public bool IsCrit;
    [HideInInspector] public Vector2 Direction;
    [HideInInspector] public string Attribute;
    [HideInInspector] public float AttrPower;

    public float Speed = 8f;

    private bool HasHit = false;

    void Start() => Destroy(gameObject, 4f);

    void Update() => transform.Translate(Direction * Speed * Time.deltaTime);

    void OnTriggerEnter2D(Collider2D Col)
    {
        if (HasHit) return;

        Enemy Enemy = Col.GetComponent<Enemy>();
        if (Enemy == null) return;

        HasHit = true;
        Enemy.TakeDamage(Damage, IsCrit);

        if (Attribute == "explosion") DoExplosion();
        else if (Attribute == "poison") Enemy.ApplyPoison(AttrPower);

        Destroy(gameObject);
    }

    void DoExplosion()
    {
        Collider2D[] Hits = Physics2D.OverlapCircleAll(
            transform.position, AttrPower, LayerMask.GetMask("Enemy"));

        foreach (var Hit in Hits)
        {
            Enemy E = Hit.GetComponent<Enemy>();
            if (E != null) E.TakeDamage(Damage * 0.5f, false);
        }
    }
}