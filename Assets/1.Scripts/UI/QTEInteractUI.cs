using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QTEInteractUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private RectTransform clock;
    [SerializeField] private Animator animator;
    
    public bool IsSuccess { get; private set; } = false;
    
    public int spriteIndex = 0;
    public float timer = 0;
    public float duration = 2f; // Duration for each sprite to be displayed
    public float clockTimer = 0;
    
    private void OnEnable()
    {
        if (sprites.Length == 0)
        {
            Debug.LogError("No sprites assigned to QTEInteractUI.");
            return;
        }
     
        animator.Play("QTEStart");
        // Start the QTE coroutine
        StartCoroutine(StartQTECoroutine());
    }

    private IEnumerator StartQTECoroutine()
    {
        
        while (true)
        {
            if ((timer += Time.deltaTime) >= (duration / sprites.Length))
            {
                timer = 0;
                spriteIndex++;
                if (spriteIndex >= sprites.Length)
                {
                    spriteIndex = 0; // Reset to the first sprite
                }
                image.sprite = sprites[spriteIndex];
            }
            yield return null;
        }
    }
    
    private void Start()
    {
        image.sprite = sprites[spriteIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if((timer += Time.deltaTime) >= (duration / sprites.Length))
        {
            timer = 0;
            spriteIndex++;
            if (spriteIndex >= sprites.Length)
            {
                spriteIndex = 0; // Reset to the first sprite
            }
            image.sprite = sprites[spriteIndex];
        }
        
        clockTimer += Time.deltaTime;
        float GetDecimalPart(float number)
        {
            return number - Mathf.FloorToInt(number);
        }
        var normalizedTime = GetDecimalPart(clockTimer/ duration);
    }
}
