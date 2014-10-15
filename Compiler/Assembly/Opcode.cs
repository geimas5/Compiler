namespace Compiler.ControlFlowGraph
{
    public enum Opcode
    {
        LEA,
        MOV,
        SUB,
        PUSH,
        POP,
        CMP,
        ADD,
        IMUL,
        JMP,
        XOR,

        CMOVE,
        CMOVNE,
        CMOVL,
        CMOVLE,
        CMOVG,
        CMOVGE
    }
}
