using System.Collections.Generic;
using UnityEngine;

public static class YieldCache
{
    class FloatComparer : IEqualityComparer<float>
    {
        bool IEqualityComparer<float>.Equals(float x, float y)
        {
            return x == y;
        }
        int IEqualityComparer<float>.GetHashCode(float obj)
        {
            return obj.GetHashCode();
        }
    }

    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();

    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

    private static readonly Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>(new FloatComparer());

    private static readonly Dictionary<float, WaitForSecondsRealtime> _timeIntervalReal = new Dictionary<float, WaitForSecondsRealtime>(new FloatComparer());

    private static List<float> _wfsPruningList = new List<float>();
    private static Dictionary<int, int> _wfsPage = new Dictionary<int, int>();

    private const int MAX_USE_COUNT = 100;

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds wfs;
        int secondsToInt = (int)seconds;
        // 캐싱 되어 있지 않은 경우
        if (_timeInterval.TryGetValue(seconds, out wfs) == false)
        {
            _timeInterval.Add(seconds, wfs = new UnityEngine.WaitForSeconds(seconds));
        }

        if (_wfsPage.ContainsKey(secondsToInt) == true)
        {
            _wfsPage[secondsToInt]++;
        }
        else
        {
            _wfsPage.Add(secondsToInt, 1);
        }

        CheckWfsPageCapacity();
        return wfs;
    }

    private static void CheckWfsPageCapacity()
    {
        foreach (var index in _wfsPage.Keys)
        {
            PruningWfs(index);
        }

        PrunedListClear();
    }

    private static void PruningWfs(int index)
    {
        if (_wfsPage[index] < MAX_USE_COUNT) return;

        foreach (var key in _timeInterval.Keys)
        {
            if ((int)key == index)
            {
                _wfsPruningList.Add(key);
            }
        }
    }

    private static void PrunedListClear()
    {
        if (_wfsPruningList.Count != 0)
        {
            foreach (var i in _wfsPruningList)
            {
                Debug.Log($"Removed : {i}");
                _timeInterval[i] = null;
                _timeInterval.Remove(i);
                _wfsPage.Remove((int)i);
            }
            _wfsPruningList.Clear();
        }
    }

    public static WaitForSecondsRealtime WaitForSecondsRealtime(float seconds)
    {
        WaitForSecondsRealtime wfsReal;
        if (!_timeIntervalReal.TryGetValue(seconds, out wfsReal))
        {
            _timeIntervalReal.Add(seconds, wfsReal = new UnityEngine.WaitForSecondsRealtime(seconds));
        }
        return wfsReal;
    }
}
