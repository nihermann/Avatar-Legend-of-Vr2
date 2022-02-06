using System.Collections.Generic;
using UnityEngine;

public struct ParticipantPreferences
{
    public int Age { get; set; }

    public Color SkinColor { get; set; }

    public Color HairColor { get; set; }
    public HairLength HairLength { get; set; }

    public Color EyeColor { get; set; }
    public ClothingStyle ClothingStyle { get; set; }

    public Color FavouriteColor { get; set; }

    public Opinion LikesGlasses { get; set; }
    
    public Opinion LikesHats { get; set; }
    
    public IEnumerable<string> Record()
    {
        return SnapshotTypeDefs.CreateSnapshot(
            Age,
            SkinColor,
            HairColor,
            HairLength,
            EyeColor,
            ClothingStyle,
            FavouriteColor,
            LikesGlasses,
            LikesHats);
    }

    public IEnumerable<string> Header()
    {
        return SnapshotTypeDefs.CreateHeaders(
            new(nameof(Age), Age.GetType()),
            new(nameof(SkinColor), SkinColor.GetType()),
            new(nameof(HairColor), HairColor.GetType()),
            new(nameof(HairLength), HairLength.GetType()),
            new(nameof(EyeColor), EyeColor.GetType()),
            new(nameof(ClothingStyle), ClothingStyle.GetType()),
            new(nameof(FavouriteColor), FavouriteColor.GetType()),
            new(nameof(LikesGlasses), LikesGlasses.GetType()),
            new(nameof(LikesHats), LikesHats.GetType())
        );
    }
}


public enum HairLength
{
    Short,
    Middle,
    Long
}


public enum ClothingStyle
{
    Casual,
    Chique,
    Sporty
}

public enum Opinion
{
    Yes,
    No,
    Neutral
}
