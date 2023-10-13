using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI coinText;

    private System.Random random = new System.Random();
    public int coin = 0;

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
                    vector = new Vector3(tile.transform.position.x, 0.34f, tile.transform.position.z);
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
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject tile in tiles)
        {
            Transform tileTransform = tile.transform;
            Vector3 tilePos = new Vector3(tileTransform.position.x, 0.34f, tileTransform.position.z);
            list.Add(tilePos);
        }

        return list;
    }

    public void GetCoin(int amount)
    {
        coin += amount;
        coinText.text = $"{coin}";
    }

    public void SetCoin(int amount)
    {
        coin -= amount;
        coinText.text = $"{coin}";
    }

}
