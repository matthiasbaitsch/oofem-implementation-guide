import numpy as np


A = np.zeros((3, 3))
b = np.zeros(3)

A[0, 0] = 4
A[0, 1] = 1
A[0, 2] = 0

A[1, 0] = 1
A[1, 1] = 3
A[1, 2] = 1

A[2, 0] = 0
A[2, 1] = 1
A[2, 2] = 2

b[0] = 1
b[1] = 2
b[2] = 3

x = np.linalg.solve(A, b)

print("A =")
print(A)

print("b =")
print(b)

print("x =")
print(x)

residual = A @ x - b

print("Residual A*x - b =")
print(residual)

print("Norm of residual |A*x - b| =", np.linalg.norm(residual))