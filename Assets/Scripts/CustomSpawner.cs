using Assets.Models;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CustomSpawner : MonoBehaviour
{
    public GameObject FirstPlayerCam;
    GameObject flyingShip;
    public Button startButton;
    AudioSource[] sounds;


    private void Awake()
    {
        sounds = GetComponents<AudioSource>();
    }

    void Update()
    {
        var aimRay = FirstPlayerCam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(1))
        {
            Physics.Raycast(aimRay, out hit);
            if (flyingShip == null && hit.collider != null)
            {
                if (hit.collider.GetComponentInParent<ShipBehaviour>()?.gameObject.tag == "team1")
                {
                    sounds[0].Play();
                    flyingShip = hit.collider.GetComponentInParent<ShipBehaviour>()?.gameObject;
                    startButton.enabled = false;
                }
            }
        }

        var groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (flyingShip != null && groundPlane.Raycast(aimRay, out float point))
        {
            Vector3 worldPoint = aimRay.GetPoint(point);
            int x = Mathf.RoundToInt(worldPoint.x);
            int z = Mathf.RoundToInt(worldPoint.z);

            flyingShip.transform.position = new Vector3(x, 0, z);

            var collides = flyingShip.GetComponentsInChildren<BoxCollider>();

            foreach (var collide in collides) collide.enabled = false;

            var flyingShipProps = flyingShip.GetComponent<ShipBehaviour>();
            var flyingShipRender = flyingShip.GetComponentsInChildren<MeshRenderer>();


            if (isPlaceFree(flyingShipProps))
            {
                foreach (var render in flyingShipRender) render.material.color = Color.green;
            }
            else
            {
                foreach (var render in flyingShipRender) render.material.color = Color.red;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (flyingShipProps.isRotatedSP)
                {
                    flyingShipProps.isRotatedSP = false;
                    flyingShip.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    flyingShipProps.isRotatedSP = true;
                    flyingShip.transform.rotation = Quaternion.Euler(0, -90, 0);
                }
                sounds[1].Play();
            }

            if (Input.GetMouseButtonDown(0) && isPlaceFree(flyingShipProps))
            {
                sounds[0].Play();
                foreach (var render in flyingShipRender) render.material.color = Color.white;
                foreach (var collide in collides) collide.enabled = true;
                flyingShip = null;
                startButton.enabled = true;
            }
        }
    }

    private bool isPlaceFree(ShipBehaviour flyingShipProps)
    {
        if (flyingShip.transform.position.x > 9
            || flyingShip.transform.position.x < 0
            || flyingShip.transform.position.z > 9
            || flyingShip.transform.position.z < 0)
            return false;

        for (int i = 0; i < flyingShipProps.deckSize; i++)
        {
            if (Physics.CheckBox(flyingShip.transform.position + (!flyingShipProps.isRotatedSP ? new Vector3(i, 0, 0) : new Vector3(0, 0, i)), new Vector3(1, 1, 1)))
                return false;
        }
        return true;
    }
}
