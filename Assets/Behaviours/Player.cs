using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject FirstPlayerCam;
    public bool isAiming;
    public GameObject AimPrefab;

    public GameObject MissPoint;

    public GameController Controller;
    GameObject flyingAim;

    private void Awake()
    {
        isAiming = false;
    }
    public void Turn()
    {
        Invoke(nameof(Aiming), 0.5f);
    }

    private void Aiming()
    {
        flyingAim = Instantiate(AimPrefab);
        isAiming = true;
    }

    private void EndOfTurn()
    {
        Controller.NextTurn("Player2");
    }

    void Update()
    {
        if (isAiming)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            var aimRay = FirstPlayerCam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(aimRay, out float point))
            {
                Vector3 worldPoint = aimRay.GetPoint(point);
                int x = Mathf.RoundToInt(worldPoint.x);
                int z = Mathf.RoundToInt(worldPoint.z);

                if (x >= 0 && x < 10 && z <= 22 && z >= 13)
                {
                    flyingAim.transform.position = new Vector3(x, 0, z);

                    Shoot();
                }
                else
                {
                    flyingAim.transform.position = new Vector3(0, 100, 0);
                }

                
            }

        }
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Physics.Raycast(flyingAim.transform.position + new Vector3(0, 1), Vector3.down, out hitInfo);


            if (flyingAim.transform.position.x > 9
                || flyingAim.transform.position.x < 0
                || flyingAim.transform.position.z > 22
                || flyingAim.transform.position.z < 13
                || hitInfo.collider?.tag == "Shoted")
            {
                Debug.Log("Not available");
                GetComponent<AudioSource>().Play();
            }
            else
            {
                if (hitInfo.collider == null)
                {
                    Instantiate(MissPoint, flyingAim.transform.position, Quaternion.identity).GetComponent<AudioSource>().Play();

                    isAiming = false;
                    Destroy(flyingAim);
                    EndOfTurn();
                }
                else
                {
                    hitInfo.collider.tag = "Shoted";
                    hitInfo.collider.GetComponent<MeshRenderer>().enabled = true;
                    hitInfo.collider.GetComponent<MeshRenderer>().material.color = Color.red;
                    hitInfo.collider.GetComponentInParent<ShipBehaviour>().Hited();
                    Debug.Log("Hit!");
                    Controller.Player1Points++;
                    if(Controller.Player1Points >= 20)
                    {
                        Destroy(gameObject);
                        Destroy(flyingAim);
                        Controller.GameOver();
                    }
                }
            }
        }
    }
}
