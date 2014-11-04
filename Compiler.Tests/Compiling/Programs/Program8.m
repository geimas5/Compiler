int main()
{
    double[] v = new double[6];
    v[0] = 443 + 23;
    v[1] = 32 - 223;
    v[2] = 23 * 34;
    v[3] = 453 / 32;
    v[4] = 23 ** 5;
	v[5] = -v[4];

    PrintDouble(v[0]); PrintLine("");
    PrintDouble(v[1]); PrintLine("");
    PrintDouble(v[2]); PrintLine("");
    PrintDouble(v[3]); PrintLine("");
    PrintDouble(v[4]); PrintLine("");
	PrintDouble(v[5]); PrintLine("");

	return 0;
}