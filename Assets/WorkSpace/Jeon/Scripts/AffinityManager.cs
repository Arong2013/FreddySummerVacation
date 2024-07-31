using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAffinity
{
    public int characterId;
    public int affinity;
}

public class AffinityManager : MonoBehaviour
{
    public List<CharacterAffinity> characterAffinities;

    public void AdjustAffinity(int characterId, int affinityChange)
    {
        var characterAffinity = characterAffinities.Find(ca => ca.characterId == characterId);
        if (characterAffinity != null)
        {
            characterAffinity.affinity += affinityChange;
            Debug.Log($"Character {characterId} affinity changed by {affinityChange}. New affinity: {characterAffinity.affinity}");
        }
        else
        {
            Debug.LogError($"Character with ID {characterId} not found.");
        }
    }
}
