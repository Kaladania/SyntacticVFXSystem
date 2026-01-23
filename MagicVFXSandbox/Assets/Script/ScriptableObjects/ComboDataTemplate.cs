using SnSECS;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboDataTemplate", menuName = "Scriptable Objects/ComboDataTemplate")]
public class ComboDataTemplate : ScriptableObject
{
    [SerializeField]
    int sectionIndex = 0; //index of section the combos will be loaded into

    List<List<Elements>> combos; //list of element combos

    //Holds:
    //Section Index
    //List of combos to spawn
}
