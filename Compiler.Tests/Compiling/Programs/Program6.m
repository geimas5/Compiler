int main()
{
    PrintBool(true == true);PrintLine("");
    PrintBool(false == true);PrintLine("");
    PrintBool(true == false);PrintLine("");
    PrintBool(false == false);PrintLine("");

    PrintLine("----------");
    PrintBool(true != true);PrintLine("");
    PrintBool(false != true);PrintLine("");
    PrintBool(true != false);PrintLine("");
    PrintBool(false != false);PrintLine("");

    PrintLine("----------");
    PrintBool(!true);PrintLine("");
    PrintBool(!false);PrintLine("");

    PrintLine("----------");
    PrintBool(true && true); PrintLine("");
    PrintBool(true && false); PrintLine("");
    PrintBool(false && true); PrintLine("");
    PrintBool(false && false); PrintLine("");

    PrintLine("----------");
    PrintBool(true || true); PrintLine("");
    PrintBool(true || false); PrintLine("");
    PrintBool(false || true); PrintLine("");
    PrintBool(false || false); PrintLine("");

    PrintBool(GetTrue() == GetTrue()); PrintLine("");
    PrintBool(GetFalse() == GetTrue()); PrintLine("");
    PrintBool(GetTrue() == GetFalse()); PrintLine("");
    PrintBool(GetFalse() == GetFalse()); PrintLine("");

    PrintLine("----------");
    PrintBool(GetTrue() != GetTrue()); PrintLine("");
    PrintBool(GetFalse() != GetTrue()); PrintLine("");
    PrintBool(GetTrue() != GetFalse()); PrintLine("");
    PrintBool(GetFalse() != GetFalse()); PrintLine("");

    PrintLine("----------");
    PrintBool(!GetTrue()); PrintLine("");
    PrintBool(!GetFalse()); PrintLine("");

    PrintLine("----------");
    PrintBool(GetTrue() && GetTrue());PrintLine("");
    PrintBool(GetTrue() && GetFalse());PrintLine("");
    PrintBool(GetFalse() && GetTrue());PrintLine("");
    PrintBool(GetFalse() && GetFalse());PrintLine("");

    PrintLine("----------");
    PrintBool(GetTrue() || GetTrue());PrintLine("");
    PrintBool(GetTrue() || GetFalse());PrintLine("");
    PrintBool(GetFalse() || GetTrue());PrintLine("");
    PrintBool(GetFalse() || GetFalse());PrintLine("");
    
    return 0;
}

void PrintBool(bool val)
{
    if (val)
    {
        PrintInt(1);
    }
    else
    {
        PrintInt(0);
    }
}

bool GetTrue()
{
    PrintBool(true);
    return true;
}

bool GetFalse()
{
    PrintBool(false);
    return false;
}
