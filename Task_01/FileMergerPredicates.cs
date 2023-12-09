﻿namespace Task_01;

public static class FileMergerPredicates
{
    public static Func<string, bool> NotContainsValue(string value)
    {
        return line => line.Contains(value) == false;
    }
}