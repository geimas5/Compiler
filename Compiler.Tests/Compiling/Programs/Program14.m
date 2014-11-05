int main()
{
    PrintDouble(4 + 0.4); PrintLine("");
    PrintDouble(4 - 2.2); PrintLine("");
    PrintDouble(4 / 2.2); PrintLine("");
    PrintDouble(4 * 2.2); PrintLine("");
    PrintDouble(4 ** 2.2); PrintLine("");

    PrintDouble(4.4 + 2); PrintLine("");
    PrintDouble(4.4 - 2); PrintLine("");
    PrintDouble(4.4 / 2); PrintLine("");
    PrintDouble(4.4 * 2); PrintLine("");
    PrintDouble(4.4 ** 2); PrintLine("");

    PrintDouble(-4 + 3.3); PrintLine("");
    PrintDouble(-4.4 + 3); PrintLine("");

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