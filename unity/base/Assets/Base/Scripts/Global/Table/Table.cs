using System;
using System.Collections.Generic;

public abstract class Table
{

}

public abstract class Table<T1, T2> : Table where T1 : IKey<T2>
{
    public abstract IReadOnlyList<T1> Datas { get; }

    public bool TryGetData<T>(T key, out T1 data)
    {
        data = default;
        if (Datas.IsNull())
            return false;

        var length = Datas.Count;
        for (var i = 0; i < length; ++i)
        {
            data = Datas[i];
            if (data.Key.Equals(key))
                return true;
        }

        return false;
    }
}
