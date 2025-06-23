using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class WorldCharacter : MonoBehaviour
{
   

    protected virtual void Awake()
    {
    }

    protected virtual void Update()
    {
        UpdateMovement();
    }

    // 배틀씬으로 전환시 캐릭터 정보를 넘겨준다.
    protected abstract void UpdateMovement();
    
}
