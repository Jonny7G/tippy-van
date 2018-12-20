using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ParallaxBackground : MonoBehaviour
{
    [SerializeField]private Poolable background;
    [SerializeField] private float leftBreakPointY, rightBreakPointY;
    [SerializeField] private float absoluteMinX, absoluteMaxX, absoluteMinY, absoluteMaxY;
    [Header("References")]
    [SerializeField] private FloatReference worldSpeed;
    [SerializeField] private Vector3Reference worldDirectionCoords;
    [SerializeField] private WorldDirectionReference worldDirection;
    [SerializeField] private BoolReference gameActive;
    private int key;
    private ObjectPooling pooler;
    private List<ParallaxObject> activeBackgrounds=new List<ParallaxObject>();

    private ParallaxObject recentBackground;
    
    private void Start()
    {
        pooler = ObjectPooling.instance;

        key = pooler.GetUniqueID();
        pooler.SetPool(background, key, 10);

        Poolable obj = pooler.GetFromPool(key);
        obj.transform.position = transform.position;
        recentBackground = obj.GetComponent<ParallaxObject>();
        activeBackgrounds.Add(recentBackground);
    }
    private void Update()
    {
        if (gameActive.value)
        {
            for (int i = activeBackgrounds.Count - 1; i >= 0; i--)
            {
                activeBackgrounds[i].transform.position += worldDirectionCoords.value * Time.deltaTime * worldSpeed.value * 0.3f;

                if (activeBackgrounds[i].transform.position.x > absoluteMaxX || activeBackgrounds[i].transform.position.x < absoluteMinX || activeBackgrounds[i].transform.position.y > absoluteMaxY || activeBackgrounds[i].transform.position.y < absoluteMinY)
                {
                    activeBackgrounds[i].GetComponent<Poolable>().EndReached();
                    activeBackgrounds.Remove(activeBackgrounds[i]);
                    return;
                }
            }
            switch (worldDirection.direction)
            {
                case WorldDirection.left:
                    if (recentBackground.transform.position.y > leftBreakPointY)
                    {
                        ParallaxObject parObj = pooler.GetFromPool(key).GetComponent<ParallaxObject>();
                        activeBackgrounds.Add(parObj);

                        parObj.transform.position = recentBackground.BottomConnection;
                        recentBackground = parObj;
                    }
                    break;
                case WorldDirection.right:
                    if (recentBackground.transform.position.y > rightBreakPointY)
                    {
                        ParallaxObject parObj = pooler.GetFromPool(key).GetComponent<ParallaxObject>();
                        activeBackgrounds.Add(parObj);

                        Vector3 offset = parObj.transform.position - parObj.LeftConnection;
                        parObj.transform.position = offset + recentBackground.BottomConnection;
                        recentBackground = parObj;
                    }
                    break;
            }
        }
    }
}