using System;
using System.Collections.Generic;

/// <summary>
/// Classe que contém os métodos de extensão relacionados a listas
/// </summary>
public static class ListExtensions
{
    /*
     * Fisher Yates Shuffle
     * https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
     */
    public static IList<T> Shuffle<T>(this IList<T> list)
    {
        var rng = new Random();

        for (int i = list.Count - 1; i > 0; i--)
        {
            int nextElement = rng.Next(i);

            T temp = list[i];
            list[i] = list[nextElement];
            list[nextElement] = temp;
        }

        return list;
    }
}
