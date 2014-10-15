int main()
{
    int d = Fibonacci(10);

    if(d == 55){
        PrintLine("OK");
    }

    return 0;
}

int Fibonacci(int n)
{
   if ( n == 0 )
      return 0;
   else if ( n == 1 )
      return 1;
   
   return ( Fibonacci(n-1) + Fibonacci(n-2) );
} 