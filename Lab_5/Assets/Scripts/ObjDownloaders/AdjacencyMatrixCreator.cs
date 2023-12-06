using TMPro;
using UnityEngine;
using static Assets.Scripts.VarsHolder;

namespace Assets.Scripts.ObjDownloaders
{
    public class AdjacencyMatrixCreator : MonoBehaviour
    {
        public TextMeshProUGUI AbjacencyMatrix;

        public void CreateAdjacencyMatrix()
        {
            int[,] adjacencyMatrix = MainGraph.GetAdjacencyMatrix();
            int rows = adjacencyMatrix.GetLength(0);
            int cols = adjacencyMatrix.GetLength(1);

            string matrixString = "    ";
            for (int i = 0; i < MainGraph.Points.Count; i++)
            {
                matrixString += MainGraph.Points[i].Name.ToString().PadRight(4);
            }
            matrixString += "\n";

            for (int row = 0; row < rows; row++)
            {
                matrixString += MainGraph.Points[row].Name.ToString().PadRight(3);
                for (int col = 0; col < cols; col++)
                {
                    matrixString += adjacencyMatrix[row, col].ToString().PadLeft(2, '0').PadRight(3);
                }
                matrixString += "\n";
            }

            AbjacencyMatrix.text = matrixString;
        }
    }
}
