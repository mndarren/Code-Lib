
/*
Prim's Algorithm	
The following function implements Prim's algorithm, which identifies a minimum spanning tree (MST) 
on a weighted, connected, undirected graph by greedily growing the tree from a start vertex. 
In particular, the function writes out the edges of a minimum spanning tree on the graph 
represented by the adjacency matrix g[n][n] and the length of that tree.
In g[][], edges that are not present are represented by an arbitrarily large value labeled INFINITY. 
The vertices are numbered by the integers from 0 to n-1. The function grows the MST from vertex 0, 
so initially the set of vertices in the MST is U = {v0}. When a vertex is connected to the tree (i.e., in the set U), 
its entry in the array connected[] is set to 1 (TRUE).
*/
void prim ( int n, matrix g )
{
  int lowcost[n];
  int closest[n];
  int connected[n];
  int k, min, cost = 0;
  int i, j;

  // Initializations:
  connected[0] = 1;         // v0 is in the spanning tree.
  for (int i=1; i<n; ++i)
  {
    connected[i] = 0;     // vi is not in the spanning tree.
    closest[i] = 0;       // v0 in the spanning tree is nearest vi.
    lowcost[i] = g[0][i]; // The length of (v0,vi) is g[0][i].
  }

  // Identify the MST.
  cout << "Minimum cost spanning tree:" << endl;

  for (i=1; i<n; ++i)
  {
    // Find the k not in the tree closest to a vertex in the tree.
    min = INFINITY;
    for (j=1; j<n; ++j)
      if ( !connected[j] && lowcost[j] < min)
      {
        min = lowcost[j];
        k = j;
      }

    // Report the new edge, add its length to the total, and include k.
    cout << "  edge = (v" << k << ",v" << closest[k] << "); cost = "
         << lowcost[k] << endl;
    cost = cost + lowcost[k];
    connected[k] = 1; 

    // Update lowcost[] and closest[].
    for (j=1; j<n; ++j)
      if ( !connected[j] && g[k][j] < lowcost[j] )
      {
        lowcost[j] = g[k][j];
        closest[j] = k;
      }
  }
  cout << "Total cost = " << cost << endl;
}