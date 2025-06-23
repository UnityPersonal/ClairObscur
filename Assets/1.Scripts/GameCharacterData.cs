using System.Collections.Generic;

[System.Serializable]
public class GameCharacterData
{
    public string CharacterName;
    public CharacterStatus Status = new CharacterStatus();
}