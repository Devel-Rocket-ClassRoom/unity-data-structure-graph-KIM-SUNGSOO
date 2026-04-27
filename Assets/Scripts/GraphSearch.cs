using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor.Search;
using UnityEngine;

public class GraphSearch
{
    private Graph graph;
    public List<GraphNode> path = new List<GraphNode>();
    public void Init(Graph graph)
    {
        this.graph = graph;
    }

    public void DFS(GraphNode node)
    {
        path.Clear();

        var visited = new HashSet<GraphNode>();
        var stack = new Stack<GraphNode>();

        stack.Push(node);
        visited.Add(node); //방문지 등록

        while (stack.Count > 0)
        {
            var currentNode = stack.Pop();
            path.Add(currentNode);

            foreach (var adjacent in currentNode.adjacents)
            {
                if (!adjacent.CanVisit || visited.Contains(adjacent))
                    continue;
                visited.Add(adjacent);
                stack.Push(adjacent);
            }

        }
    }
    public void BFS(GraphNode node)
    {
        path.Clear();

        var visited = new HashSet<GraphNode>();
        var queue = new Queue<GraphNode>();

        queue.Enqueue(node);
        visited.Add(node);

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            path.Add(currentNode);
            foreach (var adjacent in currentNode.adjacents)
            {
                if (!adjacent.CanVisit || visited.Contains(adjacent))
                    continue;
                visited.Add(adjacent);
                queue.Enqueue(adjacent);
            }
        }

    }
    public void DFSRecursive(GraphNode node)
    {
        path.Clear();
        var visited = new HashSet<GraphNode>();
        DFSRecursive(node, visited);

    }
    public void DFSRecursive(GraphNode node, HashSet<GraphNode> visited)
    {
        path.Add(node);
        visited.Add(node);
        foreach (var adjacent in node.adjacents)
        {
            if(!adjacent.CanVisit || visited.Contains(adjacent))
            {
                continue;
            }
            DFSRecursive(adjacent,visited);
        }
    }
    
    

    public bool PathFindingBFS(GraphNode start, GraphNode final)
    {
        path.Clear();
        graph.ResetNodePrevious();
        var visited = new HashSet<GraphNode>();
        var queue = new Queue<GraphNode>();

        queue.Enqueue(start);
        visited.Add(start);

        bool success = false;
        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();

            if(currentNode == final)
            {
                success = true;
                break;
            }
            foreach (var adjacent in currentNode.adjacents)
            {
                if (!adjacent.CanVisit || visited.Contains(adjacent))
                    continue;
                visited.Add(adjacent);
                adjacent.previous = currentNode;
                queue.Enqueue(adjacent);
            }
        }
        if(!success)
        {
            return false;
        }
        GraphNode step = final;
        
        while (step != null)
        {
            path.Add(step);
            step = step.previous;
        }
        path.Reverse();
        return true;
    }
    public void Dijkstra(GraphNode start, GraphNode final)
    {
        path.Clear();
        graph.ResetNodePrevious();
        var distance = new Dictionary<GraphNode, int>();
        var parent = new Dictionary<GraphNode, GraphNode>();
        var visited = new HashSet<GraphNode>();

        var pqueue = new PriorityQueue<GraphNode, int>();

        distance[start] = 0;
        pqueue.Enqueue(start, 0);

        while (pqueue.Count > 0)
        {
            var current = pqueue.Dequeue();

            if (visited.Contains(current))
                continue;

            visited.Add(current);

            if (current == final)
                break;

            foreach (var next in current.adjacents)
            {
                if (!next.CanVisit)
                    continue;

                int newCost = distance[current] + next.weight;

                if (!distance.ContainsKey(next) || newCost < distance[next])
                {
                    distance[next] = newCost;
                    parent[next] = current;

                    pqueue.Enqueue(next, newCost); 
                }
            }
        }

        var cur = final;

        if (!parent.ContainsKey(final) && start != final)
            return;

        while (cur != start)
        {
            path.Add(cur);
            cur = parent[cur];
        }

        path.Add(start);
        path.Reverse();
    }
    private int Heuristic(GraphNode a, GraphNode b)
    {
        int ax = a.id % graph.col;
        int ay = a.id / graph.col;

        int bx = b.id % graph.col;
        int by = b.id / graph.col;

        return Mathf.Abs(ax - bx) + Mathf.Abs(ay - by);
    }
    public void Astar(GraphNode start, GraphNode final)
    {
        path.Clear();
        graph.ResetNodePrevious();
        var distance = new Dictionary<GraphNode, int>();
        var parent = new Dictionary<GraphNode, GraphNode>();
        var visited = new HashSet<GraphNode>();

        var pqueue = new PriorityQueue<GraphNode, int>();

        distance[start] = 0;
        pqueue.Enqueue(start, distance[start] +Heuristic(start,final));

        while (pqueue.Count > 0)
        {
            var current = pqueue.Dequeue();

            if (visited.Contains(current))
                continue;

            visited.Add(current);

            if (current == final)
                break;

            foreach (var next in current.adjacents)
            {
                if (!next.CanVisit)
                    continue;

                int newCost = distance[current] + next.weight;

                if (!distance.ContainsKey(next) || newCost < distance[next])
                {
                    distance[next] = newCost;
                    parent[next] = current;

                    pqueue.Enqueue(next, newCost); 
                }
            }
        }

        var cur = final;

        if (!parent.ContainsKey(final) && start != final)
            return;

        while (cur != start)
        {
            path.Add(cur);
            cur = parent[cur];
        }

        path.Add(start);
        path.Reverse();
    }
}
