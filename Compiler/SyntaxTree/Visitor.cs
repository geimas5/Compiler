namespace Compiler.SyntaxTree
{
    using System.Linq;

    public abstract class Visitor : IVisitor
    {
        public virtual void Visit(ArrayCreatorExpression node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(TypeNode node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(AssignmentExpression node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(BinaryOperatorExpression node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(BreakStatement node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(ConstantExpression node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(ExpressionStatement node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(ForStatement node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(FunctionCallExpression node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(VoidFunctionDecleration node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(IfStatement node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(IndexerExpression node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(InitializedVariableDecleration node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(IntegerConstant node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(ProgramNode node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(ReturnExpressionStatement node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(ReturningFunctionDecleration node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(VoidReturnStatement node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(StringConstant node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(UnaryExpression node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(UnInitializedVariableDecleration node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(VariableExpression node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(VariableIdNode node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(VariableNode node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(WhileStatement node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(BooleanConstant node)
        {
            VisitAllChildren(node);
        }

        public virtual void Visit(DoubleConstant node)
        {
            VisitAllChildren(node);
        }

        protected void VisitAllChildren(Node node)
        {
            foreach (var child in node.Children.Where(m => m != null))
            {
                child.Accept(this);
            }
        }
    }

    public abstract class Visitor<T> : IVisitor<T>
    {
        public virtual T Visit(ArrayCreatorExpression node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(TypeNode node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(AssignmentExpression node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(BinaryOperatorExpression node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(BreakStatement node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(ConstantExpression node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(ExpressionStatement node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(ForStatement node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(FunctionCallExpression node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(VoidFunctionDecleration node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(IfStatement node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(IndexerExpression node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(InitializedVariableDecleration node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(IntegerConstant node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(ProgramNode node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(ReturnExpressionStatement node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(ReturningFunctionDecleration node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(VoidReturnStatement node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(StringConstant node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(UnaryExpression node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual  T Visit(UnInitializedVariableDecleration node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(VariableExpression node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(VariableIdNode node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(VariableNode node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(WhileStatement node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(BooleanConstant node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        public virtual T Visit(DoubleConstant node)
        {
            VisitAllChildren(node);
            return default(T);
        }

        protected void VisitAllChildren(Node node)
        {
            foreach (var child in node.Children)
            {
                child.Accept(this);
            }
        }
    }
}
