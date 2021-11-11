using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class EnemySpriteFlashRenderer : MonoBehaviour
{
    public static int sortingLayerInt = 0;
    public static int maxEnemySortingLayer = 9;
    public SpriteRenderer sprite;
    public Light2D light2D;
    bool prevVisible;
    private void Start()
    {
        List<SortingLayer> enemyLayers = new List<SortingLayer>();
        foreach (SortingLayer layer in SortingLayer.layers)
            if (layer.name.Contains("Enemy"))
                enemyLayers.Add(layer);
    }
    void Update()
    {
        if (prevVisible != sprite.isVisible && sprite.isVisible)
        {
            prevVisible = sprite.isVisible;
            sprite.sortingLayerName = "Enemy" + sortingLayerInt;
            light2D.ChangeTargetedSortingLayer(new int[] { SortingLayer.NameToID("Enemy" + sortingLayerInt) });
            sortingLayerInt++;
        }
        if (sortingLayerInt > maxEnemySortingLayer)
            sortingLayerInt = 0;
    }
    private void LateUpdate()
    {
        prevVisible = sprite.isVisible;
    }
}
