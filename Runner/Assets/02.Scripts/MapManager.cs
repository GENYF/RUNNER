using UnityEngine;

public class MapManager : MonoBehaviour {

    private Transform[] roadList ;
    private int roadCount;
    public float speed = 5;
    public float width = 5;

    public bool isStop = true;

    void Start()
    {
        roadCount = transform.childCount;
        roadList = new Transform[roadCount];
        for (int i = 0; i < roadCount; i++)
            roadList[i] = transform.GetChild(i);
    }

	void Update () {
        if (isStop)
            return;

        for (int i = 0; i < roadCount; i++)
        {
            roadList[i].position += Vector3.left * speed * Time.deltaTime;

            if (roadList[i].position.x < 0)
                roadList[i].position += Vector3.right * roadCount * width;
        }
	}
}
