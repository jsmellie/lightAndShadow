using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoutineRunner
{
    private static RoutineRunnerDummy _runnerDummy;
    private static uint _lastID = 0;
    private static Dictionary<uint, Coroutine> _activeRoutines = new Dictionary<uint, Coroutine>();

    static RoutineRunner()
    {
        GameObject dummyObj = new GameObject("RoutineRunnerDummy");
        _runnerDummy = dummyObj.AddComponent<RoutineRunnerDummy>();
        GameObject.DontDestroyOnLoad(dummyObj);
    }

    public static uint StartRoutine(IEnumerator routineFunction)
    {
        uint id = 0;
        if (routineFunction != null)
        {
            id = _lastID++;

            Coroutine routine = _runnerDummy.StartCoroutine(RoutineWrapper(routineFunction, id));

            _activeRoutines.Add(id, routine);
        }

        return id;
    }

    public static void StopRoutine(uint id)
    {
        if (_activeRoutines != null && _activeRoutines.ContainsKey(id))
        {
            Coroutine routine = _activeRoutines[id];
            if (routine != null)
            {
                _runnerDummy.StopCoroutine(routine);
            }

            CleanupRoutine(id);
        }
    }

    public static void StopAllRoutines()
    {
        if (_activeRoutines != null && _activeRoutines.Count > 0)
        {
            foreach(uint id in _activeRoutines.Keys)
            {
                StopRoutine(id);
            }
        }
    }

    private static IEnumerator RoutineWrapper(IEnumerator routineFunction, uint id)
    {
        yield return _runnerDummy.StartCoroutine(routineFunction);

        CleanupRoutine(id);
    }

    private static void CleanupRoutine(uint id)
    {
        if (_activeRoutines != null && _activeRoutines.ContainsKey(id))
        {
            _activeRoutines[id] = null;
            _activeRoutines.Remove(id);
        }
    }
}
