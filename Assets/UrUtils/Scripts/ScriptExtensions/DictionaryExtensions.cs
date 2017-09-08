//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System.Collections.Generic;

public static class DictionaryExtensions
{
    public static TKey[] KeysToArray<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
    {
        var keys = dictionary.Keys;
        var array = new TKey[keys.Count];
        keys.CopyTo(array, 0);
        return array;
    }
    
    public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
    {
        TValue value;
        if (dictionary.TryGetValue(key, out value))
        {
            return value;
        }
        return defaultValue;
    }
}
