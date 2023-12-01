using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class CPU : MonoBehaviour
{
    public GameController Controller;
    public GameObject MissPoint;
    public List<Vector3> availableTargets = new List<Vector3>();
    public bool hitedShipExist;
    public bool hitedShipTwiceExist;
    public Vector3 hitedShipPos, oldHitedShipPos;
    bool boosted;
    void Awake()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                availableTargets.Add(new Vector3(j, 0, i));
            }
        }
        boosted = false;
    }

    public void Turn()
    {
        Invoke("Shoot", 0.5f);
    }

   
    Vector3 GetShotPos()
    {
        if(!boosted && Controller.Player1Points - Controller.Player2Points > 4)
        {
            boosted = true;
            var randomShips = GameObject.FindGameObjectsWithTag("team1");
            while(true)
            {
                var random = UnityEngine.Random.Range(0, randomShips.Length);
                if (!randomShips[random].GetComponent<ShipBehaviour>().IsDrowned)
                    return randomShips[random].transform.position;
            }
        }
        else
        {
            if (hitedShipExist)
            {
                bool nearPointsAvailable = true;
                while (hitedShipExist && nearPointsAvailable)
                {
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = 1; j > -2; j--)
                        {
                            var point = hitedShipPos + new Vector3(i, 0, j);
                            if (availableTargets.Contains(point))
                            {
                                Debug.Log(point);
                                availableTargets.RemoveAt(availableTargets.IndexOf(point));
                                return point;
                            }
                        }
                    }
                    nearPointsAvailable = false;
                }
            }

            if (hitedShipExist)
            {
                bool nearPointsAvailable = true;
                while (hitedShipExist && nearPointsAvailable)
                {
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = 1; j > -2; j--)
                        {
                            var point = oldHitedShipPos + new Vector3(i, 0, j);
                            if (availableTargets.Contains(point))
                            {
                                availableTargets.RemoveAt(availableTargets.IndexOf(point));
                                return point;
                            }
                        }
                    }
                    nearPointsAvailable = false;
                }
            }

            int randIndex;
            randIndex = UnityEngine.Random.Range(0, availableTargets.Count);
            var shotPos = availableTargets[randIndex];
            availableTargets.RemoveAt(randIndex);

            return shotPos;
        }


    }

    private void Shoot()
    {
        Debug.Log("TRY SHOT" + "AVTARGS: " + availableTargets.Count);
        RaycastHit hitInfo;

        Vector3 shotPos = GetShotPos();
        
        

        Physics.Raycast(shotPos + new Vector3(0, 1), Vector3.down, out hitInfo);
        if (shotPos.z >= 0
            && shotPos.z < 10
            && shotPos.x >= 0
            && shotPos.x < 10
            && hitInfo.collider == null)
        {
            Instantiate(MissPoint, shotPos, Quaternion.identity).GetComponent<AudioSource>().Play();
            Invoke(nameof(EndOfTurn), 0.5f);

            return;
        }
        else
        {
            if (hitInfo.collider != null
                && hitInfo.collider.tag != "Shoted"
                && shotPos.z >= 0
                && shotPos.z < 10
                && shotPos.x >= 0
                && shotPos.x < 10)
            {
                var hitInfoProps = hitInfo.collider.GetComponentInParent<ShipBehaviour>();
                hitInfo.collider.tag = "Shoted";
                hitInfo.collider.GetComponent<MeshRenderer>().material.color = Color.red;
                hitInfoProps.Hited();
                if (hitInfoProps.IsDrowned)
                {
                    hitedShipExist = false;
                    hitedShipTwiceExist = false;
                }
                else
                {
                    if (hitedShipExist && !hitedShipTwiceExist)
                    {
                        oldHitedShipPos = hitedShipPos;
                        hitedShipTwiceExist = true;
                    }
                    hitedShipExist = true;
                    hitedShipPos = shotPos;
                }
                
                RemoveNearPoints(shotPos);
                Controller.Player2Points++;
                if (Controller.Player2Points >= 20)
                {
                    Destroy(gameObject);
                    Controller.GameOver();
                }
                Debug.Log("Hit!");
            }
            else
            {
                Shoot();
                return;
            }
        }
        Invoke("Shoot", 0.5f);
        return;
    }



    void RemoveNearPoints(Vector3 damagedPoint)
    {
        for (int i = -1; i < 2; i = i + 2)
        {
            for (int j = -1; j < 2; j = j + 2)
            {
                var point = damagedPoint + new Vector3(i, 0, j);
                if (availableTargets.Contains(point))
                {
                    availableTargets.RemoveAt(availableTargets.IndexOf(point));
                }
            }
        }
    }

    void EndOfTurn()
    {
        Controller.NextTurn("Player1");
    }
}
