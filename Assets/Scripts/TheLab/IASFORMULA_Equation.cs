using UnityEngine;

namespace Physiqia.TheLab
{
    public interface IASFORMULA_Equation
    {
        int id { get; }
        string equationName { get; }
        string latex { get; }
        string description { get; }
        byte category { get; }
        string[] keywords { get; }
    }
}
