using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockProporties", menuName = "ScriptableObjects/BlockProporties", order = 1)]
public class BlockProporties: ScriptableObject
{

    public string name;
    public Sprite texture;
    public List<Sprite> destroySprites;
    public float timeToDestroy;
    public ParticleSystem destroyParticles;

}
