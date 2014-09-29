namespace Compiler.SyntaxTree
{
    using System;

    public class TreePrinter : IVisitor
    {
        private const int indent = 3;

        private int level = -1;

        public void Visit(ArrayCreatorExpression node)
        {
            level++;

            PrintLevel(node.ToString());

            foreach (var expressionNode in node.Sizes)
            {
                expressionNode.Accept(this);
            }

            level--;
        }

        public void Visit(TypeNode node)
        {
            level++;

            PrintLevel(node.ToString());

            level--;
        }

        public void Visit(AssignmentExpression node)
        {
            level++;

            PrintLevel(node.ToString());

            node.LeftSide.Accept(this);
            node.RightSide.Accept(this);

            level--;
        }

        public void Visit(BinaryOperatorExpression node)
        {
            level++;

            PrintLevel(node.ToString());

            if (node.Left != null) node.Left.Accept(this);
            if (node.Right != null) node.Right.Accept(this);

            level--;
        }

        public void Visit(BreakStatement node)
        {
            level++;

            PrintLevel(node.ToString());

            level--;
        }

        public void Visit(ConstantExpression node)
        {
            level++;

            PrintLevel(node.ToString());

            node.Constant.Accept(this);

            level--;
        }

        public void Visit(ExpressionStatement node)
        {
            level++;

            PrintLevel(node.ToString());

            if (node.Expression != null) node.Expression.Accept(this);

            level--;
        }

        public void Visit(ForStatement node)
        {
            level++;

            PrintLevel(node.ToString());

            node.Initialization.Accept(this);
            node.Condition.Accept(this);
            node.Afterthought.Accept(this);
            
            foreach (var statement in node.Body)
            {
                statement.Accept(this);
            }

            level--;
        }

        public void Visit(FunctionCallExpression node)
        {
            level++;

            PrintLevel(node.ToString());

            foreach (var argument in node.Arguments)
            {
                argument.Accept(this);
            }

            level--;
        }

        public void Visit(VoidFunctionDecleration node)
        {
            level++;

            PrintLevel(node.ToString());

            foreach (var parameter in node.Parameters)
            {
                parameter.Accept(this);
            }


            foreach (var statement in node.Statements)
            {
                statement.Accept(this);
            }

            level--;
        }

        public void Visit(IfStatement node)
        {
            level++;

            PrintLevel(node.ToString());

            node.Condition.Accept(this);
            foreach (var statement in node.Body)
            {
                statement.Accept(this);
            }

            foreach (var statement in node.ElseStatements)
            {
                statement.Accept(this);
            }

            level--;
        }

        public void Visit(IndexerExpression node)
        {
            level++;

            PrintLevel(node.ToString());

            node.Name.Accept(this);
            node.Index.Accept(this);

            level--;
        }

        public void Visit(InitializedVariableDecleration node)
        {
            level++;

            PrintLevel(node.ToString());

            node.Variable.Accept(this);
            node.Initialization.Accept(this);

            level--;
        }

        public void Visit(IntegerConstant node)
        {
            level++;

            PrintLevel(node.ToString());

            level--;
        }

        public void Visit(ProgramNode node)
        {
            level++;

            PrintLevel(node.ToString());

            foreach (var function in node.Functions)
            {
                function.Accept(this);
            }

            level--;
        }

        public void Visit(ReturnExpressionStatement node)
        {
            level++;

            PrintLevel(node.ToString());

            node.Expression.Accept(this);

            level--;
        }

        public void Visit(ReturningFunctionDecleration node)
        {
            level++;

            PrintLevel(node.ToString());

            node.Type.Accept(this);

            foreach (var parameter in node.Parameters)
            {
                parameter.Accept(this);
            }


            foreach (var statement in node.Statements)
            {
                statement.Accept(this);
            }


            level--;
        }

        public void Visit(VoidReturnStatement node)
        {
            level++;

            PrintLevel(node.ToString());

            level--;
        }

        public void Visit(StringConstant node)
        {
            level++;

            PrintLevel(node.ToString());

            level--;
        }

        public void Visit(UnaryExpression node)
        {
            level++;

            PrintLevel(node.ToString());

            node.Expression.Accept(this);

            level--;
        }

        public void Visit(UnInitializedVariableDecleration node)
        {
            level++;

            PrintLevel(node.ToString());

            node.Variable.Accept(this);

            level--;
        }

        public void Visit(VariableExpression node)
        {
            level++;

            PrintLevel(node.ToString());

            node.VariableId.Accept(this);

            level--;
        }

        public void Visit(VariableIdNode node)
        {
            level++;

            PrintLevel(node.ToString());

            level--;
        }

        public void Visit(VariableNode node)
        {
            level++;

            PrintLevel(node.ToString());

            node.Type.Accept(this);
            node.Name.Accept(this);

            level--;
        }

        public void Visit(WhileStatement node)
        {
            level++;

            PrintLevel(node.ToString());

            node.Condition.Accept(this);
            foreach (var statement in node.Body)
            {
                statement.Accept(this);
            }

            level--;
        }

        public void Visit(BooleanConstant node)
        {
            level++;

            PrintLevel(node.ToString());

            level--;
        }

        private void PrintLevel(string text)
        {
            Console.WriteLine(new string(' ', this.level * indent) + text);
        }

        public void PrintTree(SyntaxTree tree)
        {
            tree.RootNode.Accept(this);
        }
    }
}
