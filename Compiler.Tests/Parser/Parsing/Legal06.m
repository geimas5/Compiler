int main() {
    for ( c = 0 ; c < n ; c=c+1 )
   {
      if ( c <= 1 )
         next = c;
      else
      {
         next = first + second;
         first = second;
         second = next;
      }
      printf("%d",next);
   }
}