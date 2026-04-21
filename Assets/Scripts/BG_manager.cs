using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_manager : MonoBehaviour
{
    public GameObject BGPrefab;
    public List<GameObject> BGPrefspawned;
    public Transform BGspawnerCheck;
    private GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
