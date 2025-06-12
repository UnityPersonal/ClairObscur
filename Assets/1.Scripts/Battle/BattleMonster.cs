using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BattleMonster : BattleCharacter
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OnAttack(AttackEventArgs args)
    {
        if(args.Target.Equals(this) == true)
        {
            Debug.Log($"{args.Attacker.name} attacked {name} for {args.Damage} damage.");
            animator.SetTrigger("Hit");
            
            BattleEventManager.OnTakeDamage(
                new TakeDamageEventArgs(this, args.Damage)
                );
        }
    }

    // Update is called once per frame
    public override void OnEmittedBeginAttackSignal()
    {
        BattleEventManager.OnAttack(new AttackEventArgs(10, this, targetCharacter));
    }

    public override BattleCharacterType CharacterType => BattleCharacterType.Enemy;

    public override BattleCharacter TargetCharacter
    {
        get { return targetCharacter; }
    }


    protected override IEnumerator UpdateBattleActionCoroutine()
    {
        yield return StartCoroutine(SelectTargetCoroutine());
        
        yield return StartCoroutine(AttackCoroutine());
        
        
        Activated = false;
    }

    private BattleCharacter targetCharacter = null;

    private IEnumerator SelectTargetCoroutine()
    {
        var playerList = BattleManager.Instance.CharacterGroup[BattleCharacterType.Player];
        foreach (var character in playerList)
        {
            if(character.IsDead == false)
            {
                targetCharacter = character;
                break;
            }
        }
        // Logic to select a target for the monster
        yield return null; // Placeholder for actual logic
    }
    
    private IEnumerator AttackCoroutine()
    {
        while (Input.GetKeyDown(KeyCode.Space) == false)
        {
            yield return null;
        }
        
        // 1. TimelineAsset 가져오기
        var timeline = director.playableAsset as TimelineAsset;
        // 2. 모든 트랙 순회
        foreach (var track in timeline.GetOutputTracks())
        {
            // MoveToTargetTrack만 처리
            if (track is MoveToTargetTrack)
            {
                // 3. 트랙의 모든 클립 순회
                foreach (var clip in track.GetClips())
                {
                    // MoveToTargetClip만 처리
                    var moveClip = clip.asset as MoveToTargetClip;
                    if (moveClip != null)
                    {
                        // 4. actor, target 동적 할당
                        moveClip.actor.exposedName = UnityEditor.GUID.Generate().ToString();
                        moveClip.target.exposedName = UnityEditor.GUID.Generate().ToString();

                        if (clip.displayName.Equals("GoTo"))
                        {
                            director.SetReferenceValue(moveClip.actor.exposedName, characterDefaultLocation);
                            director.SetReferenceValue(moveClip.target.exposedName, targetCharacter.CharacterHitTransform);
                        }
                        else if (clip.displayName.Equals("ReturnTo"))
                        {
                            director.SetReferenceValue(moveClip.actor.exposedName, targetCharacter.CharacterHitTransform);
                            director.SetReferenceValue(moveClip.target.exposedName, characterDefaultLocation);
                        }
                        else
                        {
                            Debug.LogWarning($"clip displayName '{clip.displayName}' does not match expected names.");
                        }
                        
                    }
                }
            }
        }
        
        director.Play();
        yield return WaitForTimeline(director);
        
        Activated = false; 
    }
    
    public static IEnumerator WaitForTimeline(PlayableDirector director)
    {
        bool isDone = false;

        void OnStopped(PlayableDirector _) => isDone = true;

        director.stopped += OnStopped;

        // 이미 재생 중인 경우만 대기
        if (director.state == PlayState.Playing)
            yield return new WaitUntil(() => isDone);

        director.stopped -= OnStopped;
    }
}
