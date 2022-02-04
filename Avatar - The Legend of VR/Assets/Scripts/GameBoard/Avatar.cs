using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    public new IEnumerator Move(int steps){
        
        if(isMoving || isSelecting)
        {
            yield break; 
        }

        isMoving = true; 

        // if the path is not already specified calculate it
        if(currentPath.Count == 0){
            currentPath = FindPath(steps, currentField);
        }
        
        if (_animator) _animator.SetBool("isWalking", true);

        
        // move from each field in the path to the next one
        while(currentPath.Count > 0){
            var nextFieldInPath = currentPath[0];
            

            if((currentPath.Count == 1 || nextFieldInPath is AvatarField) && _animator)
            {
                _animator.SetBool("isWalking", false);
            }

            Vector3 nextPos = nextFieldInPath.position; 
            
            // the next field is going to be occupied by the current player
            Quaternion nextRotation = nextFieldInPath.transform.rotation;

            // move to the next Field
            while (MoveToNextField(nextPos, nextRotation)){yield return null;}
            //yield return new WaitForSeconds(0.1f);

            this.currentField = nextFieldInPath;

            if (currentField is AvatarField)
            {
                // TODO: stop and allow for avatar selection
                onAvatarSelectFieldReached?.Invoke();
                yield break;
            }

            currentPath.RemoveAt(0);
        }

        // the player finished his path thus it's not longer moving
        isMoving = false;         
    }

    bool MoveToNextField(Vector3 goal, Quaternion targetRotation){
        // moves this player to the given goal position

        // The step size is equal to speed times frame time.
        var step = 200f * Time.deltaTime;

        // Rotate our transform a step closer to the target's.
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);

        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, speed * Time.deltaTime));
    }

}
