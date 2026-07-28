import numpy as np

from constraint import Constraint
from force import Force


class Node:

    def __init__(self, x1: float, x2: float, x3: float):

        self.force = Force()
        self.constraint = Constraint()

        self.position = np.zeros(3)
        self.displacement = np.zeros(3)

        self.dofs = [-1, -1, -1]

        self.position[0] = x1
        self.position[1] = x2
        self.position[2] = x3

    def enumerate_dofs(self, start: int) -> int:

        for i in range(3):

            if not self.constraint.fixed[i]:

                self.dofs[i] = start
                start += 1

        return start

    def __str__(self) -> str:

        dimension = 3
        text = ""

        for i in range(dimension):
            text += f"{self.position[i]:12.4f}"

        for i in range(dimension):

            if self.constraint.fixed[i]:
                text += "      X"
            else:
                text += "       "

        for i in range(dimension):

            force_component = self.force.components[i]

            if force_component != 0:
                text += f"{force_component:12.2f}"
            else:
                text += "            "

        return text