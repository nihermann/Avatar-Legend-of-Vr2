using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;




/// <summary>
/// Preloads the questionnaire scene until a <see cref="Button"/> was pressed from the UI to signal the start of the questionnaire.
/// </summary>
[RequireComponent(typeof(UIDocument))]

public class WelcomeScene : MonoBehaviour
{
    private TextField _id;

    void Start()
    {
        // // Preload the questionnaire scene until the user starts with the experiment.
        // var loadSceneAsync = SceneManager.LoadSceneAsync("Questionaire", LoadSceneMode.Single);
        // loadSceneAsync.allowSceneActivation = false;

        _id = GetComponent<UIDocument>().rootVisualElement.Q<TextField>("participantID");

        // Release the async loaded scene after the start button was pressed.
        GetComponent<UIDocument>()
                .rootVisualElement
                .Q<Button>("start-experiment")
                .clickable
                .clicked += OnStartClicked; //loadSceneAsync.allowSceneActivation = true;
    }


    public void OnStartClicked()
    {
        if (!WasChanged(_id)) return;
        ExperimentController.Instance.ParticipantID = Convert.ToInt32(_id.value);
        ExperimentController.TransitionToNextScene();
    }

    private bool WasChanged(TextField field) => !string.IsNullOrEmpty(field.value);
}
