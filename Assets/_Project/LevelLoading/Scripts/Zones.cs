using System;

public enum Zones
{
    URBAN = 1,
    FOREST,
    MOUNTAIN
}

public static class StageUtility
{
    public static void GetZoneAndStageFromString(string str, out Zones zone, out int stageID)
    {
        zone = (Zones)Enum.Parse(typeof(Zones), str.Split('_')[0].ToUpper());
        stageID = Int16.Parse("" + str[str.Length - 1]);
    }
}
