namespace Compiler.SemanticAnalysis
{
    using System;
    using System.Linq;

    using Compiler.SymbolTable;
    using Compiler.SyntaxTree;

    using Type = Compiler.Type;

    public class SymbolTableBuilder : Visitor
    {
        private SymbolTable currentSymbolTable;

        private readonly Logger logger;

        public SymbolTableBuilder(Logger logger)
        {
            this.logger = logger;
        }

        public SymbolTable BuildSymbolTable(ProgramNode programNode)
        {
            this.Visit(programNode);

            return this.currentSymbolTable;
        }

        public override void Visit(ProgramNode node)
        {
            this.currentSymbolTable = new SymbolTable();
            var symbolTable = this.currentSymbolTable;

            RegisterFunctions(node, symbolTable);
            base.Visit(node);

            this.currentSymbolTable = symbolTable;
        }

        public override void Visit(VoidFunctionDecleration node)
        {
            this.Visit(node);
        }

        public override void Visit(ReturningFunctionDecleration node)
        {
            this.Visit(node);
        }

        private void Visit(FunctionDecleration node)
        {
            var parent = this.currentSymbolTable;
            this.currentSymbolTable = ((FunctionSymbol)node.Symbol).SymbolTable;

            foreach (var statement in node.Statements)
            {
                statement.Accept(this);
            }

            this.currentSymbolTable = parent;
        }

        private void RegisterFunctions(ProgramNode node, SymbolTable parent)
        {
            foreach (var function in node.Functions)
            {
                FunctionSymbol functionSymbol;

                var parameters = function.Parameters.Select(m => m.Name.Name).ToArray();

                if (function is ReturningFunctionDecleration)
                {
                    functionSymbol = new ReturningFunctionSymbol(
                        function.Name,
                        ((ReturningFunctionDecleration)function).Type.Type,
                        parameters);
                }
                else
                {
                    functionSymbol = new FunctionSymbol(function.Name, parameters);
                }

                this.RegisterFunction(function, functionSymbol, parent);
            }
        }

        private void RegisterFunction(FunctionDecleration node, FunctionSymbol functionSymbol, SymbolTable parent)
        {
            functionSymbol.SymbolTable = parent.CreateNestedSymbolTable();

            if (parent.IsRegisteredInScope(functionSymbol.Name, SymbolType.Function))
            {
                this.logger.LogError(node.Location, "The function '{0}' is already defined", functionSymbol.Name);
            }
            else
            {
                parent.RegisterSymbol(functionSymbol);
                node.Symbol = functionSymbol;
            }

            foreach (var parameter in node.Parameters)
            {
                var parameterSymbol = new VariableSymbol(parameter.Name.Name, parameter.Type.Type);

                if (functionSymbol.SymbolTable.IsRegisteredInScope(parameter.Name.Name, SymbolType.Variable))
                {
                    this.logger.LogError(node.Location, "The parameter '{0}' is already defined", parameterSymbol.Name);
                }
                else
                {
                    functionSymbol.SymbolTable.RegisterSymbol(parameterSymbol);
                    parameter.Name.Symbol = parameterSymbol;
                }
            }
        }

        public override void Visit(UnInitializedVariableDecleration node)
        {
            this.Visit(node);
        }

        public override void Visit(InitializedVariableDecleration node)
        {
            this.Visit(node);
            node.Initialization.Accept(this);
        }

        private void Visit(VariableDecleration variableDecleration)
        {
            var variable = variableDecleration.Variable;

            var symbol = new VariableSymbol(variable.Name.Name, variable.Type.Type);

            if (this.currentSymbolTable.IsRegisteredInScope(symbol.Name, SymbolType.Variable))
            {
                this.logger.LogError(variableDecleration.Location, "The variable '{0}' is already defined in the current scope", symbol.Name);
            }
            else
            {
                this.currentSymbolTable.RegisterSymbol(symbol);
                variableDecleration.Variable.Name.Symbol = symbol;
            }
        }

        public override void Visit(ForStatement node)
        {
            node.Condition.Accept(this);
            node.Afterthought.Accept(this);
            node.Initialization.Accept(this);

            var parent = this.currentSymbolTable;
            this.currentSymbolTable = parent.CreateNestedSymbolTable();

            foreach (var statements in node.Body)
            {
                statements.Accept(this);
            }

            this.currentSymbolTable = parent;
        }

        public override void Visit(IfStatement node)
        {
            node.Condition.Accept(this);

            var parent = this.currentSymbolTable;
            this.currentSymbolTable = parent.CreateNestedSymbolTable();

            foreach (var statements in node.Body)
            {
                statements.Accept(this);
            }

            this.currentSymbolTable = parent;
        }

        public override void Visit(WhileStatement node)
        {
            node.Condition.Accept(this);

            var parent = this.currentSymbolTable;
            this.currentSymbolTable = parent.CreateNestedSymbolTable();

            foreach (var statements in node.Body)
            {
                statements.Accept(this);
            }

            this.currentSymbolTable = parent;
        }

        public override void Visit(VariableIdNode node)
        {
            node.Symbol = this.currentSymbolTable.GetSymbol(node.Name, SymbolType.Variable);

            if (node.Symbol == null)
            {
                this.logger.LogError(node.Location, "The variable '{0}' is not defined before it is used.", node.Name);
                node.Symbol = new VariableSymbol("Dummy", new Type(PrimitiveType.NoType));
            }
        }

        public override void Visit(FunctionCallExpression node)
        {
            var symbol = this.currentSymbolTable.GetSymbol(node.Name, SymbolType.Function);

            node.Symbol = symbol;

            base.Visit(node);
        }
    }
}
