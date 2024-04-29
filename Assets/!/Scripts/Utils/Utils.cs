using System.Collections;
using UnityEngine;
using System;

public static class Utils
{
    public static IEnumerator WaitAndDo(float seconds, Action todo)
    {
        yield return new WaitForSeconds(seconds);
        todo?.Invoke();
    }
}
