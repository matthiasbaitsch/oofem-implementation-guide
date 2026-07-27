// Vector of zeros
Vector a = Vector.Build.Dense(3);
a[0] = 1;
a[1] = 2;

// Vector of given values
Vector b = Vector.Build.DenseOfArray([4.1, 5.9, 6.3]);

// Print
Console.WriteLine("a = " + a);
Console.WriteLine("b = " + b);

// Vector arithmetic
Vector c = -a + 3 * b;
Console.WriteLine("-a + 3 * b = " + c);

// Norm and dot product
Console.WriteLine($"|a| = {a.Norm(2)}");
Console.WriteLine($"a ⋅ b = {a * b}");

// Outer product: turns two vectors into a matrix
Matrix A = Vector.OuterProduct(a, b);
Console.WriteLine("a * b^T = " + A);

// Appending vectors
Vector d = Vector.Build.DenseOfEnumerable(a.Concat(b));
Console.WriteLine("Concatenation of a and b = " + d);

