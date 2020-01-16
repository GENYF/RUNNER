using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour {

    public List<GameObject> obstacleList;
    public Transform nowObstacle01, nowObstacle02;
    public Transform first;
    public int obstacleCount = 0;

    public float missileTime = 10.0f;
    public GameObject missile;
    public GameObject warn;

    private Transform garbage;

    public bool isStop = false;

    public float speed = 5.0f;

	void Start () {
        obstacleCount = obstacleList.Count;
        garbage = new GameObject("garbage").transform;
    }

	void Update ()
    {
        if (isStop)
            return;

        if (missileTime > 0)
            missileTime -= Time.deltaTime;
        else
        {
            missileTime = Random.Range(10.0f,20.0f);
            switch (Random.Range(0, 3))
            {
                case 0:
                    GameObject newMissile = Instantiate(missile, new Vector3(-35, 3, 1), Quaternion.identity);
                    GameObject newWarn = Instantiate(warn, new Vector3(4, 3.2f, 1), Quaternion.Euler(0, -90, 0));
                    newMissile.transform.SetParent(garbage);
                    newWarn.transform.SetParent(garbage);
                    break;
                case 1:
                    newMissile = Instantiate(missile, new Vector3(-35, 3, -1), Quaternion.identity);
                    newWarn = Instantiate(warn, new Vector3(4, 3.2f, -1), Quaternion.Euler(0,-90,0));
                    newMissile.transform.SetParent(garbage);
                    newWarn.transform.SetParent(garbage);
                    break;
            }
        }

        if (nowObstacle01 != null)
            nowObstacle01.position += Vector3.left * speed * Time.deltaTime;
        if (nowObstacle02 != null)
            nowObstacle02.position += Vector3.left * speed * Time.deltaTime;

        if (nowObstacle01 != null && nowObstacle01.position.x < -50)
        {
            Destroy(nowObstacle01.gameObject);
            nowObstacle01 = nowObstacle02;
            SetObstacle(nowObstacle02.position.x);
        }
    }

    public void SetObstacle(float x)
    {
        nowObstacle02 = Instantiate(obstacleList[Random.Range(0, obstacleCount)], transform).transform;
        nowObstacle02.position = new Vector3(x + 50, 1.5f, 0);
    }

    public void ResetAll()
    {
        if (nowObstacle01 != null)
            Destroy(nowObstacle01.gameObject);
        if (nowObstacle02 != null)
            Destroy(nowObstacle02.gameObject);

        nowObstacle01 = Instantiate(first, transform).transform;
        nowObstacle01.localPosition = new Vector3(0, 0, 0);
        nowObstacle02 = Instantiate(first, transform).transform;
        nowObstacle02.localPosition = new Vector3(50, 0, 0);

        isStop = false;

        Destroy(garbage.gameObject);
        garbage = new GameObject("garbage").transform;
    }
}
