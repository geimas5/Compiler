﻿int main()
{
   int c;
   int first;
   int last;
   int middle;
   int n;
   int search;
   int[] array;
 
   printf("Enter number of elements");
   scanf("%d",n);
 
   printf("Enter %d integers", n);
 
   for ( c = 0 ; c < n ; c=c+1 )
      scanf("%d",array[c]);
 
   printf("Enter value to findn");
   scanf("%d",search);
 
   first = 0;
   last = n - 1;
   middle = (first+last)/2;
 
   while( first <= last )
   {
      if ( array[middle] < search )
         first = middle + 1;    
      else if ( array[middle] == search ) 
      {
         printf("%d found at location %d.n", search, middle+1);
         break;
      }
      else
         last = middle - 1;
 
      middle = (first + last)/2;
   }
   if ( first > last )
      printf("Not found! %d is not present in the list.n", search);
 
   return 0;   
}