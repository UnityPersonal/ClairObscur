using System;

public static class GameUtilHelper
{
    public static int GetEnumCount<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)).Length;
    }
}