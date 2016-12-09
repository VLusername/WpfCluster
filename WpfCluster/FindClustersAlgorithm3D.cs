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
        public int[, ,] threeDgrid;
        public int[] labels;
        public int totalClusters = 0;

        public FindClustersAlgorithm3D(int size, double probability)
        {
            /*this.threeDgrid = new int[size, size, size];

            Random randObj = new Random();

            for (int i = 0; i < this.threeDgrid.GetLength(0); i++)
                for (int j = 0; j < this.threeDgrid.GetLength(1); j++)
                    for (int k = 0; k < this.threeDgrid.GetLength(2); k++)
                        this.threeDgrid[i, j, k] = (randObj.NextDouble() < probability) ? 1 : 0;
            */

            this.labels = new int[size * size * size / 2];

            this.threeDgrid = new int[,,]
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

        public int SetNewCluster()
        {
            this.labels[0]++;
            this.labels[labels[0]] = this.labels[0];
            return this.labels[0];
        }

        public int FindRoot(int gridElement)
        {
            //if x is root of tree
            if (this.labels[gridElement] == gridElement) return gridElement;

            //else find root of x parent until x root will be found
            return this.labels[gridElement] = FindRoot(this.labels[gridElement]);
        }

        public int Union(int firstElement, int secondElement)
        {
            int firstRoot = this.FindRoot(firstElement);
            int secondRoot = this.FindRoot(secondElement);

            return this.labels[secondRoot] = firstRoot;
        }

        /**
         * HK algorithm for 3-dimension grid
         */
        public void HoshenKopelmanAlgorithm3D()
        {
            for (int i = 0; i < this.threeDgrid.GetLength(0); i++)
                for (int j = this.threeDgrid.GetLength(1) - 1; j >= 0; j--)
                    for (int k = 0; k < this.threeDgrid.GetLength(2); k++)
                        if (this.threeDgrid[i, j, k] != 0)
                        {
                            int deep = (i == 0 ? 0 : this.threeDgrid[i - 1, j, k]);
                            int down = (j == (this.threeDgrid.GetLength(1) - 1) ? 0 : this.threeDgrid[i, j + 1, k]);
                            int left = (k == 0 ? 0 : this.threeDgrid[i, j, k - 1]);

                            if (left == 0 && down == 0 && deep == 0)
                            {
                                this.threeDgrid[i, j, k] = this.SetNewCluster();
                            }
                            else if (left != 0 && down == 0 && deep == 0 ||
                                     left == 0 && down != 0 && deep == 0 ||
                                     left == 0 && down == 0 && deep != 0)
                            {
                                this.threeDgrid[i, j, k] = Math.Max(deep, Math.Max(left, down));
                            }
                            else
                            {
                                if (deep != 0 && left != 0 && down == 0)
                                {
                                    this.threeDgrid[i, j, k] = this.Union(deep, left);
                                }
                                else if (deep != 0 && down != 0 && left == 0)
                                {
                                    this.threeDgrid[i, j, k] = this.Union(deep, down);
                                }
                                else if (deep == 0 && down != 0 && left != 0)
                                {
                                    this.threeDgrid[i, j, k] = this.Union(down, left);
                                }
                                else
                                {
                                    this.threeDgrid[i, j, k] = this.Union(down, left);
                                    this.threeDgrid[i, j, k] = this.Union(deep, left);
                                }
                            }
                        }
        }

        public int Relabled3DGrid()
        {
            int[] newLabels = new int[this.labels.Length];

            for (int i = 0; i < this.threeDgrid.GetLength(0); i++)
                for (int j = this.threeDgrid.GetLength(1) - 1; j >= 0; j--)
                    for (int k = 0; k < this.threeDgrid.GetLength(2); k++)
                        if (this.threeDgrid[i, j, k] != 0)
                        {
                            int x = this.FindRoot(this.threeDgrid[i, j, k]);
                            if (newLabels[x] == 0)
                            {
                                newLabels[0]++;
                                newLabels[x] = newLabels[0];
                            }
                            this.threeDgrid[i, j, k] = newLabels[x];
                        }

            return newLabels[0];
        }

        /*public List<int> FindPercolationClusters()
        {
            List<int> foundClusters = new List<int>();

            for (int i = 0; i < this.grid.GetLength(0); i++)
                for (int j = 0; j < this.grid.GetLength(1); j++)
                    if (this.grid[0, i] != 0 && !foundClusters.Contains(this.grid[0, i]) && this.grid[0, i] == this.grid[this.grid.GetLength(1) - 1, j])
                    {
                        foundClusters.Add(grid[0, i]);
                        break;
                    }
            return foundClusters;
        }*/
    }
}
