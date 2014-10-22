namespace Compiler.DataFlowAnalysis
{
    using System.Collections.Generic;
    using System.Linq;

    using Compiler.ControlFlowGraph;

    public class LivenessAnalysis
    {
        private readonly ControlFlowGraph graph;

        public Dictionary<BasicBlock, BlockLiveness> BlockLiveness { get; private set; }

        public LivenessAnalysis(ControlFlowGraph graph)
        {
            this.BlockLiveness = new Dictionary<BasicBlock, BlockLiveness>();
            this.graph = graph;
            this.VariableRegister = new VariableRegister();
        }

        public VariableRegister VariableRegister { get; private set; }

        public IReadOnlyDictionary<BasicBlock, BlockLiveness> RunAnalysis()
        {
            bool inChanged = true;

            foreach (var block in this.graph.Functions.SelectMany(m => m.Value))
            {
                var blockLiveness = new BlockLiveness(block, this);
                this.BlockLiveness.Add(block, blockLiveness);
            }

            while (inChanged)
            {
                inChanged = false;

                foreach (var block in this.graph.Functions.SelectMany(m => m.Value))
                {
                    var livenessBlock = this.GetBlockLiveness(block);

                    var beforeIn = livenessBlock.In.Clone();
                    var outSet = new VariableBitset(VariableRegister);
                    foreach (var sucessor in block.Successors)
                    {
                        outSet.Or(this.GetBlockLiveness(sucessor).In);
                    }

                    livenessBlock.Out = outSet;

                    var inSet = livenessBlock.Use.Clone().Or(outSet.Clone().And(livenessBlock.Def.Clone().Invert()));
                    inChanged = !inSet.Equals(beforeIn) || inChanged;

                    livenessBlock.In = inSet;
                }
            }

            return this.BlockLiveness;
        }

        private BlockLiveness GetBlockLiveness(BasicBlock block)
        {
            if (!this.BlockLiveness.ContainsKey(block))
            {
                this.BlockLiveness[block] = new BlockLiveness(block, this);
            }

            return this.BlockLiveness[block];
        }
    }
}
