int main(){
   int r;
   int number = 1000;
   int c;
   int sum = 0;
   int temp;

   for( c = 1 ; c <= number ; c = c + 1 )
   {
      temp = c;
      while( temp != 0 )
      {
         r = temp % 10;
         sum = sum + r * r * r;
         temp = temp / 10;
      }

      if ( c == sum ){
        PrintInt(c);
        PrintLine("");
      }

      sum = 0;
   }
 
   return 0;
}