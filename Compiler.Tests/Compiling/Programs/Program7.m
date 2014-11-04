int main()
{
    int[] v = new int[6];
    v[0] = 443 + 23;
    v[1] = 32 - 223;
    v[2] = 23 * 34;
    v[3] = 453 / 32;
    v[4] = 23 % 5;
	v[5] = -v[4];

    PrintInt(v[0]); PrintLine("");
    PrintInt(v[1]); PrintLine("");
    PrintInt(v[2]); PrintLine("");
    PrintInt(v[3]); PrintLine("");
    PrintInt(v[4]); PrintLine("");
	PrintInt(v[5]); PrintLine("");

    return 0;
}