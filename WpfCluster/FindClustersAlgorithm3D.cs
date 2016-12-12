using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WpfCluster
{
    public class FindClustersAlgorithm3D
    {
        private int[] labels3D;

        protected int[, ,] grid3D;   
        protected int allClusters3D;
        protected List<int> foundClusters3D;

        /// <summary>
        /// Constructor. Initialize grid and labels array
        /// </summary>
        /// <param name="size">Dimension size for square cube</param>
        /// <param name="probability">Probability for random fill cells of cube/param>
        public FindClustersAlgorithm3D(int size, double probability)
        {
            /*this.grid3D = new int[size, size, size];

            Random randObj = new Random();

            for (int i = 0; i < this.grid3D.GetLength(0); i++)
                for (int j = 0; j < this.grid3D.GetLength(1); j++)
                    for (int k = 0; k < this.grid3D.GetLength(2); k++)
                        this.grid3D[i, j, k] = (randObj.NextDouble() < probability) ? 1 : 0;
            */

            this.labels3D = new int[size * size * size / 2];

            // for TEST!
            this.grid3D = new int[,,]
            {
                {
                    {1, 1, 0, 1},
                    {0, 1, 0, 0},
                    {1, 0, 1, 1},
                    {1, 1, 0, 1}
                },
                {
                    {1, 0, 1, 0},
                    {1, 0, 0, 1},
                    {0, 1, 0, 0},
                    {1, 0, 1, 1}
                },
                {
                    {1, 1, 1, 0},
                    {0, 0, 1, 1},
                    {0, 0, 0, 1},
                    {1, 0, 1, 0}
                },
                {
                    {0, 1, 0, 1},
                    {1, 0, 0, 0},
                    {0, 1, 0, 1},
                    {0, 1, 0, 0}
                }
            };
        }

        /// <summary>
        /// Add new cluster label (init new set for Union-Find algorithm)
        /// </summary>
        /// <returns>Set representative</returns>
        private int SetNewCluster()
        {
            this.labels3D[0]++;
            this.labels3D[labels3D[0]] = this.labels3D[0];
            return this.labels3D[0];
        }

        /// <summary>
        /// Find the root of tree for element
        /// </summary>
        /// <remarks>
        /// Return the same element if it is the root of tree.
        /// Else find the root recursive if element parent is not a root
        /// </remarks>
        /// <param name="elem">Element value</param>
        /// <returns>Tree root</returns>
        private int FindRoot(int elem)
        {
            if (this.labels3D[elem] == elem) return elem;

            // else find root of element parent until root will be found
            return this.labels3D[elem] = FindRoot(this.labels3D[elem]);
        }

        /// <summary>
        /// Сombining the two sets (two clusters) in one (with common root)
        /// </summary>
        /// <param name="firstElement">First element for union</param>
        /// <param name="secondElement">Second element for union</param>
        /// <returns>Union set representative</returns>
        private int Union(int firstElement, int secondElement)
        {
            int firstRoot = this.FindRoot(firstElement);
            int secondRoot = this.FindRoot(secondElement);

            return this.labels3D[secondRoot] = firstRoot;
        }

        /// <summary>
        /// Main HK-algorithm method. Scan the cube (3-D array) and set marks to each free cell
        /// </summary>
        protected void HoshenKopelmanAlgorithm3D()
        {
            for (int i = 0; i < this.grid3D.GetLength(0); i++)
                for (int j = 0; j < this.grid3D.GetLength(1); j++)
                    for (int k = 0; k < this.grid3D.GetLength(2); k++)
                        if (this.grid3D[i, j, k] != 0)
                        {
                            int deep = (i == 0 ? 0 : this.grid3D[i - 1, j, k]);
                            int down = (j == 0 ? 0 : this.grid3D[i, j - 1, k]);
                            int left = (k == 0 ? 0 : this.grid3D[i, j, k - 1]);

                            if (left == 0 && down == 0 && deep == 0)
                            {
                                this.grid3D[i, j, k] = this.SetNewCluster();
                            }
                            else if (left != 0 && down == 0 && deep == 0 ||
                                     left == 0 && down != 0 && deep == 0 ||
                                     left == 0 && down == 0 && deep != 0)
                            {
                                this.grid3D[i, j, k] = Math.Max(deep, Math.Max(left, down));
                            }
                            else
                            {
                                if (deep != 0 && left != 0 && down == 0)
                                {
                                    this.grid3D[i, j, k] = this.Union(deep, left);
                                }
                                else if (deep != 0 && down != 0 && left == 0)
                                {
                                    this.grid3D[i, j, k] = this.Union(deep, down);
                                }
                                else if (deep == 0 && down != 0 && left != 0)
                                {
                                    this.grid3D[i, j, k] = this.Union(down, left);
                                }
                                else
                                {
                                    this.grid3D[i, j, k] = this.Union(down, left);
                                    this.grid3D[i, j, k] = this.Union(deep, left);
                                }
                            }
                        }
            this.RelabledGrid3D();
            this.FindPercolationClusters3D();
        }

        /// <summary>
        /// Trick method. Relabed grid from 1 to N clusers instead their current numbers
        /// </summary>
        /// <remarks>
        /// After scanning grid we may have numbers like 3, 5, 6.
        /// This method change them to 1, 2, 3.
        /// Doing this operation after scanning get less time than relabled during scanning
        /// </remarks>
        private void RelabledGrid3D()
        {
            int[] newLabels = new int[this.labels3D.Length];

            for (int i = 0; i < this.grid3D.GetLength(0); i++)
                for (int j = this.grid3D.GetLength(1) - 1; j >= 0; j--)
                    for (int k = 0; k < this.grid3D.GetLength(2); k++)
                        if (this.grid3D[i, j, k] != 0)
                        {
                            int x = this.FindRoot(this.grid3D[i, j, k]);
                            if (newLabels[x] == 0)
                            {
                                newLabels[0]++;
                                newLabels[x] = newLabels[0];
                            }
                            this.grid3D[i, j, k] = newLabels[x];
                        }
            this.allClusters3D = newLabels[0];
        }

        /// <summary>
        /// Check there are any percolation clusters in all found clusters
        /// </summary>
        /// <remarks>
        /// Percolation clusters is the cluster that starts in one side of the cube and ends in the opposite side
        /// </remarks>
        public void FindPercolationClusters3D()
        {
            foundClusters3D = new List<int>();

            // TODO: create method
        }
    }
}
