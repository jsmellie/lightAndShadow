using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StageProgressionTracker
{
    public const int InvalidStageID = -1;
    public static readonly Zones[] ZoneOrder = new Zones[]
    {
        Zones.URBAN,
        Zones.FOREST,
        Zones.MOUNTAIN
    };
    public static readonly Dictionary<Zones,int>MaxStagesPerZone = new Dictionary<Zones, int>()
    {
        {Zones.URBAN, 2},
        {Zones.FOREST, 3},
        {Zones.MOUNTAIN, 4}
    };

    public static void GetNextStageInfo(Zones zone, int stageIndex, out Zones nextZone, out int nextStageIndex)
    {
        nextZone = zone;
        nextStageIndex = InvalidStageID;

        if (stageIndex != InvalidStageID)
        {
            int currentZoneIndex = -1;

            for(int i = 0; i < ZoneOrder.Length; ++i)
            {
                if (ZoneOrder[i] == zone)
                {
                    currentZoneIndex = i;
                    break;
                }
            }

            if (currentZoneIndex >= 0)
            {
                int maxStageCount = MaxStagesPerZone[zone];

                if (maxStageCount <= stageIndex && currentZoneIndex + 1 < ZoneOrder.Length)
                {
                    nextZone = ZoneOrder[currentZoneIndex + 1];
                    nextStageIndex = 1;
                }
                else
                {
                    nextStageIndex = stageIndex + 1;
                }
            }
        }
    }
}
