using UnityEngine;

public class EnergyDrinkProjectile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy"))
        {

            Destroy(collision.gameObject);

            Destroy(gameObject);
        }
    }
}