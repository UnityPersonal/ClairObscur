using UnityEngine;

[System.Serializable]
public class BattleAttribute
{
    [SerializeField] string attributeName;
    [SerializeField] string attributeDescription;
    [SerializeField] int attributeValue;
    
    public string AttributeName
    {
        get => attributeName.ToLower();
        set => attributeName = value.ToLower();
    }
    
    public string AttributeDescription
    {
        get => attributeDescription;
        set => attributeDescription = value;
    }
    
    public int AttributeValue
    {
        get => attributeValue;
        set => attributeValue = value;
    }
}
