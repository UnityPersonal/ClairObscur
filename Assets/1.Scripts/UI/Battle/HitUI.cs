using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class HitUI : MonoBehaviour, IWorldUI
{
    private Transform attacher;
    [SerializeField] float animationSpeed = 1f; // Speed of the animation
    [SerializeField] float life = 1f; // life of ui
    private Vector3 offset = new Vector3(0, 0, 0); // Offset to position the UI above the character
    private RectTransform rectTransform;
    private Canvas canvasComponent;
    [SerializeField] TMP_Text damageText;
    public AudioSource audioSource;
    
    private Vector3 attachPosition;
    
    private Camera mainCamera;
    
    private void Awake()
    {
        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();
    }
    

    public void Set(Canvas canvas, Transform attacher, string damage)
    {
        this.canvasComponent = canvas;
        this.attacher = attacher;
        attachPosition = attacher.position;
        damageText.text = damage;
    }

    // Update is called once per frame
    protected void LateUpdate()
    {
        life -= Time.deltaTime;
        if (life <= 0)
        {
            Destroy(gameObject);
            return;
        }
        
        offset += Vector3.up * (Time.deltaTime * animationSpeed) ;
        transform.position = attachPosition + offset;
        
        transform.forward = mainCamera.transform.forward;
        //rectTransform.anchoredPosition = UIUtils.WorldToCanvasPosition(canvasComponent, attachPosition + offset);
    }

    public RectTransform RectTransform => transform as RectTransform;
}
