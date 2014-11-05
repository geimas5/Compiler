int main()
{
    if(4.0 == 4.0) { PrintBool(true); } else { PrintBool(false); } PrintLine("");
    if(43.0 == 4.0) { PrintBool(true); } else { PrintBool(false); } PrintLine("");
            
    if(4.0 != 4.0) { PrintBool(true); } else { PrintBool(false); } PrintLine("");
    if(43.0 != 4.0) { PrintBool(true); } else { PrintBool(false); } PrintLine("");
            
    if(4.0 > 4.0) { PrintBool(true); } else { PrintBool(false); } PrintLine("");
    if(43.0 > 4.0) { PrintBool(true); } else { PrintBool(false); } PrintLine("");
    if(3.0 > 4.0) { PrintBool(true); } else { PrintBool(false); } PrintLine("");
            
    if(4.0 >= 4.0) { PrintBool(true); } else { PrintBool(false); } PrintLine("");
    if(43.0 >= 4.0) { PrintBool(true); } else { PrintBool(false); } PrintLine("");
    if(3.0 >= 4.0) { PrintBool(true); } else { PrintBool(false); } PrintLine("");
            
    if(4.0 <= 4.0) { PrintBool(true); } else { PrintBool(false); } PrintLine("");
    if(43.0 <= 4.0) { PrintBool(true); } else { PrintBool(false); } PrintLine("");
    if(3.0 <= 4.0) { PrintBool(true); } else { PrintBool(false); } PrintLine("");

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