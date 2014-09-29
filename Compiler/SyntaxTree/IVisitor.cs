namespace Compiler.SyntaxTree
{
    public interface IVisitor<T>
    {
        T Visit(ArrayCreatorExpression node);
        T Visit(TypeNode node);
        T Visit(AssignmentExpression node);
        T Visit(BinaryOperatorExpression node);
        T Visit(BreakStatement node);
        T Visit(ConstantExpression node);
        T Visit(ExpressionStatement node);
        T Visit(ForStatement node);
        T Visit(FunctionCallExpression node);
        T Visit(VoidFunctionDecleration node);
        T Visit(IfStatement node);
        T Visit(IndexerExpression node);
        T Visit(InitializedVariableDecleration node);
        T Visit(IntegerConstant node);
        T Visit(ProgramNode node);
        T Visit(ReturnExpressionStatement node);
        T Visit(ReturningFunctionDecleration node);
        T Visit(VoidReturnStatement node);
        T Visit(StringConstant node);
        T Visit(UnaryExpression node);
        T Visit(UnInitializedVariableDecleration node);
        T Visit(VariableExpression node);
        T Visit(VariableIdNode node);
        T Visit(VariableNode node);
        T Visit(WhileStatement node);
        T Visit(BooleanConstant node);
    }

    public interface IVisitor
    {
        void Visit(ArrayCreatorExpression node);
        void Visit(TypeNode node);
        void Visit(AssignmentExpression node);
        void Visit(BinaryOperatorExpression node);
        void Visit(BreakStatement node);
        void Visit(ConstantExpression node);
        void Visit(ExpressionStatement node);
        void Visit(ForStatement node);
        void Visit(FunctionCallExpression node);
        void Visit(VoidFunctionDecleration node);
        void Visit(IfStatement node);
        void Visit(IndexerExpression node);
        void Visit(InitializedVariableDecleration node);
        void Visit(IntegerConstant node);
        void Visit(ProgramNode node);
        void Visit(ReturnExpressionStatement node);
        void Visit(ReturningFunctionDecleration node);
        void Visit(VoidReturnStatement node);
        void Visit(StringConstant node);
        void Visit(UnaryExpression node);
        void Visit(UnInitializedVariableDecleration node);
        void Visit(VariableExpression node);
        void Visit(VariableIdNode node);
        void Visit(VariableNode node);
        void Visit(WhileStatement node);
        void Visit(BooleanConstant node);
    }
}
