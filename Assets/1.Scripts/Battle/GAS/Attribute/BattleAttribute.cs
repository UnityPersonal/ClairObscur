using UnityEngine;

[System.Serializable]
public class BattleAttribute
{
    [SerializeField] string attributeName;
    [SerializeField] string attributeDescription;
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
            character.Callbacks.OnAttributeChanged?.Invoke();
        }
    }
}
