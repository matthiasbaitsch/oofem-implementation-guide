import numpy as np


# Vector of zeros
a = np.zeros(3)

a[0] = 1
a[1] = 2


# Vector of given values
b = np.array([4.1, 5.9, 6.3])


# Print
print("a =", a)
print("b =", b)


# Vector arithmetic
c = -a + 3 * b

print("-a + 3 * b =", c)


# Norm and dot product
print("|a| =", np.linalg.norm(a))
print("a ⋅ b =", np.dot(a, b))


# Outer product: turns two vectors into a matrix
A = np.outer(a, b)

print("a * b^T =")
print(A)


# Appending vectors
d = np.concatenate((a, b))

print("Concatenation of a and b =", d)