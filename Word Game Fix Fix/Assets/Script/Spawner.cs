using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    Transform point1;
    Transform point2;
    bool spawning = true;
    float counter = 0f;
    float counter2 = 0f;
    float spawnTime = 3f;
    float spawnTime2 = 3f;
    float lastYPos = 1000f;

    ObjectPooler objP;



    // Start is called before the first frame update
    void Start()
    {
        objP = ObjectPooler.Instance;

        point1 = transform.GetChild(0);
        point2 = transform.GetChild(1);

        spawnTime = Random.Range(2, 4);
        spawnTime2 = Random.Range(2, 4);

    }

    // Update is called once per frame
    void Update()
    {
        if (spawning)
        {
            counter += Time.deltaTime;
            counter2 += Time.deltaTime;

            if (counter > spawnTime)
            {
                SpawnLetter();

                counter = 0f;
                spawnTime = Random.Range(1, 2);
            }
            if(counter2 > spawnTime2)
            {
                SpawnLetter();

                counter2 = 0f;
                spawnTime2 = Random.Range(2.5f, 4);

                
            }
        }
    }

    private void SpawnLetter()
    {
        Vector3 spawnPos = new Vector3(point1.position.x, Random.Range(point1.position.y, point2.position.y), point1.position.z);

        if (lastYPos <= (spawnPos.y + 1) && lastYPos >= (spawnPos.y - 1))
        {
            if (Mathf.Abs(((spawnPos.y + 1) - lastYPos)) > Mathf.Abs(((spawnPos.y - 1) - lastYPos)) && spawnPos.y + 1 < point1.position.y)
            {
                spawnPos = new Vector3(spawnPos.x, spawnPos.y + 1, spawnPos.z);
            }
            else
            {
                spawnPos = new Vector3(spawnPos.x, spawnPos.y - 1, spawnPos.z);
            }
        }

        lastYPos = spawnPos.y;

        objP.SpawnFromPool("Letter", spawnPos, point1.transform.rotation);
    }
}
