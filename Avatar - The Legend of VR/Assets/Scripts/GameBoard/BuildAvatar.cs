using System;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class BuildAvatar
{   
    
    private GameObject _avatar;
    private ParticipantPreferences _participantPreferences;
    private AvatarColors _avatarColors;
    private const string ShortHair = "hair0";
    private readonly string[] _middleHair = { "hair1", "hair2", "hair3", "hair4"};
    private readonly string[] _longHair= { "hair5", "hair6", "hair7"};

    // Start is called before the first frame update
    private void SetBasedOnPreferenceLevel(LevelOfMatch preferenceLevel, bool randomClothingStyle, bool randomHairLength, bool randomHat, bool randomGlasses)
    {
        SetClothing(randomClothingStyle, preferenceLevel);
        SetHair(randomHairLength, preferenceLevel, SetHat(randomHat));
        SetSkinColor(preferenceLevel);
        SetEyeColor(preferenceLevel);
        SetGlasses(randomGlasses);
    }
    
    public void Build(GameObject avatarPrefab, OptionField avatarField, ParticipantPreferences participantPreferences)
    {
        // make the avatar object and instantiate it at the position of the field
        _avatar = Object.Instantiate(avatarPrefab, avatarField.position, avatarField.quaternion).GameObject();
        LevelOfMatch questionnaireMatch = avatarField.questionnaireMatch;
        avatarField.avatar = _avatar;
        _avatar.GetComponent<Avatar>().QuestionnaireMatch = questionnaireMatch;
        _avatar.GetComponent<Avatar>().currentField = avatarField;
        
        _participantPreferences = participantPreferences;

        bool randomSwitch = Random.value > 0.5; 
        switch (questionnaireMatch)
        {
            case LevelOfMatch.Opposite:
                SetBasedOnPreferenceLevel(LevelOfMatch.Opposite,true, true, true, true);
                break;
                
            case LevelOfMatch.LessOpposite:
                SetBasedOnPreferenceLevel(LevelOfMatch.LessOpposite,true, true, randomSwitch, !randomSwitch);
                break;
            
            case LevelOfMatch.SlightMatch:
                SetBasedOnPreferenceLevel(LevelOfMatch.SlightMatch,true, true, randomSwitch, false);
                break;

            case LevelOfMatch.BetterMatch:
                SetBasedOnPreferenceLevel(LevelOfMatch.BetterMatch,true, false, false, false);
                break;

            case LevelOfMatch.FullMatch:
                SetBasedOnPreferenceLevel(LevelOfMatch.FullMatch,false, false, false, false);
                break;
        }
    }
    

    private void SetEyeColor(LevelOfMatch preferenceLevel)
    {
        // set skin color based on preferences
        Color eyeColor = AvatarColors.GetColorOnPreference(preferenceLevel, _participantPreferences.EyeColor, AvatarColors.Mode.EyeColor);
        FindChild("metarig/Global/Position/spine/spine.001/spine.002/spine.003/spine.004/spine.005/spine.006/face/eye.L/EyesL").GetComponent<Renderer>().materials[2].color = eyeColor;
        FindChild("metarig/Global/Position/spine/spine.001/spine.002/spine.003/spine.004/spine.005/spine.006/face/eye.R/EyesR").GetComponent<Renderer>().materials[2].color = eyeColor;

    }
    
    private bool SetHat(bool setBasedOnPreference)
    {
        switch (_participantPreferences.LikesHats)
        {
            case Opinion.Yes:
                if (!setBasedOnPreference){MakeActive("hat", RandomColor());}

                return true;
            case Opinion.Neutral:
            {
                if (Random.value > 0.5)
                {
                    MakeActive("hat", RandomColor());
                    return true;
                }

                break;
            }
            case Opinion.No:
                if (setBasedOnPreference)
                {
                    MakeActive("hat", RandomColor());
                    return true;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return false;
    }

    private void SetGlasses(bool setBasedOnPreference)
    {
        switch (_participantPreferences.LikesGlasses)
            {
                case Opinion.Yes:
                    if (!setBasedOnPreference){MakeActive("glasses");}
                    break;
                case Opinion.Neutral:
                {
                    if (Random.value > 0.5)
                    {
                        MakeActive("glasses");
                    }

                    break;
                }
                case Opinion.No:
                    if (setBasedOnPreference){MakeActive("glasses");}
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
    }
    
    private void SetClothing(bool randomClothingStyle, LevelOfMatch preferenceLevel)
    {
        // differentiate the two clothing types
        Enum clothingStyleTop, clothingStyleBottom;
        
        // if random set both of the two types of clothes to random types (random element of the enumeration of types)
        if (randomClothingStyle)
        {
            clothingStyleTop = GetRandomElement<ClothingStyle>();
            clothingStyleBottom = GetRandomElement<ClothingStyle>();
        }
        else
        {
            clothingStyleTop = clothingStyleBottom = _participantPreferences.ClothingStyle;

        }
        
        // set color based on preferences
        Color colorTop, colorBottom;
        colorTop = AvatarColors.GetColorOnPreference(preferenceLevel, _participantPreferences.FavouriteColor, AvatarColors.Mode.FavoriteColor);
        colorBottom = RandomFromList(new []{colorTop * (float) 0.3, Color.black, colorTop * (float) 1.1});

            // activate top clothing style (based on the preset preferences)
        switch (clothingStyleTop)
        {
            case ClothingStyle.Casual:
                MakeActive("casual_shirt", colorTop);
                break;
            case ClothingStyle.Chique:
                MakeActive("chique_shirt", colorTop);
                break;
            case ClothingStyle.Sporty:
                MakeActive("sporty_shirt", colorTop);
                break;
        }
        
        // activate bottom clothing style (based on the preset preferences)
        switch (clothingStyleBottom)
        {
            case ClothingStyle.Casual:
                MakeActive("casual_pants", colorBottom);
                break;
            case ClothingStyle.Chique:
                MakeActive("chique_pants", colorBottom);
                break;
            case ClothingStyle.Sporty:
                MakeActive("sporty_pants", colorBottom);
                break;
        }
    }
    
    private void SetSkinColor(LevelOfMatch preferenceLevel)
    {
        // set skin color based on preferences
        var skinColor = AvatarColors.GetColorOnPreference(preferenceLevel, _participantPreferences.SkinColor, AvatarColors.Mode.SkinColor);
        SetColor(skinColor, FindChild("avatar"));
    }

    private void SetHair(bool randomHairLength, LevelOfMatch preferenceLevel, bool hat)
    {
        // set hair length based on preferences
        var hairLength = randomHairLength ? GetRandomElement<HairLength>() : _participantPreferences.HairLength;
        
        // set color based on preferences
        var hairColor = AvatarColors.GetColorOnPreference(preferenceLevel, _participantPreferences.HairColor, AvatarColors.Mode.HairColor);
        
        switch (hairLength)
        {
            case HairLength.Short:
                MakeActive(ShortHair, hairColor);
                break;
            case HairLength.Middle:
                if (hat)
                {
                    MakeActive("hair1", hairColor);
                }
                else
                {
                    MakeActive(RandomFromList(_middleHair), hairColor);
                }
                break;
            default:
            {
                if (hat)
                {
                    MakeActive("hair5", hairColor);
                }
                else
                {
                    MakeActive(RandomFromList(_longHair), hairColor);
                }
                break;
            }
        }

    }

    private static Color RandomColor()
    {
        return Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f)*0.5f;
    }
    
    private static T RandomFromList<T>(T[] list)
    {
        return list[Random.Range(0, list.Length)];
    }
    
    private GameObject FindChild(string childName)
    {
        return _avatar.transform.Find(childName).gameObject;
    }
    
    private static void SetColor(Color color, GameObject child)
    {
        child.GetComponent<Renderer>().material.color = color;
    }
    
    private void MakeActive(string childName)
    {
        FindChild(childName).SetActive(true);
    }
    
    private void MakeActive(string childName, Color color)
    {
        GameObject child = FindChild(childName);
        child.SetActive(true);
        SetColor(color, child);
    }
    
    private static Enum GetRandomElement<T>()where T:Enum
    {
        var names = Enum.GetNames(typeof(T));
        return Enum.Parse<T>(names[Random.Range(0, names.Length)]);
    }

}
