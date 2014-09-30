int main()
{
  int n;
  int c;
  int k;
  int space = 1;
 
  printf("Enter number of rowsn");
  scanf("%d", n);
 
  space = n - 1;
 
  for (k = 1; k <= n; k=k+1)
  {
    for (c = 1; c <= space; c=c+1)
      printf(" ");
 
    space=space -1;
 
    for (c = 1; c <= 2*k-1; c=c+1)
      printf("*");
 
    printf("n");
  }
 
  space = 1;
 
  for (k = 1; k <= n - 1; k=k+1)
  {
    for (c = 1; c <= space; c=c+1)
      printf(" ");
 
    space=space +1;
 
    for (c = 1 ; c <= 2*(n-k)-1; c=c+1)
      printf("*");
 
    printf("n");
  }
 
  return 0;
}

void printf(string input){
    
}

void scanf(string input, int n){
}