using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public List<ShipBehaviour> ships = new List<ShipBehaviour>();
    bool team2 = false;

    public void SpawnCPUShips()
    {
        team2 = true;
        Invoke("SpawnShips", 1);
    }
    public void SpawnShips()
    {
        if(!team2)
        {
            foreach (var ship in GameObject.FindGameObjectsWithTag("team1"))
            {
                DestroyImmediate(ship);
            }
        }
        
        
        foreach (ShipBehaviour ship in ships)
            SpawnShip(ship);
    }

    private void SpawnShip(ShipBehaviour ship)
    {
        var counter = 0;
        while (counter < (int)ship.Model.type)
        {
            var shipStartPos = new Vector3(Random.Range(0, 10), 0, Random.Range(team2 ? 13 : 0, team2 ? 23 : 10));
            bool isPlaceFree = true;
            bool isRotated = Random.Range(0, 2) == 0 ? false : true;

            for (int i = 0; i < ship.Model.deckSize; i++)
            {
                if (Physics.CheckBox(shipStartPos + new Vector3(isRotated ? 0 : i, 0, isRotated ? i : 0), new Vector3(1, 1, 1)))
                {
                    isPlaceFree = false;
                    break;
                }
            }

            if (isPlaceFree)
            {
                var spawnedShip = Instantiate(ship, shipStartPos, isRotated ? Quaternion.Euler(0, -90, 0) : Quaternion.identity);
                if (!team2) spawnedShip.tag = "team1";
                else
                {
                    spawnedShip.tag = "team2";
                    foreach(MeshRenderer render in spawnedShip.GetComponentsInChildren<MeshRenderer>())
                    {
                        render.enabled = false;
                    }
                }
                spawnedShip.GetComponent<ShipBehaviour>().isRotatedSP = isRotated;
                counter++;
            }
        }
        GetComponent<AudioSource>().Play();
    }
}
