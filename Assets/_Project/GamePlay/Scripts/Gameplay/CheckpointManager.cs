using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : SingletonBehaviour<CheckpointManager>
{

    private string CHECKPOINT_KEY = "CHECKPOINT";
    private int _currentCheckpoint = 0;

    public int CurrentCheckpoint
    {
        get { return _currentCheckpoint; }
    }

    protected override void Initialize()
    {
        _currentCheckpoint = PlayerPrefs.GetInt(CHECKPOINT_KEY,0);
    }

    public void SaveCheckpoint(int checkpoint)
    {
        if(checkpoint > _currentCheckpoint)
        {
            _currentCheckpoint = checkpoint;
            PlayerPrefs.SetInt(CHECKPOINT_KEY,checkpoint);
            PlayerPrefs.Save();
        }
    }

    [ContextMenu("Reset Progress")]
    public void ResetProgress()
    {
        _currentCheckpoint = 0;
        PlayerPrefs.SetInt(CHECKPOINT_KEY,0);
        PlayerPrefs.Save();
    }


    private Dictionary<int,string> CHECKPOINT_SCENE_MAP = new Dictionary<int, string>
    {
       {0,"IntroScene"},
       {1,"URBAN_Stage1,URBAN_Stage2"},
       {2,"URBAN_Stage1,URBAN_Stage2,URBAN_Stage3"},
       {3,"URBAN_Stage2,URBAN_Stage3,URBAN_Stage4"},
       {4,"URBAN_Stage3,URBAN_Stage4,URBAN_Stage5"},
       {5,"URBAN_Stage4,URBAN_Stage5"},
       {6,"COUNTRY_Stage1,COUNTRY_Stage2"},
       {7,"COUNTRY_Stage1,COUNTRY_Stage2,COUNTRY_Stage3"},
       {8,"COUNTRY_Stage2,COUNTRY_Stage3,COUNTRY_Stage4"},
       {9,"COUNTRY_Stage3,COUNTRY_Stage4,COUNTRY_Stage5"},
       {10,"COUNTRY_Stage4,COUNTRY_Stage5"},
       {11,"FOREST_Stage1,FOREST_Stage2"},
       {12,"FOREST_Stage1,FOREST_Stage2,FOREST_Stage3"},
       {13,"FOREST_Stage2,FOREST_Stage3,FOREST_Stage4"},
       {14,"FOREST_Stage3,FOREST_Stage4,FOREST_Stage5"},
       {15,"FOREST_Stage4,FOREST_Stage5"},
       {16,"FOOTHILLS_Stage1,FOOTHILLS_Stage2"},
       {17,"FOOTHILLS_Stage1,FOOTHILLS_Stage2,FOOTHILLS_Stage3"},
       {18,"FOOTHILLS_Stage2,FOOTHILLS_Stage3,FOOTHILLS_Stage4"},
       {19,"FOOTHILLS_Stage3,FOOTHILLS_Stage4,FOOTHILLS_Stage5"},
       {20,"FOOTHILLS_Stage4,FOOTHILLS_Stage5"},
       {21,"MOUNTAIN_Stage1,MOUNTAIN_Stage2"},
       {22,"MOUNTAIN_Stage1,MOUNTAIN_Stage2,MOUNTAIN_Stage3"},
       {23,"MOUNTAIN_Stage2,MOUNTAIN_Stage3,MOUNTAIN_Stage4"},
       {24,"MOUNTAIN_Stage3,MOUNTAIN_Stage4,MOUNTAIN_Stage5"},
       {25,"MOUNTAIN_Stage4,MOUNTAIN_Stage5"}
    };

    public string GetScenesForCurrentCheckpoint()
    {
        return CHECKPOINT_SCENE_MAP[Mathf.Min(_currentCheckpoint, CHECKPOINT_SCENE_MAP.Count-1)];
    }
}
