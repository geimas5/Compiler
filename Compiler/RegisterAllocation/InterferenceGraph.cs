namespace Compiler.RegisterAllocation
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Compiler.Assembly;
    using Compiler.SymbolTable;

    public class InterferenceGraph
    {
        private readonly Dictionary<VariableSymbol, InterferenceNode> nodes = new Dictionary<VariableSymbol, InterferenceNode>();

        public void Link(VariableSymbol variable1, VariableSymbol variable2)
        {
            var node1 = this.GetNode(variable1);
            var node2 = this.GetNode(variable2);

            // Need to do this check after getting the node to 
            // ensure that the nodes are added to the graph.
            if (variable1 == variable2) return;

            if (!node1.Neighbours.Contains(node2)) node1.Neighbours.Add(node2);
            if (!node2.Neighbours.Contains(node1)) node2.Neighbours.Add(node1);
        }

        public void Add(VariableSymbol variable)
        {
            GetNode(variable);
        }

        private InterferenceNode GetNode(VariableSymbol variable)
        {
            if (!nodes.ContainsKey(variable))
            {
                nodes.Add(variable, new InterferenceNode(variable));    
            }

            return nodes[variable];
        }

        public void Color(Register[] colors)
        {
            var simplefiedNodes = new Stack<InterferenceNode>();

            bool successfull = false;
            while (!successfull)
            {
                successfull = this.SimplyfyGraph(colors, simplefiedNodes);
                if (!successfull)
                {
                    ResetGraph(simplefiedNodes);

                    this.RemoveLargestNode();
                }
            }

            foreach (var interferenceNode in simplefiedNodes.Reverse())
            {
                interferenceNode.Variable.Register = this.FindAvailableColor(interferenceNode, colors);
                interferenceNode.IsRemoved = false;
            }
        }

        private void RemoveLargestNode()
        {
            VariableSymbol largestNode = null;
            int currentLargestDegree = 0;

            foreach (var interferenceNode in this.nodes.Where(m => !m.Value.IsPrecolored))
            {
                // To avoid calculating it multiple times.
                var currentNodeDegree = interferenceNode.Value.Degree;

                if (largestNode == null)
                {
                    largestNode = interferenceNode.Key;
                    currentLargestDegree = currentNodeDegree;
                }
                else if (currentLargestDegree < currentNodeDegree)
                {
                    largestNode = interferenceNode.Key;
                    currentLargestDegree = currentNodeDegree;
                }
            }

            Trace.Assert(largestNode != null);
            this.RemoveNode(largestNode);
        }

        private static void ResetGraph(Stack<InterferenceNode> simplefiedNodes)
        {
            foreach (var interferenceNode in simplefiedNodes)
            {
                interferenceNode.IsRemoved = false;
            }

            simplefiedNodes.Clear();
        }

        private bool SimplyfyGraph(Register[] colors, Stack<InterferenceNode> simplefiedNodes)
        {
            // Not precolored nodes.
            while (this.nodes.Any(m => !m.Value.IsRemoved && !m.Value.IsPrecolored))
            {
                var nodeToSimplify = this.nodes.Values.FirstOrDefault(m => !m.IsRemoved && !m.IsPrecolored && m.Degree < colors.Length);

                if (nodeToSimplify != null)
                {
                    simplefiedNodes.Push(nodeToSimplify);
                    nodeToSimplify.IsRemoved = true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private void RemoveNode(VariableSymbol variableSymbol)
        {
            var node = nodes[variableSymbol];
            foreach (var interferenceNode in node.Neighbours)
            {
                interferenceNode.Neighbours.Remove(node);
            }

            nodes.Remove(variableSymbol);
        }

        private Register FindAvailableColor(InterferenceNode node, Register[] registers)
        {
            var possibleColors = new HashSet<Register>(registers);
            foreach (var neighbour in node.Neighbours)
            {
                if (neighbour.Variable.Register.HasValue)
                {
                    possibleColors.Remove(neighbour.Variable.Register.Value);
                }
            }

            return possibleColors.First();
        }
    }
}
