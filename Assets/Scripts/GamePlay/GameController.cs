using System;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField] private EnemyMovement EnemyPrefab;
    [SerializeField] private Transform PlayerLocation;
    [SerializeField] private GameObject PlatformPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        System.Random rnd = new System.Random();
        for(int i = 0; i < 180;i += rnd.Next(5,10))
        {
            for(int j = 0; j < 4; j++)
            {
                if(rnd.Next(1,3) == 1)
                {
                    Vector2 platformLocation = new Vector2(i, j*rnd.Next(2,4));
                    Instantiate(PlatformPrefab,platformLocation,Quaternion.identity);
                }
            }
        }
        float r = rnd.Next(0,1001)/1000f;
        float g = rnd.Next(0,1001)/1000f;
        float b = rnd.Next(0,1001)/1000f;
        SpriteRenderer backgroundTile = GameObject.Find("BackgroundTile").GetComponent<SpriteRenderer>();
        backgroundTile.color = new Color( r, g, b, 1);
    }

    // Update is called once per frame
    void Update()
    {
        bool enemiesLeft = GameObject.Find("Enemy") == null ? false : true;
        if(!enemiesLeft)
        {
            System.Random rnd = new System.Random();
            int enemiesToCreate = rnd.Next(1,6);
            int distanceBetweenEnemies = rnd.Next(3,11);
            for(int i = 0; i < enemiesToCreate; i++)
            {
                Vector2 placeEnemy = new Vector2(PlayerLocation.position.x + (20 + i * distanceBetweenEnemies), PlayerLocation.position.y + 80);
                Instantiate(EnemyPrefab,placeEnemy,PlayerLocation.rotation);
            }
        }
    }
}
