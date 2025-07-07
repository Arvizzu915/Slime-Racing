using System.Collections.Generic;
using UnityEngine;

public class TrajectoryVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject dotPrefab;
    private List<GameObject> dots = new();

    Color color;
    [SerializeField] private float colorChangeRate;


    public void ShowTrajectory(List<Vector2> positions)
    {
        ClearDots();

        color = dotPrefab.GetComponent<SpriteRenderer>().color;

        foreach (var pos in positions)
        {
            GameObject dot = Instantiate(dotPrefab, pos, Quaternion.identity);
            
            color.a *= colorChangeRate;
            color.b *= colorChangeRate;
            color.r *= colorChangeRate;
            color.g *= colorChangeRate;
            dot.GetComponent<SpriteRenderer>().color = color; 
            dots.Add(dot);
        }
    }


    public void ClearDots()
    {
        foreach (var dot in dots)
            Destroy(dot);

        dots.Clear();
    }
}
