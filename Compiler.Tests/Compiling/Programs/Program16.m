int main()
{
    IntTest1(111);
	IntTest2(111, 222);
	IntTest3(111, 222, 333);
	IntTest4(111, 222, 333, 444);
	IntTest5(111, 222, 333, 444, 555);
	IntTest6(111, 222, 333, 444, 555, 666);

	DoubleTest1(111.0);
	DoubleTest2(111.0, 222.0);
	DoubleTest3(111.0, 222.0, 333.0);
	DoubleTest4(111.0, 222.0, 333.0, 444.0);
	DoubleTest5(111.0, 222.0, 333.0, 444.0, 555.0);
	DoubleTest6(111.0, 222.0, 333.0, 444.0, 555.0, 666.0);

    return 0;
}

void IntTest1(int a){
	PrintInt(a);
	PrintLine("");
}

void IntTest2(int a, int b){
	PrintInt(a);
	PrintInt(b);
	PrintLine("");
}

void IntTest3(int a, int b, int c){
	PrintInt(a);
	PrintInt(b);
	PrintInt(c);
	PrintLine("");
}

void IntTest4(int a, int b, int c, int d){
	PrintInt(a);
	PrintInt(b);
	PrintInt(c);
	PrintInt(d);
	PrintLine("");
}

void IntTest5(int a, int b, int c, int d, int e){
	PrintInt(a);
	PrintInt(b);
	PrintInt(c);
	PrintInt(d);
	PrintInt(e);
	PrintLine("");
}

void IntTest6(int a, int b, int c, int d, int e, int f){
	PrintInt(a);
	PrintInt(b);
	PrintInt(c);
	PrintInt(d);
	PrintInt(e);
	PrintInt(f);
	PrintLine("");
}



void DoubleTest1(double a){
	PrintDouble(a);
	PrintLine("");
}

void DoubleTest2(double a, double b){
	PrintDouble(a);
	PrintDouble(b);
	PrintLine("");
}

void DoubleTest3(double a, double b, double c){
	PrintDouble(a);
	PrintDouble(b);
	PrintDouble(c);
	PrintLine("");
}

void DoubleTest4(double a, double b, double c, double d){
	PrintDouble(a);
	PrintDouble(b);
	PrintDouble(c);
	PrintDouble(d);
	PrintLine("");
}

void DoubleTest5(double a, double b, double c, double d, double e){
	PrintDouble(a);
	PrintDouble(b);
	PrintDouble(c);
	PrintDouble(d);
	PrintDouble(e);
	PrintLine("");
}

void DoubleTest6(double a, double b, double c, double d, double e, double f){
	PrintDouble(a);
	PrintDouble(b);
	PrintDouble(c);
	PrintDouble(d);
	PrintDouble(e);
	PrintDouble(f);
	PrintLine("");
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