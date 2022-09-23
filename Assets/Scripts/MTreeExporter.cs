using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtree;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class MTreeExporter
{
    public static readonly MTreeExporter Instance = new();

    public class MTreeExported
    {
        public Mesh Mesh;
        public MTree Component;

        public MTreeExported(Mesh mesh, MTree component)
        {
            Mesh = mesh;
            Component = component;
        }
    }

    public void AddTree(String name, Mesh mesh, MTree component)
    {
        if (!MTrees.ContainsKey(name))
        {
            MTrees.Add(name, new MTreeExported(mesh, component));
        }
    }

    public readonly Dictionary<String, MTreeExported> MTrees = new();

    MTreeExporter()
    {
        Debug.Log("MTreeExporter: initialized");
    }
}