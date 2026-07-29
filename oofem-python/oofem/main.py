from structure import Structure


structure = Structure()

# Nodes
node1 = structure.add_node(0.0, 0.0, 0.0)
node1.constraint.fixed[0] = True
node1.constraint.fixed[1] = True
node1.constraint.fixed[2] = True

node2 = structure.add_node(4.0, 0.0, 0.0)
node2.constraint.fixed[1] = True
node2.constraint.fixed[2] = True

node3 = structure.add_node(0.0, 3.0, 0.0)
node3.constraint.fixed[2] = True
node3.force.components[0] = 1200.0

# Elements
element1 = structure.add_element(10e9, 1e-4, 0, 1)

element2 = structure.add_element(10e9, 1e-4, 0, 2)

element3 = structure.add_element(10e9, 1e-4, 2, 1)

# Print model data
structure.print()

print("\nK3")
print(element3.stiffness_matrix())

# Degrees of freedom
number_of_dofs = structure.enumerate_dofs()

print(f"\nNumber of DOFs: {number_of_dofs}")

for i, node in enumerate(structure.nodes):
    print(f"Node {i} has DOFs {node.dofs}")

for i, element in enumerate(structure.elements):
    print(f"Element {i} has DOFs {element.dofs()}")

# Solve
structure.solve()

for i, node in enumerate(structure.nodes):
    print(
        f"Node {i} has displacement "
        f"{node.displacement}"
    )

# Normal force
print(f"\nN3 = {element3.normal_force()}")

structure.print_results()