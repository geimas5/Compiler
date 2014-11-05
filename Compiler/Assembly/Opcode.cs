namespace Compiler.ControlFlowGraph
{
    public enum Opcode
    {
        LEA,
        MOV,
        SUB,
        CMP,
        ADD,
        IMUL,
        XOR,
        AND,

        CMOVE,
        CMOVNE,
        CMOVL,
        CMOVLE,
        CMOVG,
        CMOVGE,

        MOVD,
        MOVSD,
        ADDSD,
        SUBSD,
        MULSD,
        DIVSD,

        CVTSI2SD
    }
}                                                                                                                                                                                                                                                                                                                           