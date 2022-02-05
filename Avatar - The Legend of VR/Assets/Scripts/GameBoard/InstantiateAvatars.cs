using UnityEngine;

public class InstantiateAvatars : MonoBehaviour
{   
    [SerializeField] private OptionField[] avatarFields;
    [SerializeField] private GameObject avatarPrefab;
    
    private BuildAvatar _buildAvatar = new();

    public void Instantiate(ParticipantPreferences prefs, AvatarSetupInfos currentTrialAvatarSetupInfos)
    {
        for (var i = 0; i < avatarFields.Length; i++)
            avatarFields[i].questionnaireMatch = currentTrialAvatarSetupInfos.MatchChoices[i];
        foreach (var avatarField in avatarFields)
            _buildAvatar.Build(avatarPrefab, avatarField, prefs);
    }
    
}
