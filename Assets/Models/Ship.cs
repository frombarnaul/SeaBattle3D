using Assets.Models.Enums;
using TMPro;
using UnityEngine;

namespace Assets.Models
{
    [CreateAssetMenu(fileName = "New Ship", menuName = "Ship/Create New Ship")]
    public class Ship : ScriptableObject
    {
        public int deckSize;
        public ShipType type;
    }
}
