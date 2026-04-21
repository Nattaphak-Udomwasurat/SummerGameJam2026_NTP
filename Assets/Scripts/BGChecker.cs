using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGChecker : MonoBehaviour
{
    public float PlayerCheckerRadius = 10f;
    public GameObject Self;
    public bool IsPlayeratEdge,Isspawned = false;
    public GameObject BGPrefab;
  
    
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        Self = gameObject;
        IsPlayeratEdge = CheckProximity("Player");
        if (IsPlayeratEdge && Isspawned == false)
        {
            SpawnBG();
            Isspawned = true;
        }
    }

    private bool CheckProximity(string tag)
    {
        // Use Physics2D for 2D projects
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, PlayerCheckerRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }
    private void SpawnBG()
    {
        Instantiate(BGPrefab, transform.position, Quaternion.identity);

        // Spawn itself offset on the X axis by PlayerCheckerRadius
        Vector3 newPosition = transform.position + new Vector3(PlayerCheckerRadius, 0f, 0f);
        Instantiate(Self, newPosition, Quaternion.identity);
    }
    // Optional: Visualize the radius in the Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, PlayerCheckerRadius);
    }
    
}
  

