using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class InstantiateAvatars : MonoBehaviour
{   
    [SerializeField] private AvatarField[] avatarFields;
    [SerializeField] private GameObject avatarPrefab;
    [SerializeField] private Material eyeMaterial;
    // [SerializeField] private ParticipantPreferences _participantPreferences;

    
    private BuildAvatar _buildAvatar = new ();

    // Start is called before the first frame update
    void Start()
    {
        // foreach (var avatarField in avatarFields)
        // {
        //     _buildAvatar.Build(avatarPrefab, eyeMaterial, avatarField, _participantPreferences);
        // }
    }

    public void Instantiate(ParticipantPreferences prefs, AvatarSetupInfos currentTrialAvatarSetupInfos)
    {
        for (var i = 0; i < avatarFields.Length; i++)
            avatarFields[i].questionnaireMatch = currentTrialAvatarSetupInfos.MatchChoices[i];
        foreach (var avatarField in avatarFields)
            _buildAvatar.Build(avatarPrefab, eyeMaterial, avatarField, prefs);
    }
    
}
