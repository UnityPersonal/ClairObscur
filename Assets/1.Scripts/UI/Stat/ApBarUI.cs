using UnityEngine;

public class ApBarUI : MonoBehaviour, IWorldUI
{
    [SerializeField] private GameObject[] apTileList;
    
    public void SetAp(int ap)
    {
        for (int i = 0; i < apTileList.Length; i++)
        {
            apTileList[i].SetActive(i < ap);
        }
    }

    public RectTransform RectTransform => transform as RectTransform; // Implementing IWorldUI interface
}
