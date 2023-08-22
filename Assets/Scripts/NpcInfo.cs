using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInfo : MonoBehaviour
{
    public enum Name
    {
        ANNASMITH,
        JACKSMITH,
        FELIXREED,
        HENRYDAVIS,
        BENJAMINGREEN
    }

    public enum Occupation
    {
        FARMER,
        LUMBERJACK,
        INNKEEPER,
        INNKEEPERDAUGHTER,
        BAKER
    }

    [SerializeField] public Name npcName;
    [SerializeField] public Occupation npcOccupation;

    public string GetNpcInfo()
    {
        return $"NPC Name: {npcName.ToString()}\n" + $"NPC Occupation: {npcOccupation.ToString()}";
    }
}
