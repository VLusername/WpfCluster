using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCluster
{
    /// <summary>
    /// Implementation of Hoshen-Kopelman algorithm
    /// </summary>
    public class FindClustersAlgorithm
    {
        private int[] labels;

        protected int[,] grid;

        protected int allClusters;
        public List<int> percolationClusters;

        /// <summary>
        /// Constructor. Initialize grid and labels array
        /// </summary>
        /// <param name="size">Dimension size for square grid</param>
        /// <param name="probability">Probability for random fill cells of grid/param>
        public FindClustersAlgorithm(int size, double probability)
        {
            this.grid = new int[size, size];
            this.labels = new int[size * size / 2];

            Random randObj = new Random();

            for (int i = 0; i < this.grid.GetLength(0); i++)
                for (int j = 0; j < this.grid.GetLength(1); j++)
                    this.grid[i, j] = (randObj.NextDouble() < probability) ? 1 : 0;
        }

        /// <summary>
        /// Add new cluster label (init new set for Union-Find algorithm)
        /// </summary>
        /// <returns>Set representative</returns>
        private int SetNewCluster()
        {
            this.labels[0]++;
            this.labels[labels[0]] = this.labels[0];
            return this.labels[0];
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
            // if x is root of tree
            if (this.labels[elem] == elem) return elem;

            // else find root of element parent until root will be found
            return this.labels[elem] = FindRoot(this.labels[elem]);
        }

        /// <summary>
        /// Сombining the two sets (two clusters) in one (with common root)
        /// </summary>
        /// <remarks>
        /// This function using random choosing root according to the second heuristics of Union-Find
        /// </remarks>
        /// <param name="firstElement">First element for union</param>
        /// <param name="secondElement">Second element for union</param>
        /// <returns>Union set representative</returns>
        private int Union(int firstElement, int secondElement)
        {
            int firstRoot = this.FindRoot(firstElement);
            int secondRoot= this.FindRoot(secondElement);

            // random choosing root of new union tree
            if (new Random().Next() % 2 == 0)
            {
                return this.labels[secondRoot] = firstRoot;
            }

            return this.labels[firstRoot] = secondRoot;         
        }

        /// <summary>
        /// Main HK-algorithm method. Scan the grid and set marks to each free cell
        /// </summary>
        public void HoshenKopelmanAlgorithm()
        {
            for (int i = 0; i < this.grid.GetLength(0); i++)
                for (int j = 0; j < this.grid.GetLength(1); j++)
                    if (this.grid[i, j] != 0)
                    {
                        int up = (i == 0 ? 0 : this.grid[i - 1, j]);
                        int left = (j == 0 ? 0 : this.grid[i, j - 1]);

                        if (left == 0 && up == 0)
                        {
                            this.grid[i, j] = this.SetNewCluster();
                        }
                        else if (left != 0 && up == 0 || left == 0 && up != 0)
                        {
                            this.grid[i, j] = Math.Max(up, left);
                        }
                        else
                        {
                            this.grid[i, j] = this.Union(up, left);
                        }
                    }

            this.RelabledGrid();
            this.FindPercolationClusters();
        }

        /// <summary>
        /// Trick method. Relabed grid from 1 to N clusers instead their current numbers
        /// </summary>
        /// <remarks>
        /// After scanning grid we may have numbers like 3, 5, 6.
        /// This method change them to 1, 2, 3.
        /// Doing this operation after scanning get less time than relabled during scanning
        /// </remarks>
        private void RelabledGrid()
        {
            int[] newLabels = new int[this.labels.Length];

            for (int i = 0; i < this.grid.GetLength(0); i++)
                for (int j = 0; j < this.grid.GetLength(1); j++)
                    if (this.grid[i, j] != 0)
                    {
                        int x = this.FindRoot(this.grid[i, j]);
                        if (newLabels[x] == 0)
                        {
                            newLabels[0]++;
                            newLabels[x] = newLabels[0];
                        }
                        this.grid[i, j] = newLabels[x];
                    }
            this.allClusters = newLabels[0];
        }

        /// <summary>
        /// Check there are any percolation clusters in all found clusters
        /// </summary>
        /// <remarks>
        /// Percolation clusters is the cluster that starts in first row and ends in last
        /// </remarks>
        protected void FindPercolationClusters()
        {
            this.percolationClusters = new List<int>();

            for (int i = 0; i < this.grid.GetLength(0); i++)
                for (int j = 0; j < this.grid.GetLength(1); j++)
                    if (this.grid[0, i] != 0 && !this.percolationClusters.Contains(this.grid[0, i]) && this.grid[0, i] == this.grid[this.grid.GetLength(1) - 1, j])
                    {
                        this.percolationClusters.Add(grid[0, i]);
                        break;
                    }
        }
    }
}
