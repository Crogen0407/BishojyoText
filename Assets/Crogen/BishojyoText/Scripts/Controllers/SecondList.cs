using System;
using System.Collections.Generic;

[Serializable]
public struct SecondList<T>
{
    public List<T> secondList;
}

[Serializable]
public struct SecondArray<T>
{
    public List<T> secondArray;
}