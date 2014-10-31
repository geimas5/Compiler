namespace Compiler.RegisterAllocation
{
    using System;
    using System.Collections.Generic;

    using Compiler.Assembly;
    using Compiler.ControlFlowGraph;
    using Compiler.DataFlowAnalysis;

    using Type = Compiler.Type;

    public class RegisterAllocator
    {
        public void AllocateRegisters(ControlFlowGraph graph)
        {
            foreach (var function in graph.Functions.Values)
            {
                var livenessAnalysis = new LivenessAnalysis(graph);
                var liveness = livenessAnalysis.RunAnalysis();
                var interferenceGraphs = this.buildInterferenceGraph(liveness, function);
                var genGraph = interferenceGraphs.Item1;
                var xmmGraph = interferenceGraphs.Item2;

                genGraph.Color(new[]
                                   {
                                       Register.R8,
                                       Register.R9,
                                       Register.R12,
                                       Register.R13,
                                       Register.R14,
                                       Register.R15
                                   });
            }
        }

        private Tuple<InterferenceGraph, InterferenceGraph> buildInterferenceGraph(
            IReadOnlyDictionary<BasicBlock, BlockLiveness> liveness,
            IEnumerable<BasicBlock> function)
        {
            var xmmInterferenceGraph = new InterferenceGraph();
            var genInterferenceGraph = new InterferenceGraph();

            foreach (var block in function)
            {
                var blockLiveness = liveness[block];

                foreach (var variableBitset in blockLiveness.LiveVariables.Values)
                {
                    foreach (var variable1 in variableBitset)
                    {
                        foreach (var variable2 in variableBitset)
                        {
                            if (Equals(variable1.Type, Type.DoubleType) && Equals(variable2.Type, Type.DoubleType))
                            {
                                xmmInterferenceGraph.Link(variable1, variable2);
                                continue;
                            }

                            if (!Equals(variable1.Type, Type.DoubleType) && !Equals(variable2.Type, Type.DoubleType))
                            {
                                genInterferenceGraph.Link(variable1, variable2);
                                continue;
                            }

                            if (Equals(variable1.Type, Type.DoubleType))
                            {
                                xmmInterferenceGraph.Add(variable1);
                            }

                            if (Equals(variable2.Type, Type.DoubleType))
                            {
                                xmmInterferenceGraph.Add(variable2);
                            }

                            if (!Equals(variable1.Type, Type.DoubleType))
                            {
                                genInterferenceGraph.Add(variable1);
                            }

                            if (!Equals(variable2.Type, Type.DoubleType))
                            {
                                genInterferenceGraph.Add(variable2);
                            }
                        }
                    }
                }
            }

            return new Tuple<InterferenceGraph, InterferenceGraph>(genInterferenceGraph, xmmInterferenceGraph);
        }
    }
}
