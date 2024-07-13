using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnner : MonoBehaviour
{

    //Khai bao class dot
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroup;
        public int waveQuota;//so luong trong mot dot
        public float spawnDelay; // thoi gian spawn
        public int spawnCount; // so quai da spawn
    }

    //Khai bao class Group ke dich
    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount; // tong so quai 
        public int spawnCount; // so quai da spawn ra 
        public GameObject enemyPrefabs; // prefab con quai
    }

    
    public List<Wave> waves; //tao list cac wave
    public int currentWaveCount; //the INDEX of current waves ( start from 0) 
    Transform player; // take owner location when owner is FAR to enemy, then move the enemy to near owner by TELEPORT enemy to around owner area


    [Header("Spawnner Attribute")]
    float spawnTimer; // bo dem de spawn quai
    public int enemiesAlive; // so luong ke thu HIEN TAI
    public int maxEnemiesAllowed; // tong so luong duoc Hien Thi
    public bool maxEnemiesReach=false; // check xem da du enemy chua
    public float waveDelay; // thoi gian delay khi chuyen qua Wave moi
    bool isWaveActive; // check wave hien tai

    [Header("Spawn Position")]
    public List<Transform> relativeSpawnPoints; // List vi tri enemy spawn , dua tren vi tri owner de PHAN BO xung quanh theo tung vi tri trong List nay

     void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuota();
    }
     void Update()
    {
        // Neu wave hien tai = / != wave cuoi  +  wave chi vua moi Bat dau, wave chua duoc danh dau kich hoat
        if (currentWaveCount< waves.Count && waves[currentWaveCount].spawnCount==0 && !isWaveActive)
        {
            isWaveActive = true; // wave dc danh dau dang kich hoat
            StartCoroutine(BeginNextWave()); // dung isWaveActive de ngan can viec startroutine dang doi wavedelay thi update no lai goi them mot startroutine 1 wave khac va ghi de current wave
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= waves[currentWaveCount].spawnDelay)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }
    IEnumerator BeginNextWave()
    {
        isWaveActive = true;
        yield return new WaitForSeconds(waveDelay); // wait waveDelay giay truoc khi qua wave tiep theo
        if (currentWaveCount<waves.Count-1)
        {
            isWaveActive= false;
            currentWaveCount++;
            CalculateWaveQuota();
        }
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroup) 
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }
        waves[currentWaveCount].waveQuota = currentWaveQuota;
       // Debug.LogWarning(currentWaveQuota);
    }


    void SpawnEnemies()
    {
        // neu so luong quai hien tai < total quai & tong so quai duoc cho phep hien thi
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota &&!maxEnemiesReach)
        {
            foreach(var enemygroup in waves[currentWaveCount].enemyGroup)
            {
                // cho moi loai quai ( vd Bat la 1 loai quai, Red Bat la mot loai quai khac)
                //neu so luong hien tai chua max thi spawn het ra
                if (enemygroup.spawnCount < enemygroup.enemyCount)
                {
                   //spawn ra 1 con o mot trong cac vi tri duoc gan vao xung quanh nguoi choi
                    Instantiate(enemygroup.enemyPrefabs, player.position + relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);


                    enemygroup.spawnCount++;//tang them 1 con quai da spawn ra
                    waves[currentWaveCount].spawnCount++; // tang 1 tren tong luong quai da spawn trong 1 wave
                    enemiesAlive++; // tang 1 ke thu song sot ( vua moi sinh ra)
                    if (enemiesAlive >= maxEnemiesAllowed) // check neu con quai vua voi spawn ra la du chi tieu so enemy dc hien thi tren map chua?,
                                                           // ... neu roi thi true de cai if o dong 94 bi false va khong spawn quai nua
                    {
                        maxEnemiesReach = true;
                        return;
                    }

                }
            }
        }
       
    }
    //quai bi killed thi goi ham nay de giam so ke dich dang hien song sot & so ke dich da max chua -> ham spawnEnemy o dong 94 dc true
    public void OnEnemyKilled()
    {
        enemiesAlive--;

        if (enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReach = false;
        }
    }
}
