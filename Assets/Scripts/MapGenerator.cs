using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
#if UNITY_EDITOR
    public Level targetLevel;

    [Header("LEVEL CONFIGS")]
    public float length = 250f;
    public float height = 25f;
    public float segmentLength = 1f;
    public float emptyChance = 0.25f;
    public FinishStand finishPrefab;
    public List<StandInfo> standPrefabs;

    [Header("ITEM CONFIGS")]
    public float itemChance = 0.5f;
    public List<ItemInfo> itemPrefabs;

    private Transform standParent;
    private Transform itemParent;

    [ContextMenu("Random")]
    public void RandomMap()
    {
        if (targetLevel != null) DestroyImmediate(targetLevel.gameObject);

        var levelObj = new GameObject("Level");
        levelObj.transform.SetParent(transform.parent, false);
        levelObj.transform.position = Vector3.zero;

        var standParentObj = new GameObject("Stands");
        standParentObj.transform.SetParent(levelObj.transform, false);
        standParentObj.transform.position = Vector3.zero;

        standParent = standParentObj.transform;

        var itemsParentObj = new GameObject("Items");
        itemsParentObj.transform.SetParent(levelObj.transform, false);
        itemsParentObj.transform.position = Vector3.zero;

        itemParent = itemsParentObj.transform;
        
        targetLevel = levelObj.AddComponent<Level>();

        var stands = CreateStands();

        var finishObj = PrefabUtility.InstantiatePrefab(finishPrefab.gameObject) as GameObject;
        var finish = finishObj.GetComponent<FinishStand>();

        finish.transform.SetParent(standParent.transform, false);
        finish.transform.position = stands[stands.Count - 1].EndPosition;

        var items = CreateItemsWithStands(stands);

        targetLevel.Init(stands, items, finish);
    }

    private List<Stand> CreateStands()
    {
        var z = 0f;
        var prevPosition = Vector3.zero;
        var prevIsEmtpy = true;

        var stands = new List<Stand>();

        while (z < standParent.transform.position.z + length)
        {
            if (!prevIsEmtpy)
            {
                var empty = Utils.RandomHelper.Chance01(emptyChance);
                if (empty)
                {
                    z += UnityEngine.Random.Range(5, 10);
                    prevIsEmtpy = true;
                    continue;
                }
            }

            var stand = GetRandomStand();
            stand.transform.SetParent(standParent.transform, false);

            var y = (int)UnityEngine.Random.Range(1, height);

            var position = new Vector3(0, y, z);
            stand.Init(position);

            z = stand.EndPosition.z;

            prevIsEmtpy = false;
            prevPosition = stand.EndPosition;

            stands.Add(stand);
        }

        return stands;
    }

    private List<ItemGroup> CreateItemsWithStands(List<Stand> stands)
    {
        var items = new List<ItemGroup>();

        for (int i = 0; i < stands.Count; i++)
        {
            var stand = stands[i];
            if (stand == null) continue;

            var shouldAddItem = Utils.RandomHelper.Chance01(itemChance);
            if (!shouldAddItem) continue;

            var position = stand.transform.position;
            position.z += 1.5f;

            var group = GetRandomListItems(position, itemParent);
            items.Add(group);
        }

        return items;
    }

    private Stand GetRandomStand()
    {
        var weights = standPrefabs.Select(x => x.weight).ToList();
        var index = Utils.RandomHelper.WeightedChoice(weights);

        var prefabInfo = standPrefabs[index];
        var standObj = PrefabUtility.InstantiatePrefab(prefabInfo.prefab.gameObject) as GameObject;
        var stand = standObj.GetComponent<Stand>();

        return stand;
    }

    private ItemGroup GetRandomListItems(Vector3 position, Transform parent)
    {
        var items = new List<Item>();

        var weights = itemPrefabs.Select(x => x.weight).ToList();
        var index = Utils.RandomHelper.WeightedChoice(weights);

        var prefabInfo = itemPrefabs[index];
        var number = Utils.XRandom.Range(prefabInfo.minNumber, prefabInfo.maxNumber);

        var group = new GameObject("ItemGroup");
        group.transform.SetParent(parent);
        group.transform.position = position;
        var itemGroup = group.AddComponent<ItemGroup>();

        for (int i = 0; i < number; i++)
        {
            var obj = PrefabUtility.InstantiatePrefab(prefabInfo.prefab.gameObject) as GameObject;
            var item = obj.GetComponent<Item>();

            var pos = Vector3.zero;
            pos.y = pos.y + i * item.height;

            item.transform.SetParent(group.transform, false);
            item.transform.localPosition = pos;
        
            items.Add(item);
        }

        itemGroup.items = items;

        return itemGroup;
    }

    [Serializable]
    public class StandInfo
    {
        public Stand prefab;
        public float weight;
    }

    [Serializable]
    public class ItemInfo
    {
        public Item prefab;
        public float weight;
        public int minNumber;
        public int maxNumber;
    }
#endif
}
