using System.Collections.Generic;
using UnityEngine;

public class Previewer : MonoBehaviour
{

    public Player participant;
    
    private List<Field> _lastPath;

    /// <summary>
    /// From the current field of the participant, highlight the path from respectively selected card.
    /// </summary>
    /// <param name="card">selected card or null if nothing is selected</param>
    public void Preview(Card card)
    {
        HandlePath(card == null? 0: (int)card.steps, participant);
    }

    private void HandlePath(int steps, Player selectedPlayer){
        // find the path and highlight each field in the path 
        List<Field> path = selectedPlayer.FindPath(steps, selectedPlayer.currentField);
        // path.ForEach(p => p.GetComponent<Renderer>().material = p.DefaultMaterial);
        
        if (_lastPath != null)
        {
            // decolor each of the fields and set the last path back to null
            foreach (Field field in _lastPath){
                var rendField = field.GetComponent<Renderer>();
                rendField.material.color = field.DefaultColor; 
            }

            _lastPath = null; 
        }

        foreach (Field field in path){
            var rendField = field.GetComponent<Renderer>();
            rendField.material.color = new (138/255f, 46/255f, 46/255f); 
        }

        this._lastPath = path;
    }

}
