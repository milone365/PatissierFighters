using UnityEngine;
using UnityEngine.UI;
public class SV_EnemyManager : MonoBehaviour
{
    public SV_Tower tower;       // Reference to the player's heatlh.
    public GameObject[] enemy;                // The enemy prefab to be spawned.
    public float spawnTime = 4f;            // How long between each spawn.
    public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
    int enemyIndex = 0;
    bool getRand = false;
    bool gameIsEnd = false;
    int killCount = 0;
    int score = 0;
   // Text scoretext=null;
    
    public int getScore()
    {
        return score;
    }
    public void INIT()
    {
      //  scoretext = GameObject.Find("Score").GetComponent<Text>();
       // scoretext.text = "Score: " + score;
        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        InvokeRepeating("Spawn", spawnTime, spawnTime);
        
    }


    void Spawn()
    {
        // If the player has no health left...
        if (tower.getIsDestroyed()||gameIsEnd) {
            // ... exit the function.
            return;
        }

        // Find a random index between zero and one less than the number of spawn points.
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        if (getRand)
        {
            randomIndex();
        }
        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        GameObject newEnemy= Instantiate(enemy[enemyIndex], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation)as GameObject;
        newEnemy.GetComponent<SV_EnemyHealth>().passManager(this);
        newEnemy.GetComponent<SV_EnemyController>().INITIALIZE(tower);
       
        
    }
    void randomIndex()
    {
        int rnd = Random.Range(0, enemy.Length);
        enemyIndex = rnd;
    }
    public void decreaseSpawnTime()
    {
        spawnTime--;
        enemyIndex++;
    }
    public void addKillCount(int _score)
    {
        killCount++;
        score += _score;
       
        if (killCount == 20)
        {
            decreaseSpawnTime();
        }
        if (killCount == 40)
        {
            spawnTime--;
            getRand = true;
        }
        if (killCount == 60)
        {
            getRand = false;
            enemyIndex = 2;
        }
        if (EffectDirector.instance != null)
            EffectDirector.instance.SurvivalPopUp(_score);
    }
    public void endGame()
    {
        gameIsEnd = true;
        SV_EnemyController[] allenemy = FindObjectsOfType<SV_EnemyController>();
        foreach(var e in allenemy)
        {
            if(e!=null)
            Destroy(e.gameObject);
        }
    }
    public void getRandomIndex()
    {
        getRand = true;
    }
}

