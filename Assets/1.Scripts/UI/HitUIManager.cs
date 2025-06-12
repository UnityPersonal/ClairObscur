using UnityEngine;

public class HitUIManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    
    [SerializeField] HitUI hitUIPrefab;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTakeDamage(TakeDamageEventArgs args)
    {
        var hitUI = Instantiate(hitUIPrefab, canvas.transform);
        var rt = hitUI.GetComponent<RectTransform>();
        rt.anchoredPosition = UIUtils.WorldToCanvasPosition(canvas, args.Target.transform.position);
    }
}
