using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : SingletonBehaviour<CheckpointManager>
{

    [SerializeField] private int _forcedCheckpoint = -1;

    private string CHECKPOINT_KEY = "LSCHECKPOINT";
    private int _currentCheckpoint = 0;

    private Dictionary<int, CheckpointTrigger> _registeredCheckpoints = new Dictionary<int, CheckpointTrigger>();

    public int CurrentCheckpoint
    {
        get { return _currentCheckpoint; }
    }

    protected override void Initialize()
    {
        #if UNITY_EDITOR
        if (_forcedCheckpoint >= 0)
        {
            _currentCheckpoint = _forcedCheckpoint;
        }
        else
        #endif
        {
            _currentCheckpoint = PlayerPrefs.GetInt(CHECKPOINT_KEY, 0);
        }
    }

    public void SaveCheckpoint(int checkpoint)
    {
        if (checkpoint > _currentCheckpoint)
        {
            _currentCheckpoint = checkpoint;
            PlayerPrefs.SetInt(CHECKPOINT_KEY, checkpoint);
            PlayerPrefs.Save();
        }
    }

    public void RegisterCheckpoint(CheckpointTrigger trigger, int checkpointIndex)
    {
        if (!_registeredCheckpoints.ContainsKey(checkpointIndex))
        {
            _registeredCheckpoints.Add(checkpointIndex, trigger);
        }
    }

    public void UnregisterCheckpoint(int checkpointIndex)
    {
        _registeredCheckpoints.Remove(checkpointIndex);
    }

    public CheckpointTrigger GetCurrentCheckpoint()
    {
        if (_registeredCheckpoints.ContainsKey(_currentCheckpoint))
        {
            return _registeredCheckpoints[_currentCheckpoint];
        }

        return null;
    }

    public bool IsAtVideoCheckpoint()
    {
        return _currentCheckpoint == 0 || _currentCheckpoint == 6 || _currentCheckpoint == 12 || _currentCheckpoint == 18 || _currentCheckpoint == 24 || _currentCheckpoint == 32;
    }

    [ContextMenu("Reset Progress")]
    public void ResetProgress()
    {
        _currentCheckpoint = 0;
        PlayerPrefs.SetInt(CHECKPOINT_KEY, 0);
        PlayerPrefs.Save();
    }


    private readonly Dictionary<int, string> SCENE_FOR_CHECKPOINT = new Dictionary<int, string>()
    {
        {0,"IntroScene"},
        {1,"URBAN_Stage1"},
        {2,"URBAN_Stage2"},
        {3,"URBAN_Stage3"},
        {4,"URBAN_Stage4"},
        {5,"URBAN_Stage5"},
        {6,"IntroScene"},
        {7,"COUNTRY_Stage1"},
        {8,"COUNTRY_Stage2"},
        {9,"COUNTRY_Stage3"},
        {10,"COUNTRY_Stage4"},
        {11,"COUNTRY_Stage5"},
        {12,"IntroScene"},
        {13,"FOREST_Stage1"},
        {14,"FOREST_Stage2"},
        {15,"FOREST_Stage3"},
        {16,"FOREST_Stage4"},
        {17,"FOREST_Stage5"},
        {18,"IntroScene"},
        {19,"FOOTHILLS_Stage1"},
        {20,"FOOTHILLS_Stage2"},
        {21,"FOOTHILLS_Stage3"},
        {22,"FOOTHILLS_Stage4"},
        {23,"FOOTHILLS_Stage5"},
        {24,"IntroScene"},
        {25,"MOUNTAIN_Stage1"},
        {26,"MOUNTAIN_Stage2"},
        {27,"MOUNTAIN_Stage2"},
        {28,"MOUNTAIN_Stage3"},
        {29,"MOUNTAIN_Stage3"},
        {30,"MOUNTAIN_Stage4"},
        {31,"MOUNTAIN_Stage5"},
        {32,"IntroScene"}
    };
    private Dictionary<int, string> CHECKPOINT_SCENE_MAP = new Dictionary<int, string>
    {
       {0,"IntroScene"},
       {1,"URBAN_Stage1,URBAN_Stage2"},
       {2,"URBAN_Stage1,URBAN_Stage2,URBAN_Stage3"},
       {3,"URBAN_Stage2,URBAN_Stage3,URBAN_Stage4"},
       {4,"URBAN_Stage3,URBAN_Stage4,URBAN_Stage5"},
       {5,"URBAN_Stage4,URBAN_Stage5"},
       {6,"IntroScene"},
       {7,"COUNTRY_Stage1,COUNTRY_Stage2"},
       {8,"COUNTRY_Stage1,COUNTRY_Stage2,COUNTRY_Stage3"},
       {9,"COUNTRY_Stage2,COUNTRY_Stage3,COUNTRY_Stage4"},
       {10,"COUNTRY_Stage3,COUNTRY_Stage4,COUNTRY_Stage5"},
       {11,"COUNTRY_Stage4,COUNTRY_Stage5"},
       {12,"IntroScene"},
       {13,"FOREST_Stage1,FOREST_Stage2"},
       {14,"FOREST_Stage1,FOREST_Stage2,FOREST_Stage3"},
       {15,"FOREST_Stage2,FOREST_Stage3,FOREST_Stage4"},
       {16,"FOREST_Stage3,FOREST_Stage4,FOREST_Stage5"},
       {17,"FOREST_Stage4,FOREST_Stage5"},
       {18,"IntroScene"},
       {19,"FOOTHILLS_Stage1,FOOTHILLS_Stage2"},
       {20,"FOOTHILLS_Stage1,FOOTHILLS_Stage2,FOOTHILLS_Stage3"},
       {21,"FOOTHILLS_Stage2,FOOTHILLS_Stage3,FOOTHILLS_Stage4"},
       {22,"FOOTHILLS_Stage3,FOOTHILLS_Stage4,FOOTHILLS_Stage5"},
       {23,"FOOTHILLS_Stage4,FOOTHILLS_Stage5"},
       {24,"IntroScene"},
       {25,"MOUNTAIN_Stage1,MOUNTAIN_Stage2"},
       {26,"MOUNTAIN_Stage1,MOUNTAIN_Stage2,MOUNTAIN_Stage3"},
       {27,"MOUNTAIN_Stage1,MOUNTAIN_Stage2,MOUNTAIN_Stage3"},
       {28,"MOUNTAIN_Stage2,MOUNTAIN_Stage3,MOUNTAIN_Stage4"},
       {29,"MOUNTAIN_Stage3,MOUNTAIN_Stage4"},
       {30,"MOUNTAIN_Stage3,MOUNTAIN_Stage4,MOUNTAIN_Stage5"},
       {31,"MOUNTAIN_Stage4,MOUNTAIN_Stage5"},
       {32,"IntroScene"}
    };

    public string GetScenesForCheckpoint(int checkPoint)
    {
        if (CHECKPOINT_SCENE_MAP.ContainsKey(checkPoint))
        {
            return CHECKPOINT_SCENE_MAP[checkPoint];
        }
        else
        {
            return "";
        }
    }

    public string GetSceneForCheckpoint(int checkPoint)
    {
        if (SCENE_FOR_CHECKPOINT.ContainsKey(checkPoint))
        {
            return SCENE_FOR_CHECKPOINT[checkPoint];
        }
        else
        {
            return "";
        }
    }

    public string GetScenesForCurrentCheckpoint()
    {
        return CHECKPOINT_SCENE_MAP[Mathf.Min(_currentCheckpoint, CHECKPOINT_SCENE_MAP.Count - 1)];
    }
}
