using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectMenu : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private Button selectArcherButton;
    [SerializeField] private Button selectWarriorButton;
    [SerializeField] private CharacterSkillMenu characterSkillMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        exitButton.onClick.AddListener(() => {
            gameObject.SetActive(false);
        });
        
        selectArcherButton.onClick.AddListener(() =>
        {
            characterSkillMenu.gameObject.SetActive(true);
            characterSkillMenu.Setup("Archer");
            gameObject.SetActive(false);
        });
        selectWarriorButton.onClick.AddListener(() =>
        {
            characterSkillMenu.gameObject.SetActive(true);
            characterSkillMenu.Setup("Warrior");
            gameObject.SetActive(false);
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }
}
