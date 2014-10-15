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
        IDIV,
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
