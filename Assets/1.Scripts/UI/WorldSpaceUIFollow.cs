using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceUIFollow : MonoBehaviour
{
    public BattleCharacter character;
    public Camera mainCamera; // 카메라
    public float scaleFactor = 1.0f; // 거리당 크기 비율
    public float minScale = 0.5f;
    public float maxScale = 2.0f;
    public Vector3 offset = new Vector3(0, 2f, 0); // 오브젝트 위에 위치하도록 오프셋

    private RectTransform rectTransform;
    Vector3 initialPosition;
    Canvas canvas;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>(true);
    }

    void LateUpdate()
    {
        var uiWorldPosition = character.Actor.transform.position + offset;
        transform.position = uiWorldPosition;
        transform.LookAt(mainCamera.transform);
        
        //transform.localScale = Vector3.one;
        // 1. 타겟 오브젝트 위에 UI 배치
        //rectTransform.anchoredPosition 
        //    = UIUtils.WorldToCanvasPosition(canvas, uiWorldPosition, mainCamera);
        //transform.LookAt(mainCamera.transform);
        //transform.forward = -transform.forward; // 반대로 돌려야 텍스트가 안 뒤집힘

        // 2. 카메라와의 거리 기반으로 크기 조정
        //float distance = Vector3.Distance(mainCamera.transform.position, uiWorldPosition);
        //float scale = Mathf.Clamp(1f / distance * scaleFactor, minScale, maxScale);
        //rectTransform.localScale = Vector3.one * scale;
        //slider.value = character.CurrentHp / character.MaxHp;
    }
}