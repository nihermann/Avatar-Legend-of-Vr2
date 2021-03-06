using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerSFX))]
public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public Field startingField;

    public Field currentField;

    public List<Field> currentPath;

    private Avatar _companion;

    public UnityEvent onGoalFieldReached;
    public UnityEvent onAvatarSelectFieldReached;
    public UnityEvent onMoveExit;

    private PlayerSFX _playerSfx;


    private void Start()
    {
        currentField = startingField;
        var transform1 = startingField.transform;
        var position = transform1.position;
        this.transform.position = new Vector3(position.x, this.transform.position.y,
            position.z);
        _playerSfx = GetComponent<PlayerSFX>();
    }

   public List<Field> FindPath(int steps, Field pathStartingField){
        List<Field> path = new List<Field>();

        // get the path starting with the Field the piece is currently on
        Field currentFieldInPath = pathStartingField;

        for (int i = 0; i < steps; i++)
        {
            // for each step find the corresponding field and add to the path list
            if (currentFieldInPath.CompareTag("GoalField"))
                return path;

            currentFieldInPath = currentFieldInPath.nextField;
            path.Add(currentFieldInPath);
        }

        // return the path aka the list with all fields that are going to be visited
        return path;
    }


    public void Move(int steps)
    {

        try
        {
            currentPath = FindPath(steps, currentField);
        }
        catch (NullReferenceException)
        {
            Debug.Log($"No next in {currentPath.Last().name}");
        }

        // move from each field in the path to the next one
        while(currentPath.Count > 0){
            currentField = currentPath[0];
            currentPath.RemoveAt(0);

            if (currentField is AvatarField)
            {
                Teleport(currentField);
                onAvatarSelectFieldReached?.Invoke();
                return;
            }

        }
        Teleport(currentField);


        if (currentField.CompareTag("GoalField"))
        {
            _playerSfx.PlayGoal();
            _playerSfx.StopBackGround();
            onGoalFieldReached?.Invoke();
        }
        else onMoveExit?.Invoke();
    }

    private void Teleport(Field field)
    {
        _playerSfx.PlayTeleport();
        // the position of the player to the field position plus its height
        var fieldTransform = field.transform;
        transform.position = new Vector3(fieldTransform.position.x, transform.position.y, fieldTransform.position.z);

        // set the player orienetation to the rotation of the player to keep path direction
        transform.rotation = fieldTransform.rotation;
    }

}
