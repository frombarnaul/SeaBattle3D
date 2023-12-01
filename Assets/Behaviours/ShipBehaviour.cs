using Assets.Models;
using UnityEngine;

public class ShipBehaviour : MonoBehaviour
{
    public Ship Model;
    public GameObject MissPoint;

    public bool isRotatedSP;
    public int deckSize { get; private set; }
    public int damagedDecks;
    public bool IsDrowned;
    AudioSource[] sounds;

    void Start()
    {
        sounds = GetComponents<AudioSource>();
        deckSize = GetComponentsInChildren<Collider>().Length;
    }

    // Update is called once per frame
    public void Hited()
    {

        if (damagedDecks < deckSize - 1)
        {
            damagedDecks++;
            sounds[0].Play();
        }
            
        else Drown();
    }

    void Drown()
    {
        IsDrowned = true;
        sounds[1].Play();
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.GetComponent<MeshRenderer>().material.color = Color.black;
        }

        Debug.Log("SHIP DROWNED");

        DrawDrownedZone();
    }

    private void DrawDrownedZone()
    {
        var checkCenter = transform.position;

        for (float zPos = checkCenter.z - 1f; isRotatedSP ? zPos <= checkCenter.z + deckSize : zPos <= checkCenter.z + 1f; zPos++)
        {
            for (float xPos = checkCenter.x - 1f; isRotatedSP ? xPos <= checkCenter.x + 1f : xPos <= checkCenter.x + deckSize; xPos++)
            {
                var spritePos = new Vector3(xPos, 0, zPos);
                if (!Physics.CheckSphere(spritePos, 0.3f)
                    && spritePos.x != -1
                    && spritePos.x != 10
                    && spritePos.z != -1
                    && spritePos.z != 10
                    && spritePos.z != 12
                    && spritePos.z != 23)
                {
                    Instantiate(MissPoint, spritePos, Quaternion.identity);
                }
            }
        }
    }
}