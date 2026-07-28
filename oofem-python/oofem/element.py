import numpy as np

from node import Node


class Element:

    def __init__(
        self,
        e: float,
        a: float,
        node1: Node,
        node2: Node,
    ):

        self.e = e
        self.a = a
        self.node1 = node1
        self.node2 = node2

    def length(self) -> float:

        difference = self.node2.position - self.node1.position

        return np.linalg.norm(difference)

    def d(self) -> np.ndarray:

        return (
            self.node2.position - self.node1.position
        ) / self.length()

    def t(self) -> np.ndarray:

        e1 = self.d()

        return np.concatenate((-e1, e1))

    def stiffness_matrix(self) -> np.ndarray:

        return (
            self.e
            * self.a
            / self.length()
            * np.outer(self.t(), self.t())
        )

    def dofs(self) -> np.ndarray:

        return np.concatenate(
            (self.node1.dofs, self.node2.dofs)
        )

    def displacement(self) -> np.ndarray:

        return np.concatenate(
            (
                self.node1.displacement,
                self.node2.displacement,
            )
        )

    def normal_force(self) -> float:

        return (
            self.e
            * self.a
            / self.length()
            * np.dot(self.t(), self.displacement())
        )

    def __str__(self) -> str:

        return (
            f"{self.e:15.0f}"
            f"{self.a:15.6f}"
            f"{self.length():15.4f}"
        )