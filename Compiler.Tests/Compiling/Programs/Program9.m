int main()
{
	PrintBool(4 == 4); PrintLine("");
	PrintBool(43 == 4); PrintLine("");

	PrintBool(4 != 4); PrintLine("");
	PrintBool(43 != 4); PrintLine("");

	PrintBool(4 > 4); PrintLine("");
	PrintBool(43 > 4); PrintLine("");
	PrintBool(3 > 4); PrintLine("");

	PrintBool(4 >= 4); PrintLine("");
	PrintBool(43 >= 4); PrintLine("");
	PrintBool(3 >= 4); PrintLine("");

	PrintBool(4 <= 4); PrintLine("");
	PrintBool(43 <= 4); PrintLine("");
	PrintBool(3 <= 4); PrintLine("");
    
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