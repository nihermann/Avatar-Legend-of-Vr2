using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Previewer : MonoBehaviour
{

    public Player participant;
    
    private Transform _selection; 

    private List<Field> lastPath;

    public int steps;

    public bool lastWasCard;

    private void Start()
    {
        lastWasCard = false;
    }

    /// <summary>
    /// From the current field of the participant, highlight the path from respectively selected card.
    /// </summary>
    /// <param name="card">selected card or null if nothing is selected</param>
    public void Preview(Card card)
    {
        handlePath(card == null? 0: (int)card.steps, participant);
    }

    private void handlePath(int _steps, Player selectedPlayer){
        // find the path and highlight each field in the path 
        List<Field> path = selectedPlayer.FindPath(_steps, selectedPlayer.currentField);
        // path.ForEach(p => p.GetComponent<Renderer>().material = p.DefaultMaterial);

        if (steps == 0)
        {

            if (lastPath != null)
            {
                // decolor each of the fields and set the last path back to null
                foreach (Field field in lastPath){
                    var rendField = field.GetComponent<Renderer>();
                    rendField.material.color = field.DefaultColor; 
                }
    
                lastPath = null; 
            }
            
        }
        
        foreach (Field field in path){
            var rendField = field.GetComponent<Renderer>();
            rendField.material.color = new (138/255f, 46/255f, 46/255f); 
        }

        this.lastPath = path;
    }

}
