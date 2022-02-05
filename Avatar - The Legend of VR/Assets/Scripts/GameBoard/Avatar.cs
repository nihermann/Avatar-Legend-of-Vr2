using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Avatar : Player
{
    public LevelOfMatch QuestionnaireMatch { get; set ; }
    // Start is called before the first frame update
    float speed = 4f; 

    public Animator _animator;

    void Start()
    {
        currentField = startingField;
        _animator = this.GetComponent<Animator>();
    }
    
    public IEnumerator Move(int steps, Player participant){
        
        if(isMoving) yield break; 

        isMoving = true; 

        try
        {
            currentPath = FindPath(steps, currentField);
        }
        catch (NullReferenceException)
        {
            Debug.Log($"No next in {currentPath.Last().name}");
        }
        
        if (_animator) 
            _animator.SetBool("isWalking", true);

        
        // move from each field in the path to the next one
        while(currentPath.Count > 0){
            var nextFieldInPath = currentPath[0];
            currentPath.RemoveAt(0);

            Vector3 nextPos = nextFieldInPath.position;
            
            // the next field is going to be occupied by the current player
            Quaternion nextRotation = nextFieldInPath.transform.rotation;

            // move to the next Field
            while (MoveToNextField(nextPos, nextRotation)) 
                yield return null;

            currentField = nextFieldInPath;

            if (currentField is OptionField)
            {
                break;
            };
        }

        while (LookToPlayer(participant))
            yield return null;
        
        // the player finished his path thus it's not longer moving
        if(_animator != null)
            _animator.SetBool("isWalking", false);
        currentPath = new();
        isMoving = false;

        if (currentField is OptionField)
            this.transform.rotation = currentField.transform.rotation;
            this._animator.SetFloat("Blend", Random.Range(0.5f, 6.0f) );
        onMoveExit?.Invoke();
    }

    bool MoveToNextField(Vector3 goal, Quaternion targetRotation){
        // moves this player to the given goal position

        // The step size is equal to speed times frame time.
        var step = 200f * Time.deltaTime;

        // Rotate our transform a step closer to the target's.
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);

        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, speed * Time.deltaTime));
    }

    private bool LookToPlayer(Player p)
    {
        var directionToPlayer = p.transform.position - transform.position;
        directionToPlayer.y = 0f;
        var rotToPlayer = Quaternion.LookRotation(directionToPlayer);
        
        // The step size is equal to speed times frame time.
        var step = 200f * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotToPlayer, step);
        return Quaternion.Angle(transform.rotation, rotToPlayer) < 1f;
    }

}
