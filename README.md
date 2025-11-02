# 🎭 ClairObscur

[![ClairObscur Demo](https://img.youtube.com/vi/O6d7ws8WySM/0.jpg)](https://youtu.be/O6d7ws8WySM?si=_J8h_vFc5XiN5QCe)

---

## 📖 Project Overview [프로젝트 개요]

- **클레르 옵스퀴르 33 원정대** 전투 시스템 모작 프로젝트  
- **Unity 3D** 로 제작  
- **개발 기간:** 2025.6 ~ 2025.6 (3주)  

---

## ✨ Key Features [주요 기능]

### ⚔️ Battle Character
- **BattleCharacter:** 몬스터,캐릭터등 전투 클래스
- (Base)[https://github.com/UnityPersonal/ClairObscur/blob/main/Assets/1.Scripts/Battle/Character/BattleCharacter.cs]
- (Player)[https://github.com/UnityPersonal/ClairObscur/blob/main/Assets/1.Scripts/Battle/Player/BattlePlayer.cs]
- (Monster)[https://github.com/UnityPersonal/ClairObscur/blob/main/Assets/1.Scripts/Battle/Monster/BattleMonster.cs]

### ⚔️ Battle Action
- **BattleAction:** 다양한 스킬 연출 및 전투 시스템을 **Timeline** 기반으로 통합 관리  
- **Signal Asset** 기반으로 전투와 연출을 상호작용  

### 🎭 Actor
- Timeline 에서 동적으로 변경된 연출을 Actor 시스템을 통해 적용 가능  

### 🪄 Skill
- 태그 기반 스탯 조회를 통해 스킬 효과 부여  

### 🧩 Manager
- **BattleEventManager:** 전투 상호작용 이벤트 관리  

---

## 🛠 Tech Stack [기술 스택]

- **C#**  
- **Unity 6000.0.50f1**  
- **ScriptableObject** (연출, 스킬)  
- **Timeline** (전투 연출)  
- **State Pattern** (전투 캐릭터 액션 제어)  
- **Coroutine** (턴제 관리)  
- **GitHub** (형상 관리)  
