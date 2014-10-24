int main()
{
    int[] d = new int[6];

    d[0] = 1;
    d[1] = 2;
    d[2] = 3;       
    d[3] = 4;
    d[4] = 5;
    d[5] = 6;       

    BinarySearch(2, d, 0, 5);

    return 1;
}
void BinarySearch(int x, int[] a, int m, int n)
{
    int middle=(m+n)/2;

    if(a[middle]==x)
    {
        PrintLine("Found it!");
        PrintInt(middle + 1);
    }
    else if(x > a[middle])
        BinarySearch(x, a, middle, n);
    else if(x < a[middle])
        BinarySearch(x, a, m, middle);
}