using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class BattleAttribute
{
    [TableColumnWidth(20)]
    [SerializeField] string attributeName;
    [TableColumnWidth(20)]
    [SerializeField] string attributeDescription;
    [TableColumnWidth(20)]
    [SerializeField] int attributeValue;

    BattleCharacter character;
    public void BindCharacter(BattleCharacter character)
    {
        this.character = character;
    }
    
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

        set
        {
            attributeValue = value;
            if(character != null)
                character.Callbacks.OnAttributeChanged?.Invoke();
        }
    }

    public void IncrementAttributeValue(int i)
    {
        AttributeValue += i;
    }

    public void DecrementAttributeValue(int i)
    {
        AttributeValue -= i;
    }
}
