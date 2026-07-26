Matrix A = Matrix.Build.Dense(3, 3);
Vector b = Vector.Build.Dense(3);

A[0, 0] = 4; A[0, 1] = 1; A[0, 2] = 0;
A[1, 0] = 1; A[1, 1] = 3; A[1, 2] = 1;
A[2, 0] = 0; A[2, 1] = 1; A[2, 2] = 2;

b[0] = 1;
b[1] = 2;
b[2] = 3;

Vector x = A.Solve(b);

Console.WriteLine("A =");
Console.WriteLine(A);
Console.WriteLine("b =");
Console.WriteLine(b);
Console.WriteLine("x =");
Console.WriteLine(x);
Console.WriteLine("Residual A*x - b =");
Console.WriteLine(A * x - b);
Console.WriteLine($"Norm of residual |A*x - b| = {(A * x - b).Norm(2)}");
