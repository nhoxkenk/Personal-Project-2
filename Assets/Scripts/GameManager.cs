using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private System.Random random = new System.Random();

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnPlayer(List<Tile> lastTilesTouched = null)
    {
        Vector3 vector = Vector3.zero;
        bool flag = false;
        if (lastTilesTouched != null)
        {
            int num = lastTilesTouched.Count - 1;
            while (num > -1 && !flag)
            {
                Tile tile = lastTilesTouched[num];
                if (tile != null)
                {
                    vector = new Vector3(tile.transform.position.x, 0.362f, tile.transform.position.z);
                    flag = true;
                }
                num--;
            }
        }
        if (!flag)
        {
            List<Vector3> landTileCoordinates = GetLandTileCoordinates();
            vector = landTileCoordinates[random.Next(0, landTileCoordinates.Count - 1)];
            Vector3 vector3 = vector;
            Debug.Log("Respawn randomly at " + vector3.ToString());
        }
        Debug.Log(vector);
        player.transform.position = vector;
    }

    public List<Vector3> GetLandTileCoordinates()
    {
        List<Vector3> list = new List<Vector3>();
        GameObject[] tiles = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject tile in tiles)
        {
            Vector3 tilePos = new Vector3(tile.transform.position.x, 0.362f, tile.transform.position.z);
            list.Add(tilePos);
        }

        return list;
    }
}
