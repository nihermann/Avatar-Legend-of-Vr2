using UnityEngine;

[ExecuteAlways]
public class DontRenderChildren : MonoBehaviour
{
    [Header("Should the children be rendered?")][SerializeField] private bool renderChildren;
    private bool _childrenActive;

    [Header("Check if all nexts are setup correctly")][SerializeField] private bool checkNexts;
    [Header("Check if gaol is reachable from every AvatarField")][SerializeField] private bool checkAvatarFields;

    private void Start()
    {
        _childrenActive = transform.GetComponentInChildren<Renderer>().enabled;
    }

    private void Update()
    {
        if (_childrenActive != renderChildren)
        {
            foreach (var rend in GetComponentsInChildren<Renderer>())
                rend.enabled = renderChildren;
            _childrenActive = renderChildren;
        }

        if (checkNexts)
        {
            CheckNexts(transform.GetChild(0).GetComponent<Field>());
            checkNexts = false;
        }

        if (checkAvatarFields)
        {
            var avFields = transform.GetComponentsInChildren<AvatarField>();
            if(avFields.Length == 0) Debug.Log("No avatar field as child! Good to go! :)");
            foreach (var child in avFields)
                CheckNexts(child);
            checkAvatarFields = false;
        }
    }

    private void CheckNexts(Field field)
    {
        var orig = field;
        while (field.nextField != null) field = field.nextField;
        if(field.CompareTag("GoalField")) Debug.Log("All fields are setup correctly! :)");
        else Debug.LogAssertion($"Last linked field was not tagged as \"GoalField\". Started from: {orig.name}. Last field was: {field.name}");
        checkNexts = false;
    }
}
