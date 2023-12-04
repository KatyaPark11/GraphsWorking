using Assets.Scripts.GraphComponents;
using UnityEngine;
using static Assets.Scripts.VarsHolder;

namespace Assets.Scripts.Controllers
{
    public class MeshColliderController : MonoBehaviour
    {
        public static GameObject GameObject;

        private void Start()
        {
            GameObject = gameObject;
        }

        public static void UpdateMeshColliders()
        {
            foreach (Line line in MainGraph.Lines)
            {
                Vector3 worldDifference = line.EndPoint.Position - line.StartPoint.Position;
                Vector3 localEndPosition = GameObject.transform.InverseTransformDirection(worldDifference) * 2;
                Vector3 lineDirection = (line.EndPoint.Position - line.StartPoint.Position).normalized;
                Vector3 offsetVector = new Vector3(-lineDirection.y, lineDirection.x, 0) * 10f;

                MeshCollider meshCollider = line.LineObj.GetComponent<MeshCollider>();
                Mesh mesh = new();
                Vector3[] vertices = new Vector3[]
                {
                    offsetVector,
                    localEndPosition + offsetVector,
                    localEndPosition - offsetVector,
                    -offsetVector
                };

                mesh.vertices = vertices;
                mesh.triangles = new int[] { 0, 1, 2, 2, 3, 0 };
                meshCollider.sharedMesh = mesh;
            }
        }
    }
}
