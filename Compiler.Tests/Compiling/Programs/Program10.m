int main()
{
    PrintBool(4.0 == 4.0); PrintLine("");
    PrintBool(43.0 == 4.0); PrintLine("");
            
    PrintBool(4.0 != 4.0); PrintLine("");
    PrintBool(43.0 != 4.0); PrintLine("");
            
    PrintBool(4.0 > 4.0); PrintLine("");
    PrintBool(43.0 > 4.0); PrintLine("");
    PrintBool(3.0 > 4.0); PrintLine("");
            
    PrintBool(4.0 >= 4.0); PrintLine("");
    PrintBool(43.0 >= 4.0); PrintLine("");
    PrintBool(3.0 >= 4.0); PrintLine("");
            
    PrintBool(4.0 <= 4.0); PrintLine("");
    PrintBool(43.0 <= 4.0); PrintLine("");
    PrintBool(3.0 <= 4.0); PrintLine("");
    
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