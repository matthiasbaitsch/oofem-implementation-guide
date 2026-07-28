import numpy as np

from node import Node
from element import Element


class Structure:

    def __init__(self):

        self.nodes = []
        self.elements = []

    def add_node(self, x1: float, x2: float, x3: float) -> Node:

        node = Node(x1, x2, x3)

        self.nodes.append(node)

        return node

    def add_element(self, e: float, a: float, n1: int, n2: int) -> Element:

        element = Element(e, a, self.nodes[n1], self.nodes[n2])

        self.elements.append(element)

        return element

    def enumerate_dofs(self) -> int:

        number_of_dofs = 0

        for node in self.nodes:

            number_of_dofs = node.enumerate_dofs(
                number_of_dofs
            )

        return number_of_dofs

    def solve(self):

        number_of_dofs = self.enumerate_dofs()

        stiffness_matrix = np.zeros(
            (number_of_dofs, number_of_dofs)
        )

        load_vector = np.zeros(number_of_dofs)

        # Assemble element stiffness matrices
        for element in self.elements:

            element_dofs = element.dofs()
            element_stiffness = element.stiffness_matrix()

            for i in range(len(element_dofs)):

                global_i = element_dofs[i]

                for j in range(len(element_dofs)):

                    global_j = element_dofs[j]

                    if global_i != -1 and global_j != -1:

                        stiffness_matrix[
                            global_i,
                            global_j,
                        ] += element_stiffness[i, j]

        # Assemble nodal forces
        for node in self.nodes:

            node_dofs = node.dofs

            for i in range(len(node_dofs)):

                global_i = node_dofs[i]

                if global_i != -1:

                    load_vector[global_i] += (
                        node.force.components[i]
                    )

        # Solve the linear system
        displacement_vector = np.linalg.solve(
            stiffness_matrix,
            load_vector,
        )

        # Store the calculated nodal displacements
        for node in self.nodes:

            node_dofs = node.dofs

            for i in range(len(node_dofs)):

                global_i = node_dofs[i]

                if global_i != -1:

                    node.displacement[i] = (
                        displacement_vector[global_i]
                    )

    def print(self):

        line = "─" * 93

        print(line)
        print("                                  N O D E S")
        print(line)

        print(
            "          Position"
            "                         Constraint"
            "                   Force"
        )

        print(
            "          X"
            "           Y"
            "           Z"
            "       Cx"
            "     Cy"
            "     Cz"
            "          Fx"
            "          Fy"
            "          Fz"
        )

        print(line)

        for i, node in enumerate(self.nodes):

            print(f"{i:3}{node}")

        print(line)

        print()
        print()

        print(line)
        print("                                E L E M E N T S")
        print(line)

        print(
            "                 E"
            "              A"
            "              L"
        )

        print(line)

        for i, element in enumerate(self.elements):

            print(f"{i:3}{element}")

        print(line)

    def print_results(self):

        line = "─" * 66

        print(line)
        print("                     N O D E   R E S U L T S")
        print(line)

        print(
            "                Ux"
            "             Uy"
            "             Uz"
        )

        print(line)

        for i, node in enumerate(self.nodes):

            print(
                f"{i:3}"
                f"{node.displacement[0]:15.6f}"
                f"{node.displacement[1]:15.6f}"
                f"{node.displacement[2]:15.6f}"
            )

        print(line)

        print()
        print()

        print(line)
        print("                  E L E M E N T   R E S U L T S")
        print(line)

        print("    Normal force N")

        print(line)

        for i, element in enumerate(self.elements):

            print(
                f"{i:3}"
                f"{element.normal_force():15.6f}"
            )

        print(line)