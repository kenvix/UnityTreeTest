using System.Collections;
using System.Collections.Generic;
using System.Text;
using Mono.Data.Sqlite;
using Mtree;
using UnityEngine;

public class MTreeExporterMonoCore : MonoBehaviour
{ 
    private readonly System.Random random = new();
    public int generateNum = 100;
    public bool isValidationData = false;
    public string generatedDataName = "Tree";

    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
    public void executeExportation()
    {
        Debug.Log($"Exportation started with {generateNum} objects");
        
        MtreeComponent mtreeComponent = gameObject.GetComponent<MtreeComponent>();
        MTree mtree = mtreeComponent.tree;
        MeshFilter meshFilter = mtreeComponent.filter;
        var treeFunctions = mtreeComponent.treeFunctionsAssets;

        for (int i = 0; i < generateNum; i++)
        {
            using (var command = GlobalGameSystem.Instance.CreateCommand())
            {
                treeFunctions.ForEach(fun =>
                {
                    randomizeTreeFunction(fun);
                    switch (fun)
                    {
                        case BranchFunction branches:
                            command.Parameters.Add(new SqliteParameter("@branch_seed", branches.seed));
                            command.Parameters.Add(new SqliteParameter("@branch_length", branches.length));
                            command.Parameters.Add(new SqliteParameter("@branch_number", branches.number));
                            command.Parameters.Add(new SqliteParameter("@branch_resolution", branches.resolution));
                            command.Parameters.Add(new SqliteParameter("@branch_split_proba", branches.splitProba));
                            command.Parameters.Add(new SqliteParameter("@branch_randomness", branches.randomness));
                            command.Parameters.Add(new SqliteParameter("@branch_angle", branches.angle));
                            command.Parameters.Add(new SqliteParameter("@branch_up_attraction", branches.upAttraction));
                            command.Parameters.Add(new SqliteParameter("@branch_start", branches.start));
                            break;

                        case LeafFunction leaves:
                            // todo: leaves support
                            break;

                        case TrunkFunction trunk:
                            command.Parameters.Add(new SqliteParameter("@trunk_seed", trunk.seed));
                            command.Parameters.Add(new SqliteParameter("@trunk_length", trunk.length));
                            command.Parameters.Add(new SqliteParameter("@trunk_radius", trunk.radiusMultiplier));
                            command.Parameters.Add(new SqliteParameter("@trunk_resolution", trunk.resolution));
                            command.Parameters.Add(new SqliteParameter("@trunk_axis", trunk.originAttraction));
                            command.Parameters.Add(new SqliteParameter("@trunk_randomness", trunk.randomness));
                            break;
                    }
                });

                mtreeComponent.GenerateTree();

                StringBuilder trianglesStr = new StringBuilder();
                StringBuilder verticesStr = new StringBuilder();

                var mesh = meshFilter.sharedMesh;
                foreach (var vertex in mesh.vertices)
                {
                    verticesStr.Append($"{vertex.x} {vertex.y} {vertex.z}\n");
                }

                int triangleCounter = 0;
                foreach (var triangle in mesh.triangles)
                {
                    switch (triangleCounter)
                    {
                        case 0:
                            trianglesStr.Append($"3 {triangle} ");
                            triangleCounter++;
                            break;

                        case 1:
                            trianglesStr.Append($"{triangle} ");
                            triangleCounter++;
                            break;

                        case 2:
                            trianglesStr.Append($"{triangle}\n");
                            triangleCounter = 0;
                            break;
                    }
                }

                trianglesStr.Remove(trianglesStr.Length - 1, 1);
                verticesStr.Remove(verticesStr.Length - 1, 1);

                command.Parameters.Add(new SqliteParameter("@triangles", trianglesStr.ToString()));
                command.Parameters.Add(new SqliteParameter("@vertices", verticesStr.ToString()));
                GlobalGameSystem.Instance.AddTreeItem(command, generatedDataName, isValidationData ? 1 : 0);
            }
        }

        //MeshFilter meshFilter = mtree.
        //var mesh = meshFilter.mesh;
        //Vector3[] vertices = mesh.vertices;
        //mesh.vertices = vertices;

    }

    private void randomizeTreeFunction(TreeFunctionAsset fun)
    {
        fun.seed = random.Next();
        switch (fun)
        {
            case BranchFunction branches:
                //branches.length = ;
                //branches.number = ;
                //branches.resolution));
                //branches.splitProba));
                //branches.randomness));
                //branches.angle));
                //branches.upAttraction));
                //branches.start));
                break;

            case LeafFunction leaves:
                // todo: leaves support
                break;

            case TrunkFunction trunk:
                trunk.length = random.Next(1_00, 65_00) / 100f;
                //trunk.radiusMultiplier));
                //trunk.resolution));
                //trunk.originAttraction));
                //trunk.randomness));
                break;
        }
    }
}
