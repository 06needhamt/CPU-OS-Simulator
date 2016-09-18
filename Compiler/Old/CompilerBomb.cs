#pragma warning disable 1591
using System;
using System.Linq;

#pragma warning disable 1591
class X<A, B, C, D> { class Y : X<Y, Y, Y, Y> { Y.Y.Y.Y.Y.Y.Y.Y.Y y; } }
#pragma warning disable 1591
class Program
{
    public static void Main()
    {
        Enumerable.Range(0, 1).Sum(a =>
        Enumerable.Range(0, 1).Sum(b =>
        Enumerable.Range(0, 1).Sum(c =>
        Enumerable.Range(0, 1).Sum(d =>
        Enumerable.Range(0, 1).Count(e => true)))));
    }
}