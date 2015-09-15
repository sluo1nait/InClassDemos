<Query Kind="Program" />

void Main()
{
	// simple concatenation
	//"hello world"
	//5+7
	
	//simple C# statements
	//string name = "Jessica";
	//string message = "hello " + name;
	//message.Dump();
	
	//simple C# program
	//subroutine call
	SayHello ("Jessica");
}

// Define other methods and classes here
public void SayHello(string name)
{
  string message = "hello "+ name;
  message.Dump();
}