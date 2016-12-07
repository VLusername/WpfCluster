using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCluster
{
    public class FindClustersAlgorithm
    {
        public int[,] grid;
        public int[] labels;
        public int totalClusters = 0;

        public FindClustersAlgorithm(int size, double probability)
        {
            this.grid = new int[size, size];
            this.labels = new int[size * size / 2];

            Random randObj = new Random();

            for (int i = 0; i < this.grid.GetLength(0); i++)
                for (int j = 0; j < this.grid.GetLength(1); j++)
                    this.grid[i, j] = (randObj.NextDouble() < probability) ? 1 : 0;
        }

        public int SetNewCluster()
        {
            this.labels[0]++;

            //TODO: if (this.labels[0] < this.labels.Length)

            this.labels[labels[0]] = this.labels[0];
            return this.labels[0];
        }

        public int FindRoot(int x)
        {
            // if x is root of tree
            if (this.labels[x] == x) return x;

            // else find root of x parent until x root will be found
            return this.labels[x] = FindRoot(this.labels[x]);
        }

        public int Union(int x, int y)
        {
            int xRoot = this.FindRoot(x);
            int yRoot= this.FindRoot(y);

            // random choosing root of new union tree
            if (new Random().Next() % 2 == 0)
            {
                return this.labels[yRoot] = xRoot;
            }

            return this.labels[xRoot] = yRoot;         
        }

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
        }

        public int RelabledGrid()
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

            return newLabels[0];
        }

        public List<int> FindPercolationClusters()
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
        }
    }
}
