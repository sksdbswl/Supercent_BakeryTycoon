using System.Collections.Generic;
using UnityEngine;

public class ShowCaseManager : Singleton<ShowCaseManager>
{
    public List<Showcase> Showcases = new List<Showcase>();

    public Showcase GetRandomShowcase()
    {
        if (Showcases.Count == 0) return null;
        int index = Random.Range(0, Showcases.Count);
        return Showcases[index];
    }
}
