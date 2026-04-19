using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigLadyBoyBehavior : MonoBehaviour
{
    [SerializeField] public EnemySO enemy;
    [SerializeField] GameObject waterPrefab;
    [SerializeField] GameObject noticePrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        if (enemy == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemy.awarenessRadian);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemy.splashingRadian);
    }
}
