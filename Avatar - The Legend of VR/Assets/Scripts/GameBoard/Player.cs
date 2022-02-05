using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isMoving; 
    public bool isSelecting; 

    public Field startingField;

    public Field currentField; 

    public List<Field> currentPath;

    private Avatar _companion;

    public UnityEvent onGoalFieldReached;
    public UnityEvent onAvatarSelectFieldReached;
    public UnityEvent onMoveExit;


   void Start()
    {
        currentField = startingField;
    }

   public List<Field> FindPath(int steps, Field pathStartingField){
        List<Field> path = new List<Field>(); 

        // get the path starting with the Field the piece is currently on
        Field currentFieldInPath = pathStartingField; 

        Field nextField;


        for (int i = 0; i < steps; i++)
        {
            // for each step find the corresponding field and add to the path list
            nextField = currentFieldInPath.nextField;
            path.Add(nextField);
            currentFieldInPath = nextField;
        }

        this.currentPath = path; 

        // return the path aka the list with all fields that are going to be visited
        return path;
    }
   

    public void Move(int steps)
    {
        if(isMoving || isSelecting)
        {
            return; 
        }
        
        if(currentPath.Count == 0){
            currentPath = FindPath(steps, currentField);
        }

        // move from each field in the path to the next one
        while(currentPath.Count > 0){
            currentField = currentPath[0];

            if (currentField is AvatarField )
            {
                transform.position = currentField.position;
                onAvatarSelectFieldReached?.Invoke();
                // if (currentPath.Count > 1 && _companion._animator)
                // {
                //     _companion._animator.SetBool("isWalking", true);
                // }
                return;
            }
            
            currentPath.RemoveAt(0);
        }

        transform.position = currentField.position;
        if (_companion != null)
        {
            StartCoroutine(_companion.Move(steps));
        }
        onMoveExit?.Invoke();
        currentPath = null;
    }

}