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
    
    public bool IsSuccess => (endTime - startTime) >= successTime;
    
    public int spriteIndex = 0;
    public float timer = 0;
    public float duration = 2f; // Duration for each sprite to be displayed
    public float clockTimer = 0;

    public float successTime = 0.2f; // Time required to successfully complete the QTE
    public float startTime = 0;
    public float endTime = 0;
    public float InputTime = 0;
    
    
    private void OnEnable()
    {
        if (sprites.Length == 0)
        {
            Debug.LogError("No sprites assigned to QTEInteractUI.");
            return;
        }

        startTime = Time.time;
        spriteIndex = 0;
        timer = 0;
        clockTimer = 0;

        animator.Play("QTE_Start",0, 0);
        // Start the QTE coroutine
        //StartCoroutine(StartQTECoroutine());
    }

    private void OnDisable()
    {
        QTESystem.Instance.UnRegistQTE(this);
    }

    private void Start()
    {
        image.sprite = sprites[spriteIndex];
    }

    // Update is called once per frame
    public void EndQTE(float endTime)
    {
        InputTime = endTime;
        animator.Play("QTE_End", 0, 0);
    }
    
    void Update()
    {
        /*if((timer += Time.deltaTime) >= (duration / sprites.Length))
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
        var normalizedTime = GetDecimalPart(clockTimer/ duration);*/
    }
}
